# 先放成果

直接下载：[加密记事本.exe](./加密记事本.exe)

# 界面设计

1. **控件布局**：
   - 添加一个 `TextBox`（命名为 `txtContent`），用于输入和显示文本。Anchor设置为四个方向，随窗口拉伸。
   - 添加一个 `TextBox`（命名为 `txtPassword`），用于输入密码。Anchor设置为上左右三个方向，随窗口拉伸。
   - 添加六个 `Button`：
     - 一个用于新建（命名为`btnNew`，文本为“新建”）。
     - 一个用于加密保存（命名为 `btnEncryptSave`，文本为“保存(Ctrl+S)”）。
     - 一个用于解密读取（命名为 `btnDecryptRead`，文本为“打开(拖拽)”）。
     - 一个用于加密整个文件夹的txt文件（命名为 `btnEncryptFolder`，文本为“加密文件夹”）。
     - 一个用于解密整个文件夹的txt文件（命名为 `btnDecryptFolder`，文本为“解密文件夹”）。
     - 一个用于临时查看密码的按钮（命名为 `btnEye`，文本为“👁”）。Anchor设置为上右两个方向。
   - 添加一个 `Label`，用于提示密码输入（文本为“密码”）。
2. **事件绑定**：
   - 双击 `btnEncryptSave` 按钮，生成 `btnEncryptSave_Click` 事件处理程序。
   - 双击 `btnDecryptRead` 按钮，生成 `btnDecryptRead_Click` 事件处理程序。
   - 双击 `btnNew` 按钮，生成 `btnNew_Click` 事件处理程序。
   - 双击 `btnEncryptFolder` 按钮，生成 `btnEncryptFolder_Click` 事件处理程序。
   - 双击 `btnDecryptFolder` 按钮，生成 `btnDecryptFolder_Click` 事件处理程序。
   - 选中 `btnEye` 按钮，查看属性面板的【闪电】图标，双击里面的MouseDown和MouseUp，会自动生成 `btnEye_MouseDown` 和 `btnEye_MouseUp` 事件处理程序。

# 主代码

```csharp
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace EncryptedNotepad
{
    public partial class MainForm : Form
    {
        private string currentFilePath = null; // 当前打开的文件路径
        private string password = null; // 当前密码
        private string defaultTitleName;
        const string PassPortFileName = "key.cfg";

        public MainForm()
        {
            InitializeComponent();
            LoadPassword(); // 启动时加载密码
            defaultTitleName = this.Text;

            // 启用拖拽功能
            this.AllowDrop = true;

            // 绑定拖拽事件
            this.DragEnter += MainForm_DragEnter;
            this.DragDrop += MainForm_DragDrop;
        }

        // 拖拽进入窗体时触发
        private void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            // 检查拖拽的文件是否是文本文件
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (Path.GetExtension(files[0]).ToLower() == ".txt")
                {
                    e.Effect = DragDropEffects.Copy; // 允许拖拽
                }
            }
        }

        // 拖拽释放时触发
        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            // 获取拖拽的文件路径
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            string filePath = files[0];

            // 调用你的读取文件逻辑
            password = txtPassword.Text; // 更新密码

            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("请先输入密码", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            currentFilePath = filePath;
            DoRead();
        }

        private void LoadPassword()
        {
            if (File.Exists(PassPortFileName))
            {
                try
                {
                    // 从文件读取加密的密码
                    byte[] encryptedPassword = File.ReadAllBytes(PassPortFileName);

                    // 使用固定密钥解密密码
                    password = DecryptStringFromBytes(encryptedPassword, "fixed_key_for_password_storage");
                    txtPassword.Text = password; // 显示密码
                }
                catch
                {
                    MessageBox.Show($"未找到密钥文件{PassPortFileName}。请输入新的密码。", "注意", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void SavePassword()
        {
            try
            {
                // 使用固定密钥加密密码
                byte[] encryptedPassword = EncryptStringToBytes(password, "fixed_key_for_password_storage");

                // 保存到文件
                File.WriteAllBytes(PassPortFileName, encryptedPassword);
            }
            catch
            {
                MessageBox.Show($"保存密钥文件{PassPortFileName}失败。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void EncryptFolder(string folderPath, string password)
        {
            // 遍历文件夹及其子文件夹中的所有 .txt 文件
            foreach (string filePath in Directory.GetFiles(folderPath, "*.txt", SearchOption.AllDirectories))
            {
                try
                {
                    var encode = DetectFileEncoding(filePath);
                    // 读取文件内容
                    string plainText = File.ReadAllText(filePath, encode);

                    // 加密文本
                    string encryptedText = EncryptText(plainText, password);

                    // 保存加密后的内容
                    File.WriteAllText(filePath, encryptedText);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"加密失败: {filePath}\nError: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void DecryptFolder(string folderPath, string password)
        {
            // 遍历文件夹及其子文件夹中的所有 .txt 文件
            foreach (string filePath in Directory.GetFiles(folderPath, "*.txt", SearchOption.AllDirectories))
            {
                try
                {
                    var encode = DetectFileEncoding(filePath);
                    // 读取文件内容
                    string plainText = File.ReadAllText(filePath, encode);

                    // 加密文本
                    string encryptedText = DecryptText(plainText, password);

                    // 保存加密后的内容
                    File.WriteAllText(filePath, encryptedText);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"解密失败: {filePath}\nError: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        //执行文件读取
        private void DoRead()
        {
            var encode = DetectFileEncoding(currentFilePath);
            // 从文件读取加密数据
            string encryptedText = File.ReadAllText(currentFilePath, encode);

            // 解密数据
            try
            {
                string decryptedText = DecryptText(encryptedText, password);
                txtContent.Text = decryptedText;
                //MessageBox.Show("解密成功.", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 更新窗口标题
                this.Text = $"{defaultTitleName} - {Path.GetFileName(currentFilePath)}";

                // 保存密码
                SavePassword();
            }
            catch (CryptographicException)
            {
                MessageBox.Show("无效的密码", "读取文件失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DoSave()
        {
            string text = txtContent.Text;
            password = txtPassword.Text; // 更新密码

            if (string.IsNullOrEmpty(text))
            {
                MessageBox.Show("请输入内容再保存", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("请输入密码", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 如果没有打开文件，提示用户选择保存路径
            if (string.IsNullOrEmpty(currentFilePath))
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                saveFileDialog.DefaultExt = "txt";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    currentFilePath = saveFileDialog.FileName;
                }
                else
                {
                    return; // 用户取消保存
                }
            }

            // 加密文本
            string encryptedText = EncryptText(text, password);

            // 保存到文件
            File.WriteAllText(currentFilePath, encryptedText);
            MessageBox.Show($"文件已加密保存到 {currentFilePath}", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // 更新窗口标题
            this.Text = $"{defaultTitleName} - {Path.GetFileName(currentFilePath)}";

            // 保存密码
            SavePassword();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // 检测是否按下了 Ctrl + S
            if (keyData == (Keys.Control | Keys.S))
            {
                // 调用保存按钮的逻辑
                DoSave();
                return true; // 表示已处理该快捷键
            }

            // 其他按键交给基类处理
            return base.ProcessCmdKey(ref msg, keyData);
        }

        #region 按钮点击
        private void btnEncryptSave_Click(object sender, EventArgs e)
        {
            DoSave();
        }

        private void btnDecryptRead_Click(object sender, EventArgs e)
        {
            password = txtPassword.Text; // 更新密码

            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("请先输入密码", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 打开文件选择对话框
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            openFileDialog.DefaultExt = "txt";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                currentFilePath = openFileDialog.FileName;
                DoRead();
            }
        }

        private void btnEncryptFolder_Click(object sender, EventArgs e)
        {
            password = txtPassword.Text; // 更新密码

            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("请先输入密码", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 打开文件夹选择对话框
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();
            if (folderDialog.ShowDialog() == DialogResult.OK)
            {
                string folderPath = folderDialog.SelectedPath;

                // 加密文件夹中的所有 .txt 文件
                EncryptFolder(folderPath, password);
                MessageBox.Show($"所有来自 {folderPath} 的txt文件都完成了加密.", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnDecryptFolder_Click(object sender, EventArgs e)
        {
            password = txtPassword.Text; // 更新密码

            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("请先输入密码", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 打开文件夹选择对话框
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();
            if (folderDialog.ShowDialog() == DialogResult.OK)
            {
                string folderPath = folderDialog.SelectedPath;

                // 加密文件夹中的所有 .txt 文件
                DecryptFolder(folderPath, password);
                MessageBox.Show($"所有来自 {folderPath} 的txt文件都完成了解密.", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            // 清空文本框内容
            txtContent.Text = string.Empty;

            // 重置窗口标题
            this.Text = defaultTitleName;

            // 弹出文件保存对话框
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            saveFileDialog.DefaultExt = "txt";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                currentFilePath = saveFileDialog.FileName;
                this.Text = $"{defaultTitleName} - {Path.GetFileName(currentFilePath)}";
            }
        }
        private void btnEye_MouseDown(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = false;//显示明文
        }
        private void btnEye_MouseUp(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = true;//恢复为密码模式
        }
        #endregion

        #region 静态工具函数
        static string EncryptText(string plainText, string password)
        {
            // 按行拆分文本
            string[] lines = plainText.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

            // 对每一行单独加密
            StringBuilder encryptedText = new StringBuilder();
            for (int i = 0; i < lines.Length; i++)
            {
                byte[] encryptedData = EncryptStringToBytes(lines[i], password);
                string encryptedLine = Convert.ToBase64String(encryptedData); // 使用 Base64 编码
                encryptedText.Append(encryptedLine);

                // 如果不是最后一行，则添加换行符
                if (i < lines.Length - 1)
                {
                    encryptedText.Append(Environment.NewLine);
                }
            }

            return encryptedText.ToString();
        }

        static string DecryptText(string encryptedText, string password)
        {
            // 按行拆分加密文本
            string[] lines = encryptedText.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

            // 对每一行单独解密
            StringBuilder decryptedText = new StringBuilder();
            for (int i = 0; i < lines.Length; i++)
            {
                if (string.IsNullOrEmpty(lines[i])) continue;

                byte[] encryptedData = Convert.FromBase64String(lines[i]); // 使用 Base64 解码
                string decryptedLine = DecryptStringFromBytes(encryptedData, password);
                decryptedText.Append(decryptedLine);

                // 如果不是最后一行，则添加换行符
                if (i < lines.Length - 1)
                {
                    decryptedText.Append(Environment.NewLine);
                }
            }

            return decryptedText.ToString();
        }

        static byte[] EncryptStringToBytes(string plainText, string password)
        {
            using (Aes aes = Aes.Create())
            {
                // 使用密码生成密钥和IV
                byte[] key = GenerateKey(password, aes.KeySize / 8);
                byte[] iv = GenerateIV(password, aes.BlockSize / 8);

                aes.Key = key;
                aes.IV = iv;

                // 创建加密器
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                // 加密数据
                using (var ms = new MemoryStream())
                using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                    cs.Write(plainBytes, 0, plainBytes.Length);
                    cs.FlushFinalBlock();
                    return ms.ToArray();
                }
            }
        }

        static string DecryptStringFromBytes(byte[] cipherText, string password)
        {
            using (Aes aes = Aes.Create())
            {
                // 使用密码生成密钥和IV
                byte[] key = GenerateKey(password, aes.KeySize / 8);
                byte[] iv = GenerateIV(password, aes.BlockSize / 8);

                aes.Key = key;
                aes.IV = iv;

                // 创建解密器
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                // 解密数据
                using (var ms = new MemoryStream(cipherText))
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (var sr = new StreamReader(cs))
                {
                    return sr.ReadToEnd();
                }
            }
        }

        static byte[] GenerateKey(string password, int keySize)
        {
            // 使用 SHA256 哈希算法从密码生成密钥
            using (var sha256 = SHA256.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] hash = sha256.ComputeHash(passwordBytes);
                byte[] key = new byte[keySize];
                Array.Copy(hash, key, keySize);
                return key;
            }
        }

        static byte[] GenerateIV(string password, int ivSize)
        {
            // 使用 MD5 哈希算法从密码生成 IV
            using (var md5 = MD5.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] hash = md5.ComputeHash(passwordBytes);
                byte[] iv = new byte[ivSize];
                Array.Copy(hash, iv, ivSize);
                return iv;
            }
        }

        public static Encoding DetectFileEncoding(string filePath)
        {
            // 检测 BOM
            Encoding bomEncoding = DetectBomEncoding(filePath);
            if (bomEncoding != null)
            {
                return bomEncoding;
            }

            // 检测无 BOM 的 UTF-8
            if (IsValidUtf8(filePath))
            {
                return Encoding.UTF8;
            }

            // 检测其他编码
            var encodingDetectors = new Dictionary<string, Func<string, bool>>
            {
                { "GBK", file => IsValidEncoding(file, Encoding.GetEncoding("GBK")) },
                { "GB2312", file => IsValidEncoding(file, Encoding.GetEncoding("GB2312")) },
                { "Big5", file => IsValidEncoding(file, Encoding.GetEncoding("Big5")) }
            };

            foreach (var detector in encodingDetectors)
            {
                if (detector.Value(filePath))
                {
                    return Encoding.GetEncoding(detector.Key);
                }
            }

            // 如果以上方法无法确定编码，则使用 StreamReader 的默认检测
            using (var reader = new StreamReader(filePath, Encoding.Default, true))
            {
                reader.Peek(); // 读取文件头以检测编码
                return reader.CurrentEncoding;
            }
        }

        // 检测 BOM
        private static Encoding DetectBomEncoding(string filePath)
        {
            byte[] buffer = new byte[4];
            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                int bytesRead = fileStream.Read(buffer, 0, buffer.Length);
                if (bytesRead < 2)
                {
                    return null; // 文件太小，无法检测 BOM
                }
            }

            if (buffer[0] == 0xEF && buffer[1] == 0xBB && buffer[2] == 0xBF)
            {
                return Encoding.UTF8; // UTF-8 with BOM
            }
            else if (buffer[0] == 0xFF && buffer[1] == 0xFE)
            {
                return Encoding.Unicode; // UTF-16 LE
            }
            else if (buffer[0] == 0xFE && buffer[1] == 0xFF)
            {
                return Encoding.BigEndianUnicode; // UTF-16 BE
            }
            else if (buffer[0] == 0 && buffer[1] == 0 && buffer[2] == 0xFE && buffer[3] == 0xFF)
            {
                return Encoding.UTF32; // UTF-32
            }

            return null; // 无 BOM
        }

        // 检测文件是否是无 BOM 的 UTF-8
        private static bool IsValidUtf8(string filePath)
        {
            try
            {
                byte[] fileBytes = File.ReadAllBytes(filePath);
                int i = 0;
                while (i < fileBytes.Length)
                {
                    if (fileBytes[i] <= 0x7F) // 单字节字符 (0x00-0x7F)
                    {
                        i++;
                    }
                    else if (fileBytes[i] >= 0xC2 && fileBytes[i] <= 0xDF) // 双字节字符
                    {
                        if (i + 1 >= fileBytes.Length || fileBytes[i + 1] < 0x80 || fileBytes[i + 1] > 0xBF)
                        {
                            return false;
                        }
                        i += 2;
                    }
                    else if (fileBytes[i] >= 0xE0 && fileBytes[i] <= 0xEF) // 三字节字符
                    {
                        if (i + 2 >= fileBytes.Length || fileBytes[i + 1] < 0x80 || fileBytes[i + 1] > 0xBF ||
                            fileBytes[i + 2] < 0x80 || fileBytes[i + 2] > 0xBF)
                        {
                            return false;
                        }
                        i += 3;
                    }
                    else if (fileBytes[i] >= 0xF0 && fileBytes[i] <= 0xF4) // 四字节字符
                    {
                        if (i + 3 >= fileBytes.Length || fileBytes[i + 1] < 0x80 || fileBytes[i + 1] > 0xBF ||
                            fileBytes[i + 2] < 0x80 || fileBytes[i + 2] > 0xBF ||
                            fileBytes[i + 3] < 0x80 || fileBytes[i + 3] > 0xBF)
                        {
                            return false;
                        }
                        i += 4;
                    }
                    else
                    {
                        return false;
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        // 检测文件是否可以用指定编码解码
        private static bool IsValidEncoding(string filePath, Encoding encoding)
        {
            try
            {
                byte[] fileBytes = File.ReadAllBytes(filePath);
                string content = encoding.GetString(fileBytes);

                foreach (char c in content)
                {
                    if (!IsValidCharForEncoding(c, encoding))
                    {
                        return false;
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        // 检查字符是否在目标编码的有效范围内
        private static bool IsValidCharForEncoding(char c, Encoding encoding)
        {
            if (encoding == Encoding.GetEncoding("GBK"))
            {
                return (c >= 0x4E00 && c <= 0x9FFF) || (c >= 0x3400 && c <= 0x4DBF) || (c >= 0x20000 && c <= 0x2A6DF);
            }
            else if (encoding == Encoding.GetEncoding("GB2312"))
            {
                return (c >= 0xB0A1 && c <= 0xF7FE);
            }
            else if (encoding == Encoding.GetEncoding("Big5"))
            {
                return (c >= 0xA140 && c <= 0xF9FE);
            }

            return true; // 其他编码默认返回 true
        }

        #endregion
    }
}
```

