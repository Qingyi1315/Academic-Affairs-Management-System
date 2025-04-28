using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System;
using System.IO;
using System.Threading.Tasks;

public class ElGamalHelper
{
    // 使用2048位安全密钥
    public static AsymmetricCipherKeyPair GenerateKeyPair()
    {
        var generator = new ElGamalParametersGenerator();
        generator.Init(2048, 30, new SecureRandom());
        var parameters = generator.GenerateParameters();

        var keyGen = new ElGamalKeyPairGenerator();
        keyGen.Init(new ElGamalKeyGenerationParameters(new SecureRandom(), parameters));
        return keyGen.GenerateKeyPair();
    }

    // 异步加密方法（带进度回调）
    public static async Task<string> EncryptAsync(
        string inputPath,
        ElGamalPublicKeyParameters publicKey,
        IProgress<double> progress = null)
    {
        var engine = new ElGamalEngine();
        engine.Init(true, publicKey);

        int inputBlockSize = engine.GetInputBlockSize();
        int outputBlockSize = engine.GetOutputBlockSize();
        string outputPath = $"{inputPath}.elgamal.enc";

        using (var input = File.OpenRead(inputPath))
        using (var output = File.Create(outputPath))
        {
            long totalBytes = input.Length;
            long processedBytes = 0;
            byte[] buffer = new byte[inputBlockSize];

            while (true)
            {
                int bytesRead = await input.ReadAsync(buffer, 0, inputBlockSize);
                if (bytesRead <= 0) break;

                // 异步处理加密块
                byte[] encrypted = await Task.Run(() =>
                    engine.ProcessBlock(buffer, 0, bytesRead));

                await output.WriteAsync(encrypted, 0, encrypted.Length);

                // 更新进度
                processedBytes += bytesRead;
                progress?.Report((double)processedBytes / totalBytes);
            }
        }

        return outputPath;
    }

    // 异步解密方法（带取消功能）
    public static async Task<string> DecryptAsync(
        string inputPath,
        ElGamalPrivateKeyParameters privateKey,
        IProgress<double> progress = null,
        System.Threading.CancellationToken cancellationToken = default)
    {
        var engine = new ElGamalEngine();
        engine.Init(false, privateKey);

        int inputBlockSize = engine.GetInputBlockSize();
        int outputBlockSize = engine.GetOutputBlockSize();
        string outputPath = Path.ChangeExtension(inputPath, "dec");

        using (var input = File.OpenRead(inputPath))
        using (var output = File.Create(outputPath))
        {
            long totalBytes = input.Length;
            long processedBytes = 0;
            byte[] buffer = new byte[inputBlockSize];

            while (!cancellationToken.IsCancellationRequested)
            {
                int bytesRead = await input.ReadAsync(buffer, 0, inputBlockSize, cancellationToken);
                if (bytesRead <= 0) break;

                byte[] decrypted = await Task.Run(() =>
                    engine.ProcessBlock(buffer, 0, bytesRead),
                    cancellationToken);

                await output.WriteAsync(decrypted, 0, decrypted.Length, cancellationToken);

                processedBytes += bytesRead;
                progress?.Report((double)processedBytes / totalBytes);
            }
        }

        return outputPath;
    }

    // Save public key to PEM file
    public static void SavePublicKey(string path, ElGamalPublicKeyParameters publicKey)
    {
        string dir = Path.GetDirectoryName(path);
        string outpath = dir + "publicKey.pem";
        using (var writer = File.CreateText(outpath))
        {
            var pemWriter = new Org.BouncyCastle.OpenSsl.PemWriter(writer);
            pemWriter.WriteObject(publicKey);
        }
    }

    // Load public key from PEM file
    public static ElGamalPublicKeyParameters LoadPublicKey(string path)
    {
        string dir = Path.GetDirectoryName(path);
        string outpath = dir + "publicKey.pem";
        using (var reader = File.OpenText(path))
        {
            var pemReader = new Org.BouncyCastle.OpenSsl.PemReader(reader);
            return (ElGamalPublicKeyParameters)pemReader.ReadObject();
        }
    }

    // Save private key to PEM file
    public static void SavePrivateKey(string path, ElGamalPrivateKeyParameters privateKey)
    {
        string dir = Path.GetDirectoryName(path);
        string outpath = dir + "privateKey.pem";
        using (var writer = File.CreateText(path))
        {
            var pemWriter = new Org.BouncyCastle.OpenSsl.PemWriter(writer);
            pemWriter.WriteObject(privateKey);
        }
    }

    // Load private key from PEM file
    public static ElGamalPrivateKeyParameters LoadPrivateKey(string path)
    {
        string dir = Path.GetDirectoryName(path);
        string outpath = dir + "privateKey.pem";
        using (var reader = File.OpenText(path))
        {
            var pemReader = new Org.BouncyCastle.OpenSsl.PemReader(reader);
            return (ElGamalPrivateKeyParameters)pemReader.ReadObject();
        }
    }
}
