using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 教务管理系统
{
    public partial class fileED : Form
    {
        private fileED_logform logForm;
        Dictionary<string, List<string>> encryptionCategories = new Dictionary<string, List<string>>();
        // 生成自签名证书
        X509Certificate2 certificate = GenerateSelfSignedCertificate("CN=MyTestCertificate", 2048, "password123");
        /*        AsymmetricCipherKeyPair keyPair = null;
                ElGamalPublicKeyParameters publicKey = null;
                ElGamalPrivateKeyParameters privateKey = null;*/
        // 在fileED类中添加成员变量
        private int totalOperations = 0;
        private int completedOperations = 0;
        private CancellationTokenSource _cancellationTokenSource;
        private List<string> _tempFiles = new List<string>();
        private object _fileLock = new object();

        private void InitializeLogForm()
        {
            // 确保在UI线程初始化
            if (groupBox4.InvokeRequired)
            {
                groupBox4.Invoke(new Action(InitializeLogForm));
                return;
            }

            logForm = new fileED_logform
            {
                TopLevel = false,  // 关键设置：使窗体可以作为控件
                FormBorderStyle = FormBorderStyle.None,
                Dock = DockStyle.Fill,
                Visible = true,
                Font = new Font("微软雅黑", 10, FontStyle.Bold)
            };

            groupBox4.Controls.Clear();
            groupBox4.Controls.Add(logForm);

            // 确保窗体句柄已创建
            if (!logForm.IsHandleCreated)
            {
                var handle = logForm.Handle; // 强制创建句柄
            }
        }

        public fileED()
        {
            InitializeComponent();

            // 延迟初始化日志窗体
            this.Load += (s, e) =>
            {
                InitializeLogForm();
                logForm.AppendLog("日志系统初始化完成"); // 测试日志
            };

            // 推荐的定义方式（修正算法名称）
            encryptionCategories.Add("哈希算法", new List<string> { "MD5", "SHA1", "SHA256", "SHA384", "SHA512" });
            encryptionCategories.Add("对称加密", new List<string> { "AES", "TripleDES", "DES", "RC2", "Rijndael" });
            encryptionCategories.Add("非对称加密", new List<string> { "RSA"/*, "ElGamal"*/ });

            // 将加密类别添加到 ComboBox1
            foreach (var category in encryptionCategories.Keys)
            {
                comboBox1.Items.Add(category);
            }

            comboBox1.SelectedIndex = 0; // 默认选择第一个类别

            // 初始化拖放事件
            listView1.DragDrop += listView1_DragDrop;
            listView1.DragEnter += listView1_DragEnter;

            listView1.ColumnWidthChanging += ListView1_ColumnWidthChanging;

            // 设置label1不拦截鼠标事件（关键！）
            label1.Enabled = false;      // 禁用控件交互
            label1.AllowDrop = false;    // 禁用拖放事件
            label1.BringToFront();      // 确保覆盖在ListView上方

            // 初始化遮罩状态
            UpdateMaskVisibility();

            button3.Enabled = false; // 初始状态下禁用终止按钮
        }

        private void ListView1_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            // 阻止列宽改变
            e.Cancel = true;
            // 保持列宽不变
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
                // 获取所有拖放路径
                string[] paths = (string[])e.Data.GetData(DataFormats.FileDrop);

                // 检查是否包含目录
                bool hasDirectory = paths.Any(p => Directory.Exists(p));

                // 只有全部是文件才允许拖放
                e.Effect = hasDirectory ? DragDropEffects.None : DragDropEffects.Copy;
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
                List<string> skippedDirs = new List<string>();

                // 获取当前所有已存在的文件路径（不区分大小写）
                HashSet<string> existingPaths = new HashSet<string>(
                    listView1.Items
                        .Cast<ListViewItem>()
                        .Select(item => item.SubItems[1].Text),
                    StringComparer.OrdinalIgnoreCase
                );

                foreach (string path in files)
                {
                    // 跳过目录
                    if (Directory.Exists(path))
                    {
                        skippedDirs.Add(path);
                        continue;
                    }

                    // 验证是否为有效文件
                    if (!File.Exists(path))
                    {
                        skippedFiles.Add(path);
                        continue;
                    }

                    // 检查重复项
                    if (existingPaths.Contains(path))
                    {
                        skippedFiles.Add(path);
                        continue;
                    }

                    // 获取文件名和图标
                    string fileName = System.IO.Path.GetFileName(path);
                    Icon fileIcon = Icon.ExtractAssociatedIcon(path);

                    // 添加图标到 ImageList
                    if (!imageList1.Images.ContainsKey(fileName))
                    {
                        imageList1.Images.Add(fileName, fileIcon);
                    }

                    // 创建 ListViewItem
                    ListViewItem item = new ListViewItem(fileName, fileName); // 使用文件名作为图标键
                    item.SubItems.Add(path); // 添加文件路径

                    // 添加到 ListView
                    listView1.Items.Add(item);
                }

                if (skippedFiles.Count > 0)
                {
                    MessageBox.Show($"已跳过 {skippedFiles.Count} 个重复文件:\n" + string.Join("\n", skippedFiles));

                }

                UpdateMaskVisibility(); // 关键更新
            }
        }

        private void 全选toolStripButton1_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.Items)
            {
                item.Checked = true; // 设置复选框为选中状态
            }
        }

        private void 取消全选toolStripButton1_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.Items)
            {
                item.Checked = false; // 设置复选框为未选中状态
            }

        }

        private void 删除选择项toolStripButton1_Click(object sender, EventArgs e)
        {
            // 检查是否有选中项
            if (listView1.CheckedItems.Count == 0)
            {
                MessageBox.Show("请先选择要删除的项", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 确认对话框
            var result = MessageBox.Show($"确定要删除选中的 {listView1.CheckedItems.Count} 项吗？",
                "确认删除",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result != DialogResult.Yes) return;

            // 使用反向遍历删除（避免索引错乱）
            try
            {
                List<ListViewItem> itemsToRemove = new List<ListViewItem>();
                foreach (ListViewItem item in listView1.CheckedItems)
                {
                    itemsToRemove.Add(item);
                    AppendLog($"已删除项：{item.Text} - {item.SubItems[1].Text}");
                }
                foreach (var item in itemsToRemove)
                {
                    listView1.Items.Remove(item);
                }

                // 更新界面
                UpdateMaskVisibility();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"删除过程中发生错误：{ex.Message}",
                    "错误",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 清空 CheckedListBox1
            checkedListBox1.Items.Clear();

            // 获取选中的加密类别
            string selectedCategory = comboBox1.SelectedItem.ToString();

            // 根据选中类别添加子项到 CheckedListBox1
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
                // 切换勾选状态
                checkedListBox1.SetItemChecked(index, !checkedListBox1.GetItemChecked(index));
            }
        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            // 如果是勾选操作
            if (e.NewValue == CheckState.Checked)
            {
                // 取消其他项的勾选
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
            SetOperationInProgress(true);
            try
            {
                // 初始化取消令牌
                _cancellationTokenSource = new CancellationTokenSource();
                var token = _cancellationTokenSource.Token;
                // 检查是否有选中的文件和算法
                var selectedFiles = listView1.CheckedItems.Cast<ListViewItem>().ToList();
                var selectedAlgorithms = checkedListBox1.CheckedItems.Cast<string>().ToList();

                if (!selectedFiles.Any())
                {
                    MessageBox.Show("请先选择要加密的文件");
                    return;
                }

                if (!selectedAlgorithms.Any())
                {
                    MessageBox.Show("请至少选择一个加密算法");
                    return;
                }

                // 计算总文件大小
                long totalSize = 0;
                foreach (ListViewItem fileItem in selectedFiles)
                {
                    string filePath = fileItem.SubItems[1].Text;
                    totalSize += new FileInfo(filePath).Length;
                }

                totalOperations = selectedFiles.Count;
                completedOperations = 0;

                progressBar1.Value = 0;
                progressBar1.Style = ProgressBarStyle.Continuous;

                // 启动异步加密任务
                await Task.Run(() =>
                {
                    ProcessFilesWithCancel(selectedFiles, selectedAlgorithms, true, token);
                }, token);
                logForm.AddManualSeparator(); // 添加手动分割线
            }
            catch (OperationCanceledException)
            {
                AppendLog("用户已取消加密操作");
                CleanupTempFiles();
            }
            finally
            {
                SetOperationInProgress(false);
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            SetOperationInProgress(true);
            try
            {
                // 初始化取消令牌
                _cancellationTokenSource = new CancellationTokenSource();
                var token = _cancellationTokenSource.Token;
                // 检查是否有选中的文件和算法
                var selectedFiles = listView1.CheckedItems.Cast<ListViewItem>().ToList();
                var selectedAlgorithms = checkedListBox1.CheckedItems.Cast<string>().ToList();

                if (!selectedFiles.Any())
                {
                    MessageBox.Show("请先选择要解密的文件");
                    return;
                }

                if (!selectedAlgorithms.Any())
                {
                    MessageBox.Show("请至少选择一个加密算法");
                    return;
                }

                // 计算总文件大小
                long totalSize = 0;
                foreach (ListViewItem fileItem in selectedFiles)
                {
                    string filePath = fileItem.SubItems[1].Text;
                    totalSize += new FileInfo(filePath).Length;
                }

                progressBar1.Value = 0;
                progressBar1.Style = ProgressBarStyle.Continuous;

                // 启动异步解密任务
                await Task.Run(() =>
                {
                    ProcessFilesWithCancel(selectedFiles, selectedAlgorithms, false, token);
                }, token);

                logForm.AddManualSeparator(); // 添加手动分割线
            }
            catch (OperationCanceledException)
            {
                AppendLog("用户已取消解密操作");
                CleanupTempFiles();
            }
            finally
            {
                SetOperationInProgress(false);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (_cancellationTokenSource != null && !_cancellationTokenSource.IsCancellationRequested)
                {
                    // 立即更新UI状态
                    button3.Enabled = false;
                    button3.Text = "正在停止...";

                    // 异步延迟确保UI更新
                    Task.Delay(200).ContinueWith(_ =>
                    {
                        _cancellationTokenSource.Cancel();
                    }, TaskScheduler.FromCurrentSynchronizationContext());
                }
            }
            catch (Exception ex)
            {
                AppendLog($"终止失败: {ex.Message}");
            }
        }

        private void ProcessFilesWithCancel(List<ListViewItem> files, List<string> algorithms, bool isEncrypt, CancellationToken token)
        {
            long totalSize = files.Sum(f => new FileInfo(f.SubItems[1].Text).Length);
            long processedSize = 0;

            foreach (var fileItem in files)
            {
                token.ThrowIfCancellationRequested();
                string filePath = fileItem.SubItems[1].Text;
                string fileName = Path.GetFileName(filePath);
                string outputPath = string.Empty;
                string finalPath = string.Empty;

                try
                {
                    foreach (var algorithm in algorithms)
                    {
                        token.ThrowIfCancellationRequested();

                        if (isEncrypt)
                        {
                            try
                            {
                                outputPath = GenerateTempFilePath(filePath);
                                finalPath = GetFinalEncryptedPath(filePath, algorithm); // 新增最终路径生成方法
                                AddTempFile(outputPath);

                                if (encryptionCategories["非对称加密"].Contains(algorithm))
                                {
                                    AppendLog(fileName + "正在加密...");
                                    EncryptAsymmetricRSAWithProgress(filePath, outputPath, certificate, ref processedSize, totalSize, token); AppendLog($"{fileName}已加密.");
                                    // 加密完成后重命名文件
                                    File.Move(outputPath, finalPath);
                                    RemoveTempFile(outputPath); // 从临时列表移除
                                    AppendLog($"{fileName}已加密为 {Path.GetFileName(finalPath)}");
                                }
                                else if (encryptionCategories["对称加密"].Contains(algorithm))
                                {
                                    AppendLog(fileName + "正在加密...");
                                    EncryptFileWithProgress(filePath, outputPath, algorithm, ref processedSize, totalSize, token);
                                    // 生成最终文件名并重命名
                                    finalPath = $"{filePath}.{algorithm.ToLower()}.enc";
                                    File.Move(outputPath, finalPath);
                                    RemoveTempFile(outputPath);
                                    AppendLog($"{fileName}已加密为 {Path.GetFileName(finalPath)}");
                                }
                                else if (encryptionCategories["哈希算法"].Contains(algorithm))
                                {
                                    AppendLog(fileName + "正在计算哈希值...");
                                    string hash = ComputeHashWithProgress(filePath, algorithm, ref processedSize, totalSize, token);
                                    AppendLog($"{fileName}的{algorithm}哈希值: {hash}");
                                }
                            }
                            catch (Exception ex)
                            {
                                AppendLog($"{algorithm}: {ex.Message}");
                                SafeDeleteFile(outputPath);
                            }
                        }
                        else
                        {
                            try
                            {
                                // 根据算法类型执行不同操作
                                if (encryptionCategories["哈希算法"].Contains(algorithm))
                                {
                                    AppendLog(fileName + "正在计算哈希值...");
                                    string hash = ComputeHashWithProgress(filePath, algorithm, ref processedSize, totalSize, token);
                                    AppendLog($"{fileName}的{algorithm}哈希值: {hash}");
                                }
                                else if (encryptionCategories["对称加密"].Contains(algorithm))
                                {
                                    AppendLog(fileName + "正在解密...");
                                    // 读取密钥和 IV
                                    (byte[] key, byte[] iv) = ReadKeyAndIV(filePath, algorithm);
                                    // 解密文件
                                    string decryptedFilePath = DecryptFileWithProgress(filePath, key, iv, algorithm, ref processedSize, totalSize);
                                    AppendLog($"{fileName}已解密为 {Path.GetFileName(decryptedFilePath)}");
                                }
                                else if (encryptionCategories["非对称加密"].Contains(algorithm))
                                {
                                    if (algorithm.Equals("RSA", StringComparison.OrdinalIgnoreCase))
                                    {
                                        AppendLog(fileName + "正在解密...");
                                        string decryptedFilePath = DecryptFileWithProgressRSA(filePath, certificate, ref processedSize, totalSize);
                                        AppendLog($"{fileName}已解密为 {Path.GetFileName(decryptedFilePath)}");
                                    }
                                    // 其他非对称加密算法的处理逻辑可以在这里补充
                                }
                            }
                            catch (Exception ex)
                            {
                                AppendLog($"操作失败 - {algorithm}: {ex.Message}");
                            }
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    AppendLog("操作已取消");
                    SafeDeleteFile(outputPath);
                    throw;
                }
                finally
                {
                    UpdateProgressSafe(0, 1);
                }
            }
        }

        // 生成最终加密文件名
        private string GetFinalEncryptedPath(string originalPath, string algorithm)
        {
            if (encryptionCategories["非对称加密"].Contains(algorithm))
            {
                return $"{originalPath}.rsa.enc";
            }
            return $"{originalPath}.{algorithm.ToLower()}.enc";
        }

        // 计算文件哈希值
        private string ComputeHashWithProgress(string filePath, string algorithm, ref long processedSize, long totalSize, CancellationToken token)
        {
            using (var hashAlgorithm = HashAlgorithm.Create(algorithm))
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                long fileTotalBytes = stream.Length;
                long fileProcessedBytes = 0;
                byte[] buffer = new byte[4096];
                int bytesRead;

                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    token.ThrowIfCancellationRequested();
                    hashAlgorithm.TransformBlock(buffer, 0, bytesRead, null, 0);
                    fileProcessedBytes += bytesRead;

                    // 更新文件处理进度
                    processedSize += bytesRead;
                    UpdateProgress(processedSize, totalSize);
                }

                hashAlgorithm.TransformFinalBlock(buffer, 0, 0);
                return BitConverter.ToString(hashAlgorithm.Hash).Replace("-", "").ToLowerInvariant();
            }
        }

        // 对称加密（使用随机密钥）
        private void EncryptFileWithProgress(string filePath, string outputPath, string algorithm, ref long processedSize, long totalSize, CancellationToken token)
        {
            using (SymmetricAlgorithm algo = GetAlgorithm(algorithm))
            {
                algo.GenerateKey();
                algo.GenerateIV();

                long fileTotalBytes = new FileInfo(filePath).Length;

                using (var input = new FileStream(filePath, FileMode.Open))
                using (var output = new FileStream(outputPath, FileMode.Create))
                using (var crypto = new CryptoStream(output, algo.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    byte[] buffer = new byte[4096];
                    int bytesRead;
                    long fileProcessedBytes = 0;

                    while ((bytesRead = input.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        token.ThrowIfCancellationRequested();
                        crypto.Write(buffer, 0, bytesRead);
                        fileProcessedBytes += bytesRead;

                        // 更新文件处理进度
                        processedSize += bytesRead;
                        UpdateProgress(processedSize, totalSize);
                    }
                }

                SaveKeyAndIV(algo.Key, algo.IV, filePath, algorithm);
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

        // 在Symmetric加密方法中修改密钥保存路径
        private void SaveKeyAndIV(byte[] key, byte[] iv, string filePath, string algorithm)
        {
            // 保存到最终文件路径的临时位置
            string keyPath = $"{filePath}.{algorithm.ToLower()}.enc.key";
            string ivPath = $"{filePath}.{algorithm.ToLower()}.enc.iv";

            using (FileStream keyFile = new FileStream(keyPath, FileMode.Create))
            {
                keyFile.Write(key, 0, key.Length);
            }

            using (FileStream ivFile = new FileStream(ivPath, FileMode.Create))
            {
                ivFile.Write(iv, 0, iv.Length);
            }
        }

        // 解密文件
        private string DecryptFileWithProgress(string encryptedFilePath, byte[] key, byte[] iv, string algorithm, ref long processedSize, long totalSize)
        {
            if (!File.Exists(encryptedFilePath))
                throw new FileNotFoundException("加密文件未找到", encryptedFilePath);

            if (key == null || iv == null)
                throw new ArgumentNullException("密钥或 IV 不能为空");

            using (SymmetricAlgorithm algo = GetAlgorithm(algorithm))
            {
                algo.Key = key;
                algo.IV = iv;

                string decryptedFilePath = $"{encryptedFilePath}.dec";

                long fileTotalBytes = new FileInfo(encryptedFilePath).Length;

                using (var input = new FileStream(encryptedFilePath, FileMode.Open))
                using (var output = new FileStream(decryptedFilePath, FileMode.Create))
                using (var crypto = new CryptoStream(output, algo.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    byte[] buffer = new byte[4096];
                    int bytesRead;
                    long fileProcessedBytes = 0;

                    while ((bytesRead = input.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        crypto.Write(buffer, 0, bytesRead);
                        fileProcessedBytes += bytesRead;

                        // 更新文件处理进度
                        processedSize += bytesRead;
                        UpdateProgress(processedSize, totalSize);
                    }
                }

                return decryptedFilePath;
            }
        }

        // 读取密钥和 IV
        public (byte[] key, byte[] iv) ReadKeyAndIV(string filePath, string algorithm)
        {
            string keyPath = $"{filePath}.key";
            string ivPath = $"{filePath}.iv";

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
        private void EncryptAsymmetricRSAWithProgress(string inputPath, string outputPath, X509Certificate2 cert,
                    ref long processedSize, long totalSize, CancellationToken token)
        {
            using (RSA rsa = cert.GetRSAPublicKey())
            using (var input = File.OpenRead(inputPath))
            using (var output = File.Create(outputPath))
            {
                int keySize = rsa.KeySize / 8;
                int maxBlockSize = keySize - 11;
                byte[] buffer = new byte[maxBlockSize];

                int bytesRead;
                while ((bytesRead = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    token.ThrowIfCancellationRequested();

                    byte[] encryptedBlock = rsa.Encrypt(buffer.AsSpan(0, bytesRead).ToArray(), RSAEncryptionPadding.Pkcs1);
                    output.Write(encryptedBlock, 0, encryptedBlock.Length);

                    processedSize += bytesRead;
                    UpdateProgressSafe(processedSize, totalSize);
                }
            }
        }

        // 解密文件（RSA）
        private string DecryptFileWithProgressRSA(string encryptedFilePath, X509Certificate2 certificate, ref long processedSize, long totalSize)
        {
            using (RSA rsa = certificate.GetRSAPrivateKey())
            {
                if (rsa == null) throw new ArgumentException("证书不包含有效的 RSA 私钥");

                int keySize = rsa.KeySize / 8; // 解密块大小 = 密钥长度
                string decryptedFilePath = $"{encryptedFilePath}.dec";

                using (var input = File.OpenRead(encryptedFilePath))
                using (var output = File.Create(decryptedFilePath))
                {
                    byte[] buffer = new byte[keySize];
                    int bytesRead;

                    while ((bytesRead = input.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        // 确保读取完整的加密块
                        if (bytesRead != keySize)
                        {
                            throw new CryptographicException($"无效的加密块大小，预期 {keySize} 字节，实际读取 {bytesRead} 字节");
                        }

                        // 解密数据块
                        byte[] decryptedBlock = rsa.Decrypt(buffer, RSAEncryptionPadding.Pkcs1);
                        output.Write(decryptedBlock, 0, decryptedBlock.Length);

                        // 更新处理进度
                        processedSize += bytesRead;
                        UpdateProgress(processedSize, totalSize);
                    }
                }
                return decryptedFilePath;
            }
        }

        // 日志记录方法
        // 日志追加方法
        public void AppendLog(string message)
        {
            try
            {
                if (logForm != null && !logForm.IsDisposed)
                {
                    if (logForm.InvokeRequired)
                    {
                        logForm.BeginInvoke(new Action<string>(AppendLog), message);
                    }
                    else
                    {
                        logForm.AppendLog($"[{DateTime.Now:HH:mm:ss}] {message}");
                    }
                }
            }
            catch (ObjectDisposedException)
            {
                // 忽略已释放的对象
            }
        }

        // 添加临时文件
        private void AddTempFile(string path)
        {
            lock (_fileLock)
            {
                _tempFiles.Add(path);
            }
        }

        // 清理临时文件
        private void CleanupTempFiles()
        {
            lock (_fileLock)
            {
                foreach (var file in _tempFiles.ToList())
                {
                    SafeDeleteFile(file);
                }
                _tempFiles.Clear();
            }
        }

        // 安全删除文件
        private void SafeDeleteFile(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                    AppendLog($"已清理临时文件: {Path.GetFileName(path)}");
                }
            }
            catch (Exception ex)
            {
                AppendLog($"清理文件失败 {Path.GetFileName(path)}: {ex.Message}");
            }
        }

        // 生成临时文件路径
        private string GenerateTempFilePath(string originalPath)
        {
            return Path.Combine(
                Path.GetDirectoryName(originalPath),
                $"{Path.GetFileNameWithoutExtension(originalPath)}_{Guid.NewGuid()}.tmp");
        }

        // 更新进度条（线程安全）
        private void UpdateProgressSafe(long processed, long total)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => UpdateProgress(processed, total)));
                return;
            }

            // 确保进度条可见性
            if (progressBar1.Style != ProgressBarStyle.Continuous)
            {
                progressBar1.Style = ProgressBarStyle.Continuous;
            }

            // 计算百分比并限制范围
            int percent = Math.Max(0, Math.Min(100, (int)((double)processed / total * 100)));
            progressBar1.Value = percent;

            // 更新按钮状态（确保操作中状态）
            if (!button3.Enabled)
            {
                button3.Enabled = true;
            }
        }

        // 设置操作进行中的状态
        private void SetOperationInProgress(bool inProgress)
        {
            // 允许UI线程调用
            if (InvokeRequired)
            {
                Invoke(new Action<bool>(SetOperationInProgress), inProgress);
                return;
            }

            button1.Enabled = !inProgress;
            button2.Enabled = !inProgress;
            button3.Enabled = inProgress;
            button3.Text = inProgress ? "终止操作" : "终止操作";

            // 进度条重置
            if (!inProgress)
            {
                progressBar1.Value = 0;
                progressBar1.Style = ProgressBarStyle.Continuous;
            }
        }

        // 更新进度条
        private void UpdateProgress(long processed, long total)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => UpdateProgress(processed, total)));
                return;
            }

            progressBar1.Style = ProgressBarStyle.Continuous;
            progressBar1.Maximum = 100;
            progressBar1.Value = (int)((double)processed / total * 100);
        }

        // 新增临时文件移除方法
        private void RemoveTempFile(string path)
        {
            lock (_fileLock)
            {
                if (_tempFiles.Contains(path))
                {
                    _tempFiles.Remove(path);
                }
            }
        }

        // 关闭窗体时清理资源
        private void fileED_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (logForm != null && !logForm.IsDisposed)
            {
                logForm.Close();
            }
        }

        // 处理键盘事件
        private void listView1_KeyDown(object sender, KeyEventArgs e)
        {
            // 检测是否按下 Del 键
            if (e.KeyCode == Keys.Delete)
            {
                DeleteSelectedItems();
                e.SuppressKeyPress = true; // 阻止系统发出按键声音
            }
        }

        // 删除选中的项
        private void DeleteSelectedItems()
        {
            // 检查 ListView1 中是否有选中的项
            if (listView1.SelectedItems.Count > 0)
            {
                // 删除选中的项
                foreach (ListViewItem selectedItem in listView1.SelectedItems)
                {
                    listView1.Items.Remove(selectedItem);
                }
            }
            else
            {
                MessageBox.Show("请先选择要删除的行！", "提示");
            }
        }

    }
}