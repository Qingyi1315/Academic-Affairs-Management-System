using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 教务管理系统
{
    public partial class fileED : Form
    {
        Dictionary<string, List<string>> encryptionCategories = new Dictionary<string, List<string>>();
        // 生成自签名证书
        X509Certificate2 certificate = GenerateSelfSignedCertificate("CN=MyTestCertificate", 2048, "password123");
        /*        AsymmetricCipherKeyPair keyPair = null;
                ElGamalPublicKeyParameters publicKey = null;
                ElGamalPrivateKeyParameters privateKey = null;*/

        public fileED()
        {
            InitializeComponent();

            encryptionCategories.Add("哈希算法", new List<string> { "MD5", "SHA1", "SHA256", "SHA384", "SHA512" });
            encryptionCategories.Add("对称加密", new List<string> { "AES", "TripleDES", "DES", "RC2", "Rijndael" });
            encryptionCategories.Add("非对称加密", new List<string> { "RSA" });

            foreach (var category in encryptionCategories.Keys)
            {
                comboBox1.Items.Add(category);
            }

            comboBox1.SelectedIndex = 0;

            listView1.DragDrop += listView1_DragDrop;
            listView1.DragEnter += listView1_DragEnter;
            listView1.ColumnWidthChanging += ListView1_ColumnWidthChanging;

            label1.Enabled = false;
            label1.AllowDrop = false;
            label1.BringToFront();

            // 为控件绑定事件
            全选toolStripButton1.Click += 全选toolStripButton1_Click;
            取消全选toolStripButton1.Click += 取消全选toolStripButton1_Click;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            checkedListBox1.ItemCheck += checkedListBox1_ItemCheck;
            checkedListBox1.MouseDoubleClick += checkedListBox1_MouseDoubleClick;

            UpdateMaskVisibility();
        }

        private void ListView1_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            e.Cancel = true;
            e.NewWidth = listView1.Columns[e.ColumnIndex].Width;
        }

        private void fileED_Load(object sender, EventArgs e)
        {
            UpdateMaskVisibility();
        }

        private void UpdateMaskVisibility()
        {
            label1.Visible = listView1.Items.Count == 0;
        }

        private void listView1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void listView1_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                List<string> skippedFiles = new List<string>();

                HashSet<string> existingPaths = new HashSet<string>(
                    listView1.Items
                        .Cast<ListViewItem>()
                        .Select(item => item.SubItems[1].Text),
                    StringComparer.OrdinalIgnoreCase
                );

                foreach (string file in files)
                {
                    string filePath = file;
                    if (existingPaths.Contains(filePath))
                    {
                        skippedFiles.Add(filePath);
                        continue;
                    }

                    string fileName = Path.GetFileName(file);
                    Icon fileIcon = Icon.ExtractAssociatedIcon(file);

                    if (!imageList1.Images.ContainsKey(fileName))
                    {
                        imageList1.Images.Add(fileName, fileIcon);
                    }

                    ListViewItem item = new ListViewItem(fileName, fileName);
                    item.SubItems.Add(file);
                    listView1.Items.Add(item);
                }

                if (skippedFiles.Count > 0)
                {
                    MessageBox.Show($"已跳过 {skippedFiles.Count} 个重复文件:\n" + string.Join("\n", skippedFiles));
                }

                UpdateMaskVisibility();
            }
        }

        private void 全选toolStripButton1_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.Items)
            {
                item.Checked = true;
            }
        }

        private void 取消全选toolStripButton1_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.Items)
            {
                item.Checked = false;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            checkedListBox1.Items.Clear();
            string selectedCategory = comboBox1.SelectedItem.ToString();

            if (encryptionCategories.TryGetValue(selectedCategory, out List<string> items))
            {
                foreach (var item in items)
                {
                    checkedListBox1.Items.Add(item);
                }
            }
        }

        private void checkedListBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = checkedListBox1.IndexFromPoint(e.Location);
            if (index != ListBox.NoMatches)
            {
                checkedListBox1.SetItemChecked(index, !checkedListBox1.GetItemChecked(index));
            }
        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked)
            {
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    if (i != e.Index)
                    {
                        checkedListBox1.SetItemChecked(i, false);
                    }
                }
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count <= 0)
            {
                MessageBox.Show("请先选择要加密的文件");
                return;
            }

            if (checkedListBox1.CheckedItems.Count <= 0)
            {
                MessageBox.Show("请至少选择一个加密算法");
                return;
            }

            button1.Enabled = false;
            progressBar1.Value = 0;
            progressBar1.Maximum = listView1.SelectedItems.Count;

            try
            {
                var filePaths = listView1.SelectedItems
                    .Cast<ListViewItem>()
                    .Select(item => item.SubItems[1].Text)
                    .Where(File.Exists)
                    .ToList();

                foreach (var checkedItem in checkedListBox1.CheckedItems)
                {
                    string algorithm = checkedItem.ToString();

                    // 并行处理多个文件
                    await Task.Run(() =>
                    {
                        Parallel.ForEach(filePaths, new ParallelOptions { MaxDegreeOfParallelism = 2 }, filePath =>
                        {
                            ProcessFile(filePath, algorithm);
                            this.Invoke((MethodInvoker)delegate
                            {
                                progressBar1.Value++;
                            });
                        });
                    });
                }
            }
            catch (Exception ex)
            {
                AppendLog($"全局错误: {ex.Message}");
            }
            finally
            {
                button1.Enabled = true;
            }
        }

        private void AppendLog(string message)
        {
            string log = $"[{DateTime.Now:HH:mm:ss}] {message}";
            MessageBox.Show(log);
        }

        // 解密文件模块
        private void button2_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count <= 0)
            {
                MessageBox.Show("请先选择要解密的文件");
                return;
            }

            if (checkedListBox1.CheckedItems.Count <= 0)
            {
                MessageBox.Show("请至少选择一个加密算法");
                return;
            }

            foreach (ListViewItem fileItem in listView1.SelectedItems)
            {
                string filePath = fileItem.SubItems[1].Text;

                foreach (var checkedItem in checkedListBox1.CheckedItems)
                {
                    string algorithm = checkedItem.ToString();

                    try
                    {
                        if (encryptionCategories["哈希算法"].Contains(algorithm))
                        {
                            string hash = ComputeHash(filePath, algorithm);
                            AppendLog($"{algorithm}哈希值: {hash}");
                        }
                        else if (encryptionCategories["对称加密"].Contains(algorithm))
                        {
                            (byte[] key, byte[] iv) = ReadKeyAndIV(filePath, algorithm);
                            string decryptedFilePath = DecryptFile(filePath, key, iv, algorithm);
                            AppendLog($"文件已解密，解密后的文件路径：{decryptedFilePath}");
                        }
                        else if (encryptionCategories["非对称加密"].Contains(algorithm))
                        {
                            if (algorithm.Equals("RSA", StringComparison.OrdinalIgnoreCase))
                            {
                                string decryptedFilePath = DecryptFile(filePath, certificate);
                                AppendLog($"{algorithm}解密完成: {decryptedFilePath}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        AppendLog($"解密失败 - {algorithm}: {ex.Message}");
                    }
                }
            }
        }

        private string ComputeHash(string filePath, string algorithm)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("文件未找到", filePath);

            using (var hashAlgorithm = HashAlgorithm.Create(algorithm))
            {
                if (hashAlgorithm == null)
                    throw new ArgumentException("不支持的哈希算法", algorithm);

                using (var stream = File.OpenRead(filePath))
                {
                    byte[] hashBytes = hashAlgorithm.ComputeHash(stream);
                    return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
                }
            }
        }

        // 对称加密（使用随机密钥）
        public string EncryptFile(string filePath, string algorithm)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("文件未找到", filePath);

            if (string.IsNullOrWhiteSpace(algorithm))
                throw new ArgumentException("算法不能为空", nameof(algorithm));

            algorithm = algorithm.ToUpper();

            using (SymmetricAlgorithm algo = GetAlgorithm(algorithm))
            {
                algo.GenerateKey(); // 生成密钥
                algo.GenerateIV();  // 生成初始化向量

                // 输出文件路径
                string outputPath = $"{filePath}.{algorithm.ToLower()}.enc";

                using (FileStream input = new FileStream(filePath, FileMode.Open))
                using (FileStream output = new FileStream(outputPath, FileMode.Create))
                using (CryptoStream crypto = new CryptoStream(output, algo.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    input.CopyTo(crypto);
                }

                // 注意：实际应用中需要保存密钥和 IV，以便解密时使用
                SaveKeyAndIV(algo.Key, algo.IV, filePath, algorithm);

                return outputPath;
            }
        }

        // 获取对称算法实现
        private SymmetricAlgorithm GetAlgorithm(string algorithm)
        {
            switch (algorithm)
            {
                case "AES":
                    return Aes.Create();
                case "DES":
                    return DES.Create();
                case "RC2":
                    return RC2.Create();
                case "Rijndael":
                    return Rijndael.Create();
                case "TripleDES":
                    return TripleDES.Create();
                default:
                    throw new ArgumentException("不支持的对称加密算法", nameof(algorithm));
            }
        }

        // 保存密钥和 IV
        private void SaveKeyAndIV(byte[] key, byte[] iv, string filePath, string algorithm)
        {
            string keyPath = $"{filePath}.{algorithm.ToLower()}.enc.key";
            using (FileStream keyFile = new FileStream(keyPath, FileMode.Create))
            {
                keyFile.Write(key, 0, key.Length);
            }

            string ivPath = $"{filePath}.{algorithm.ToLower()}.enc.iv";
            using (FileStream ivFile = new FileStream(ivPath, FileMode.Create))
            {
                ivFile.Write(iv, 0, iv.Length);
            }
        }

        // 解密文件
        public string DecryptFile(string encryptedFilePath, byte[] key, byte[] iv, string algorithm)
        {
            if (!File.Exists(encryptedFilePath))
                throw new FileNotFoundException("加密文件未找到", encryptedFilePath);

            if (key == null || iv == null)
                throw new ArgumentNullException("密钥或 IV 不能为空");

            if (string.IsNullOrWhiteSpace(algorithm))
                throw new ArgumentException("算法不能为空", nameof(algorithm));

            algorithm = algorithm.ToUpper();

            using (SymmetricAlgorithm algo = GetAlgorithm(algorithm))
            {
                // 设置密钥和 IV
                algo.Key = key;
                algo.IV = iv;

                // 输出文件路径
                string outputPath = $"{encryptedFilePath}.dec";

                using (FileStream input = new FileStream(encryptedFilePath, FileMode.Open))
                using (FileStream output = new FileStream(outputPath, FileMode.Create))
                using (CryptoStream crypto = new CryptoStream(output, algo.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    input.CopyTo(crypto);
                }

                return outputPath;
            }
        }

        // 读取密钥和 IV
        public (byte[] key, byte[] iv) ReadKeyAndIV(string filePath, string algorithm)
        {
            string keyPath = $"{filePath}.{algorithm.ToLower()}.key";
            string ivPath = $"{filePath}.{algorithm.ToLower()}.iv";

            if (!File.Exists(keyPath) || !File.Exists(ivPath))
                throw new FileNotFoundException("密钥或 IV 文件未找到");

            byte[] key = File.ReadAllBytes(keyPath);
            byte[] iv = File.ReadAllBytes(ivPath);

            return (key, iv);
        }

        // 非对称加密（使用自签名证书）
        // 生成自签名证书（RSA）
        public static X509Certificate2 GenerateSelfSignedCertificate(string subjectName, int keySize, string password)
        {
            // 创建证书主题
            var dn = new X500DistinguishedName(subjectName);

            // 创建 RSA 密钥对
            using (RSA rsa = RSA.Create(keySize))
            {
                // 创建证书请求
                var request = new CertificateRequest(dn, rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

                // 设置证书的增强型密钥用法
                request.CertificateExtensions.Add(new X509EnhancedKeyUsageExtension(
                    new OidCollection { new Oid("1.3.6.1.5.5.7.3.3") }, // 客户端认证
                    critical: false));

                // 设置证书的关键用法
                request.CertificateExtensions.Add(new X509KeyUsageExtension(
                    X509KeyUsageFlags.DataEncipherment | X509KeyUsageFlags.KeyEncipherment | X509KeyUsageFlags.DigitalSignature,
                    critical: true));

                // 创建自签名证书
                using (X509Certificate2 certificate = request.CreateSelfSigned(
                    notBefore: DateTime.UtcNow.AddDays(-1),
                    notAfter: DateTime.UtcNow.AddYears(10)))
                {
                    // 导出证书（包含私钥）到 PFX 文件
                    byte[] pfxData = certificate.Export(X509ContentType.Pfx, password);
                    File.WriteAllBytes("certificate.pfx", pfxData);

                    // 导出公钥到 CER 文件
                    byte[] cerData = certificate.Export(X509ContentType.Cert);
                    File.WriteAllBytes("certificate.cer", cerData);

                    return new X509Certificate2(pfxData, password);
                }
            }
        }

        // 加密文件（RSA）
        // 修改后的EncryptAsymmetricRSA方法，使其支持异步操作
        public async Task<string> EncryptAsymmetricRSAAsync(string inputFilePath, X509Certificate2 certificate, string algorithm)
        {
            if (!File.Exists(inputFilePath))
                throw new FileNotFoundException("文件未找到", inputFilePath);

            if (string.IsNullOrWhiteSpace(algorithm))
                throw new ArgumentException("算法不能为空", nameof(algorithm));

            // 读取文件内容
            byte[] data = await File.ReadAllBytesAsync(inputFilePath); // 异步读取文件内容

            using (RSA rsa = certificate.PublicKey.Key as RSA)
            {
                if (rsa == null)
                    throw new ArgumentException("证书不包含有效的 RSA 公钥");

                byte[] encryptedData = await Task.Run(() => rsa.Encrypt(data, RSAEncryptionPadding.Pkcs1)); // 异步加密

                // 输出文件路径
                string outputPath = $"{inputFilePath}.rsa.enc";

                // 保存加密后的数据
                await File.WriteAllBytesAsync(outputPath, encryptedData); // 异步保存加密数据

                return outputPath;
            }
        }

        // 修改后的ProcessFile方法调用异步加密
        private async Task ProcessFile(string filePath, string algorithm)
        {
            try
            {
                if (encryptionCategories["非对称加密"].Contains(algorithm))
                {
                    if (algorithm.Equals("RSA", StringComparison.OrdinalIgnoreCase))
                    {
                        // 使用异步RSA加密
                        string encryptedFilePath = await EncryptAsymmetricRSAAsync(filePath, certificate, algorithm);
                        this.Invoke((MethodInvoker)delegate
                        {
                            AppendLog($"[{Path.GetFileName(filePath)}] {algorithm}加密完成: {encryptedFilePath}");
                        });
                    }
                }
                else if (encryptionCategories["对称加密"].Contains(algorithm))
                {
                    string encryptedFilePath = EncryptFile(filePath, algorithm);
                    this.Invoke((MethodInvoker)delegate
                    {
                        AppendLog($"[{Path.GetFileName(filePath)}] {algorithm}加密完成: {encryptedFilePath}");
                    });
                }
                else if (encryptionCategories["哈希算法"].Contains(algorithm))
                {
                    string hash = ComputeHash(filePath, algorithm);
                    this.Invoke((MethodInvoker)delegate
                    {
                        AppendLog($"{algorithm}哈希值: {hash}");
                    });
                }
            }
            catch (Exception ex)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    AppendLog($"[{Path.GetFileName(filePath)}] {algorithm}处理失败: {ex.Message}");
                });
            }
        }


        // 解密文件（RSA）
        public string DecryptFile(string inputFilePath, X509Certificate2 certificate)
        {
            // 读取加密后的文件内容
            byte[] encryptedData = File.ReadAllBytes(inputFilePath);

            // 使用私钥解密数据
            using (RSA rsa = certificate.PrivateKey as RSA)
            {
                if (rsa == null)
                    throw new ArgumentException("证书不包含有效的 RSA 私钥");

                byte[] decryptedData = rsa.Decrypt(encryptedData, RSAEncryptionPadding.Pkcs1);

                // 输出文件路径
                string outputPath = $"{inputFilePath}.enc";

                // 保存解密后的数据
                File.WriteAllBytes(outputPath, decryptedData);

                return outputPath;
            }
        }

    }
}
