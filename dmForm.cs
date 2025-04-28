using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

namespace FloatingNames
{
    public partial class dmForm : Form
    {
        private string[] names = { "张三", "李四", "王五", "诸葛亮", "张飞", "关羽", "李白", "张亮", "荆轲", "吕布" };
        private List<Label> charLabels = new List<Label>();
        private Random random = new Random();
        private const int ANIMATION_DURATION = 1000; // 动画持续时间(毫秒)
        private Button callButton;
        private System.Windows.Forms.Timer animationTimer;
        private Dictionary<Label, PointF> labelDirections = new Dictionary<Label, PointF>();
        private int moveStep = 2; // 每次移动的步长
        private bool isAnimating = false;
        private const float MIN_SPEED = 0.5f; // 最小速度

        public dmForm()
        {
            InitializeComponent();
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            // 设置窗体属性
            this.Text = "名字漂浮与组合";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.DoubleBuffered = true; // 减少闪烁
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
            this.BackColor = Color.FromArgb(40, 44, 52); // 深色背景

            // 创建点名按钮
            callButton = new Button
            {
                Text = "点名",
                Location = new Point(350, 500),
                Size = new Size(100, 30),
                Font = new Font("宋体", 12),
                BackColor = Color.FromArgb(78, 110, 170),
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 }
            };
            callButton.Click += CallButton_Click;
            this.Controls.Add(callButton);

            // 初始化并显示名字的单个字
            InitializeFloatingChars();

            // 创建动画计时器
            animationTimer = new System.Windows.Forms.Timer();
            animationTimer.Interval = 50; // 50毫秒更新一次
            animationTimer.Tick += AnimationTimer_Tick;
            animationTimer.Start();
        }

        private void InitializeFloatingChars()
        {
            // 清除之前的所有字标签
            foreach (var label in charLabels)
            {
                this.Controls.Remove(label);
            }
            charLabels.Clear();
            labelDirections.Clear();

            // 定义一些字体颜色
            Color[] fontColors = {
                Color.FromArgb(255, 69, 0),
                Color.FromArgb(255, 140, 0),
                Color.FromArgb(255, 215, 0),
                Color.FromArgb(32, 178, 170),
                Color.FromArgb(0, 128, 0),
                Color.FromArgb(75, 0, 130),
                Color.FromArgb(128, 0, 128),
                Color.FromArgb(255, 0, 255)
            };

            // 将每个名字分解为单个字，并创建标签
            foreach (string name in names)
            {
                foreach (char c in name)
                {
                    Label charLabel = new Label
                    {
                        Text = c.ToString(),
                        Font = new Font("微软雅黑", 20, FontStyle.Bold),
                        AutoSize = true,
                        TextAlign = ContentAlignment.MiddleCenter,
                        ForeColor = fontColors[random.Next(fontColors.Length)],
                        BackColor = Color.Transparent
                    };

                    // 设置随机位置且保证不重叠
                    Point position = GetNonOverlappingPosition(charLabel.Size);
                    charLabel.Location = position;

                    // 添加到标签方向字典中，用于动画
                    // 确保初始速度不为零
                    float xSpeed, ySpeed;
                    do
                    {
                        xSpeed = random.Next(-3, 4) * 0.5f;
                    } while (Math.Abs(xSpeed) < MIN_SPEED);

                    do
                    {
                        ySpeed = random.Next(-3, 4) * 0.5f;
                    } while (Math.Abs(ySpeed) < MIN_SPEED);

                    labelDirections[charLabel] = new PointF(xSpeed, ySpeed);

                    this.Controls.Add(charLabel);
                    charLabels.Add(charLabel);
                }
            }

            // 确保所有标签在初始化后不重叠
            EnsureNoOverlaps();
        }

        private Point GetNonOverlappingPosition(Size labelSize)
        {
            // 尝试多次找到一个不重叠的位置
            for (int i = 0; i < 100; i++)
            {
                int x = random.Next(50, this.Width - labelSize.Width - 50);
                int y = random.Next(50, this.Height - 150); // 留出按钮区域

                // 检查是否与现有标签重叠
                bool isOverlapping = false;
                foreach (var label in charLabels)
                {
                    if (label.Bounds.IntersectsWith(new Rectangle(x, y, labelSize.Width, labelSize.Height)))
                    {
                        isOverlapping = true;
                        break;
                    }
                }

                if (!isOverlapping)
                {
                    return new Point(x, y);
                }
            }

            // 如果尝试多次仍未找到非重叠位置，则返回一个随机位置
            return new Point(
                random.Next(50, this.Width - labelSize.Width - 50),
                random.Next(50, this.Height - 150)
            );
        }

        private void EnsureNoOverlaps()
        {
            // 额外检查并调整初始位置，确保没有重叠
            for (int i = 0; i < charLabels.Count; i++)
            {
                for (int j = i + 1; j < charLabels.Count; j++)
                {
                    Label label1 = charLabels[i];
                    Label label2 = charLabels[j];

                    // 检查是否重叠
                    if (label1.Bounds.IntersectsWith(label2.Bounds))
                    {
                        // 计算中心点
                        Point center1 = new Point(
                            label1.Location.X + label1.Width / 2,
                            label1.Location.Y + label1.Height / 2
                        );

                        Point center2 = new Point(
                            label2.Location.X + label2.Width / 2,
                            label2.Location.Y + label2.Height / 2
                        );

                        // 计算方向
                        int dx = center2.X - center1.X;
                        int dy = center2.Y - center1.Y;
                        double distance = Math.Sqrt(dx * dx + dy * dy);

                        // 如果距离太近，则分开
                        if (distance < label1.Width)
                        {
                            double angle = Math.Atan2(dy, dx);
                            int moveX = (int)(Math.Cos(angle) * label1.Width);
                            int moveY = (int)(Math.Sin(angle) * label1.Width);

                            // 调整位置
                            label1.Location = new Point(
                                label1.Location.X - moveX,
                                label1.Location.Y - moveY
                            );

                            label2.Location = new Point(
                                label2.Location.X + moveX,
                                label2.Location.Y + moveY
                            );
                        }
                    }
                }
            }
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            if (isAnimating) return;

            foreach (var kvp in labelDirections.ToList())
            {
                Label label = kvp.Key;
                PointF direction = kvp.Value;

                // 计算新位置
                int newX = label.Location.X + (int)(direction.X * moveStep);
                int newY = label.Location.Y + (int)(direction.Y * moveStep);

                // 检查边界碰撞
                if (newX < 0)
                {
                    direction = new PointF(-direction.X, direction.Y);
                    newX = 0;
                }
                else if (newX + label.Width > this.Width)
                {
                    direction = new PointF(-direction.X, direction.Y);
                    newX = this.Width - label.Width;
                }

                if (newY < 0)
                {
                    direction = new PointF(direction.X, -direction.Y);
                    newY = 0;
                }
                else if (newY + label.Height > this.Height - 150) // 留出按钮区域
                {
                    direction = new PointF(direction.X, -direction.Y);
                    newY = this.Height - 150 - label.Height;
                }

                // 检查与其他标签的碰撞
                bool collision = false;
                foreach (var otherLabel in charLabels)
                {
                    if (otherLabel == label) continue;

                    Rectangle newRect = new Rectangle(newX, newY, label.Width, label.Height);
                    if (newRect.IntersectsWith(otherLabel.Bounds))
                    {
                        collision = true;

                        // 计算碰撞方向并反转相应方向
                        float dx = newX + label.Width / 2 - (otherLabel.Location.X + otherLabel.Width / 2);
                        float dy = newY + label.Height / 2 - (otherLabel.Location.Y + otherLabel.Height / 2);

                        if (Math.Abs(dx) > Math.Abs(dy))
                        {
                            direction = new PointF(-direction.X, direction.Y);
                        }
                        else
                        {
                            direction = new PointF(direction.X, -direction.Y);
                        }

                        break;
                    }
                }

                if (!collision)
                {
                    label.Location = new Point(newX, newY);
                }

                // 更新方向
                labelDirections[label] = direction;
            }
        }

        private async void CallButton_Click(object sender, EventArgs e)
        {
            if (isAnimating) return;

            isAnimating = true;
            callButton.Enabled = false;

            // 随机选择一个名字
            string selectedName = names[random.Next(names.Length)];
            var selectedChars = selectedName.ToCharArray();

            // 找到对应的名字的字标签
            var selectedLabels = charLabels
                .Where(l => selectedName.Contains(l.Text))
                .ToList();

            // 创建一个字典来存储每个字的最终位置
            Dictionary<Label, PointF> targetPositions = new Dictionary<Label, PointF>();

            // 计算最终组合的位置（窗体中心附近）
            int centerX = this.Width / 2;
            int centerY = this.Height / 2;
            int spacing = 40; // 增大字之间的间距

            int startX = centerX - (selectedChars.Length - 1) * spacing / 2;
            int index = 0;

            foreach (var c in selectedChars)
            {
                // 找到对应字的标签
                var charLabel = selectedLabels.FirstOrDefault(l => l.Text == c.ToString());
                if (charLabel != null)
                {
                    // 设置最终位置
                    int targetX = startX + index * spacing;
                    int targetY = centerY;
                    targetPositions[charLabel] = new PointF(targetX, targetY);
                    index++;
                }
            }

            // 执行动画移动
            await MoveLabelsToTargetPositions(targetPositions);

            // 等待一段时间后重置
            Task.Delay(3000).Wait();
            InitializeFloatingChars(); // 重新随机位置

            // 恢复按钮状态
            callButton.Enabled = true;
            isAnimating = false;
        }

        private async Task MoveLabelsToTargetPositions(Dictionary<Label, PointF> targetPositions)
        {
            // 创建动画参数
            double duration = ANIMATION_DURATION / 1000.0;
            double step = 0.02; // 每帧的进度步长
            double totalSteps = 1.0 / step;

            for (double progress = 0; progress < 1; progress += step)
            {
                foreach (var kvp in targetPositions)
                {
                    Label label = kvp.Key;
                    PointF target = kvp.Value;
                    PointF start = new PointF(label.Location.X, label.Location.Y);

                    // 计算新的位置
                    float newX = (float)(start.X + (target.X - start.X) * progress);
                    float newY = (float)(start.Y + (target.Y - start.Y) * progress);

                    // 更新位置
                    label.Location = new Point((int)newX, (int)newY);
                }

                // 更新UI并等待一小段时间
                this.Invalidate();
                await Task.Delay((int)(duration * 1000 * step));
            }

            // 确保最终位置准确
            foreach (var kvp in targetPositions)
            {
                Label label = kvp.Key;
                PointF target = kvp.Value;
                label.Location = new Point((int)target.X, (int)target.Y);
            }

            // 调整最终位置以避免重叠
            AdjustFinalPositions(targetPositions.Keys.ToList());
        }

        private void AdjustFinalPositions(List<Label> labels)
        {
            // 增加额外的间距调整
            int extraSpacing = 10;

            for (int i = 0; i < labels.Count; i++)
            {
                for (int j = i + 1; j < labels.Count; j++)
                {
                    Label label1 = labels[i];
                    Label label2 = labels[j];

                    // 检查是否重叠
                    if (label1.Bounds.IntersectsWith(label2.Bounds))
                    {
                        // 计算中心点
                        Point center1 = new Point(
                            label1.Location.X + label1.Width / 2,
                            label1.Location.Y + label1.Height / 2
                        );

                        Point center2 = new Point(
                            label2.Location.X + label2.Width / 2,
                            label2.Location.Y + label2.Height / 2
                        );

                        // 计算方向
                        int dx = center2.X - center1.X;
                        int dy = center2.Y - center1.Y;
                        double distance = Math.Sqrt(dx * dx + dy * dy);

                        // 如果距离太近，则分开
                        if (distance < label1.Width)
                        {
                            double angle = Math.Atan2(dy, dx);
                            int moveX = (int)(Math.Cos(angle) * extraSpacing);
                            int moveY = (int)(Math.Sin(angle) * extraSpacing);

                            label1.Location = new Point(
                                label1.Location.X - moveX,
                                label1.Location.Y - moveY
                            );

                            label2.Location = new Point(
                                label2.Location.X + moveX,
                                label2.Location.Y + moveY
                            );
                        }
                    }
                }
            }
        }
    }
}