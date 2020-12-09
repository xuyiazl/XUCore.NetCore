using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace XUCore.Develops
{
    public interface IConsoleProgressBarSettings
    {
        /// <summary>
        /// 偏移量
        /// </summary>
        int Left { get; set; }
        /// <summary>
        /// 进度条长度
        /// </summary>
        int Width { get; set; }
        /// <summary>
        /// 进度条背景色
        /// </summary>
        ConsoleColor BackgroundColor { get; set; }
        /// <summary>
        /// 进度条前景色
        /// </summary>
        ConsoleColor Color { get; set; }
        /// <summary>
        /// 进度条消息字体颜色
        /// </summary>
        ConsoleColor MessageColor { get; set; }
        /// <summary>
        /// 进度条百分比字体色
        /// </summary>
        ConsoleColor PercentColor { get; set; }
        /// <summary>
        /// 进度条标题字体颜色
        /// </summary>
        ConsoleColor TitleColor { get; set; }
    }

    public class ConsoleProgressBarSettings : IConsoleProgressBarSettings
    {
        /// <summary>
        /// 偏移量
        /// </summary>
        public int Left { get; set; }
        /// <summary>
        /// 进度条长度
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// 进度条背景色
        /// </summary>
        public ConsoleColor BackgroundColor { get; set; }
        /// <summary>
        /// 进度条前景色
        /// </summary>
        public ConsoleColor Color { get; set; }
        /// <summary>
        /// 进度条标题字体颜色
        /// </summary>
        public ConsoleColor TitleColor { get; set; }
        /// <summary>
        /// 进度条消息字体颜色
        /// </summary>
        public ConsoleColor MessageColor { get; set; }
        /// <summary>
        /// 进度条百分比字体色
        /// </summary>
        public ConsoleColor PercentColor { get; set; }
    }

    public class ConsoleProgressBarWin2003Settings : ConsoleProgressBarSettings, IConsoleProgressBarSettings
    {
        public ConsoleProgressBarWin2003Settings()
            : base()
        {
            Left = 10;
            Width = 50;
            BackgroundColor = ConsoleColor.DarkBlue;
            Color = ConsoleColor.Blue;
            TitleColor = ConsoleColor.Gray;
            MessageColor = ConsoleColor.Gray;
            PercentColor = ConsoleColor.Gray;
        }

    }

    public class ConsoleProgressBarWinXPSettings : ConsoleProgressBarSettings, IConsoleProgressBarSettings
    {
        public ConsoleProgressBarWinXPSettings()
            : base()
        {
            Left = 10;
            Width = 50;
            BackgroundColor = ConsoleColor.Gray;
            Color = ConsoleColor.Blue;
            TitleColor = ConsoleColor.Gray;
            MessageColor = ConsoleColor.Gray;
            PercentColor = ConsoleColor.Gray;
        }
    }

    public class ConsoleProgressBarWin8Settings : ConsoleProgressBarSettings, IConsoleProgressBarSettings
    {
        public ConsoleProgressBarWin8Settings()
            : base()
        {
            Left = 10;
            Width = 50;
            BackgroundColor = ConsoleColor.DarkCyan;
            Color = ConsoleColor.Yellow;
            TitleColor = ConsoleColor.Gray;
            MessageColor = ConsoleColor.Gray;
            PercentColor = ConsoleColor.Green;
        }
    }

    public static class ConsoleProgressBarTheme
    {
        public static IConsoleProgressBarSettings Win2003
        {
            get
            {
                return new ConsoleProgressBarWin2003Settings();
            }
        }
        public static IConsoleProgressBarSettings WinXP
        {
            get
            {
                return new ConsoleProgressBarWinXPSettings();
            }
        }
        public static IConsoleProgressBarSettings Win8
        {
            get
            {
                return new ConsoleProgressBarWin8Settings();
            }
        }
    }

    /// <summary>
    /// Console进度条工具类
    /// 每个进度条占用4行的间距
    /// </summary>
    public class ConsoleProgressBar
    {
        IConsoleProgressBarSettings barSettings;
        ConsoleColor colorBack = Console.BackgroundColor;
        ConsoleColor colorFore = Console.ForegroundColor;
        int barCoordY = 0;

        public ConsoleProgressBar(string title, IConsoleProgressBarSettings settings = null)
        {
            ConsoleProgressBarWinApi.BindHandle();
            var coord = ConsoleProgressBarWinApi.GetCursorPos();
            Bind(title, coord.Y, settings);
        }

        public ConsoleProgressBar(string title, int barCoordY, IConsoleProgressBarSettings settings = null)
        {
            Bind(title, barCoordY, settings);
        }

        public void Bind(string title, int barCoordY, IConsoleProgressBarSettings settings)
        {
            this.barCoordY = barCoordY;
            if (settings == null)
                barSettings = new ConsoleProgressBarWin2003Settings();
            else
                barSettings = settings;
            if (this.barCoordY >= 290)
            {
                Console.Clear();
                this.barCoordY = 0;
            }
            Console.ForegroundColor = colorFore;
            Console.WriteLine("----------------------------------------------------------------------");
            Console.WriteLine();
            //获取字符长度
            int len = GetStringLength(title);
            //设置标题的相对居中位置
            Console.SetCursorPosition(barSettings.Left + (barSettings.Width / 2 - len), this.barCoordY + 1);
            Console.Write(title);

            //写入进度条背景
            Console.BackgroundColor = barSettings.BackgroundColor;
            Console.SetCursorPosition(barSettings.Left, this.barCoordY + 3);
            Createblank(barSettings.Width);

            Console.WriteLine();
            Console.BackgroundColor = colorBack;
        }
        /// <summary>
        /// 更新进度条
        /// </summary>
        /// <param name="current">当前进度</param>
        /// <param name="total">总进度</param>
        /// <param name="message">说明文字</param>
        public void Update(int current, int total, string message)
        {
            //计算百分比
            int i = (int)Math.Ceiling(current / (double)total * 100);

            Console.BackgroundColor = barSettings.Color;
            Console.SetCursorPosition(barSettings.Left, this.barCoordY + 3);
            //写入进度条
            //当前百分比*进度条总长度=要输出的进度最小单位数量
            int count = (int)Math.Ceiling((double)i / 100 * barSettings.Width);
            Createblank(count);

            //写入未完成的进度条 主要用于进度回溯
            Console.BackgroundColor = barSettings.BackgroundColor;
            Console.SetCursorPosition(barSettings.Left + count, this.barCoordY + 3);

            Createblank(barSettings.Width - count);

            //设置和写入百分比
            Console.BackgroundColor = colorBack;
            Console.ForegroundColor = barSettings.PercentColor;
            Console.SetCursorPosition(barSettings.Left + barSettings.Width, this.barCoordY + 3);
            Console.Write(" {0}% ", i);

            Clear(this.barCoordY + 4);

            int len = GetStringLength(message);
            Console.SetCursorPosition(barSettings.Left + (barSettings.Width / 2 - len), this.barCoordY + 5);
            Console.Write(message);

            Console.ForegroundColor = colorFore;
            Console.SetCursorPosition(0, this.barCoordY + 6);
            Console.WriteLine("----------------------------------------------------------------------");

            //进度完成另起新行作为输出
            if (i >= 100 || i <= 0)
            {
                Console.WriteLine();
            }
        }
        /// <summary>
        /// 清空数据
        /// </summary>
        /// <param name="y"></param>
        private void Clear(int y)
        {
            Console.BackgroundColor = colorBack;
            Console.SetCursorPosition(0, y);
            Createblank(Console.WindowWidth);
        }
        /// <summary>
        /// 创建颜色区域
        /// </summary>
        /// <param name="num"></param>
        private void Createblank(int num)
        {
            StringBuilder blank = new StringBuilder();
            for (int i = 0; ++i <= num;) blank.Append(" ");
            Console.Write(blank);
        }
        /// <summary>
        /// 获取字符长度
        /// </summary>
        /// <remarks>中文和全角占长度1，英文和半角字符2个字母占长度1</remarks>
        /// <param name="message"></param>
        /// <returns></returns>
        private int GetStringLength(string message)
        {
            int len = Encoding.ASCII.GetBytes(message).Count(b => b == 63);
            return (message.Length - len) / 2 + len;
        }
    }

    public static class ConsoleProgressBarWinApi
    {

        #region [ window api ]

        private const int STD_OUTPUT_HANDLE = -11;
        private static int mHConsoleHandle;

        [StructLayout(LayoutKind.Sequential)]
        public struct COORD
        {
            public short X;
            public short Y;
            public COORD(short x, short y)
            {
                X = x;
                Y = y;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        struct SMALL_RECT
        {
            public short Left;
            public short Top;
            public short Right;
            public short Bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct CONSOLE_SCREEN_BUFFER_INFO
        {
            public COORD dwSize;
            public COORD dwCursorPosition;
            public int wAttributes;
            public SMALL_RECT srWindow;
            public COORD dwMaximumWindowSize;
        }

        [DllImport("kernel32.dll", EntryPoint = "GetStdHandle", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern int GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll", EntryPoint = "GetConsoleScreenBufferInfo", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern int GetConsoleScreenBufferInfo(int hConsoleOutput, out CONSOLE_SCREEN_BUFFER_INFO lpConsoleScreenBufferInfo);

        [DllImport("kernel32.dll", EntryPoint = "SetConsoleCursorPosition", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern int SetConsoleCursorPosition(int hConsoleOutput, COORD dwCursorPosition);

        public static void SetCursorPos(short x, short y)
        {
            SetConsoleCursorPosition(mHConsoleHandle, new COORD(x, y));
        }

        public static COORD GetCursorPos()
        {
            CONSOLE_SCREEN_BUFFER_INFO res;
            GetConsoleScreenBufferInfo(mHConsoleHandle, out res);
            return res.dwCursorPosition;
        }

        #endregion

        public static void BindHandle()
        {
            //获取当前窗体句柄
            mHConsoleHandle = GetStdHandle(STD_OUTPUT_HANDLE);
        }
    }
}
