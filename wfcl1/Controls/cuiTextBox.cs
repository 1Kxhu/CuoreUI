using CuoreUI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using static CuoreUI.Drawing;

[Obsolete]
[ToolboxBitmap(typeof(TextBox))]
[DefaultEvent("ContentChanged")]
public class cuiTextBox : UserControl
{
    private Padding borderRadius = new Padding(6);
    public Padding BorderRadius
    {
        get
        {
            return borderRadius;
        }
        set
        {
            borderRadius = value;
            Invalidate();
        }
    }

    private string privateContent = "Text here!";

    public string Content
    {
        get
        {
            return privateContent;
        }
        set
        {
            privateContent = value;
            Invalidate();
        }
    }

    private bool privatePswdChar = false;
    public bool UsePasswordChar
    {
        get
        {
            return privatePswdChar;
        }
        set
        {
            privatePswdChar = value;
            Refresh();
        }
    }

    Timer caretBlinkTimer = new Timer();

    public cuiTextBox()
    {
        DoubleBuffered = true;
        Size = new Size(200, 34);
        Cursor = Cursors.IBeam;
        ForeColor = SystemColors.ButtonFace;
        SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        SetStyle(ControlStyles.UserPaint, true);
        BorderStyle = BorderStyle.None;
        SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        caretBlinkTimer.Interval = 1000;
        caretBlinkTimer.Tick += (object sender, EventArgs args) =>
        {
            if (Enabled && Focused)
            {
                showCaret = !showCaret;
            }
            else
            {
                showCaret = false;
            }
        };
        caretBlinkTimer.Start();

        FrameDrawn += (e, s) =>
        {
            if (!Focused)
            {
                showCaret = false;
            }
            Refresh();
        };
    }

    public event EventHandler ContentChanged;

    private Color privateBackground = Color.FromArgb(50, 34, 34, 34);
    [Description("Only used when DesignStyle is set to Full")]
    public Color Background
    {
        get
        {
            return privateBackground;
        }
        set
        {
            privateBackground = value;
            Invalidate();
        }
    }


    private Color privateBorder = Color.FromArgb(34, 34, 34);
    public Color Border
    {
        get
        {
            return privateBorder;
        }
        set
        {
            privateBorder = value;
            Invalidate();
        }
    }

    private float privateBorderSize = 1.6f;
    [Description("Only used when DesignStyle is set to Full")]
    public float BorderSize
    {
        get
        {
            return privateBorderSize;
        }
        set
        {
            privateBorderSize = value;
            Invalidate();
        }
    }

    private Color privateFocusedBorder = CuoreUI.Drawing.PrimaryColor;
    public Color FocusedBorder
    {
        get
        {
            return privateFocusedBorder;
        }
        set
        {
            privateFocusedBorder = value;
            Invalidate();
        }
    }

    private string privatePlaceholder = "I am a placeholder.";
    public string Placeholder
    {
        get
        {
            return privatePlaceholder;
        }
        set
        {
            privatePlaceholder = value;
            Invalidate();
        }
    }


    private Color privatePlaceholderColor = Color.FromArgb(100, 100, 100);
    public Color PlaceholderColor
    {
        get
        {
            return privatePlaceholderColor;
        }
        set
        {
            privatePlaceholderColor = value;
            Invalidate();
        }
    }

    private Color privateFocusedBackground = Color.FromArgb(100, 34, 34, 34);
    [Description("Only used when DesignStyle is set to Full")]
    public Color FocusedBackground
    {
        get
        {
            return privateFocusedBackground;
        }
        set
        {
            privateFocusedBackground = value;
            Invalidate();
        }
    }

    private float privatePartialThickness = 2;
    public float PartialThickness
    {
        get
        {
            return privatePartialThickness;
        }
        set
        {
            privatePartialThickness = value;
            Invalidate();
        }
    }

    bool showCaret = false;

    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        e.Graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
        e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

        Rectangle modifiedCR = ClientRectangle;
        modifiedCR.Inflate(-1, -1);
        GraphicsPath path = Helper.RoundRect(modifiedCR, borderRadius);

        if (DesignStyle == Styles.Full)
        {
            if (Focused)
            {
                e.Graphics.FillPath(new SolidBrush(FocusedBackground), path);
                e.Graphics.DrawPath(new Pen(FocusedBorder, BorderSize), path);
            }
            else
            {
                e.Graphics.FillPath(new SolidBrush(Background), path);
                e.Graphics.DrawPath(new Pen(Border, BorderSize), path);
            }
        }
        else if (DesignStyle == Styles.Partial)
        {
            Matrix matrix = new Matrix();
            matrix.Translate(0, modifiedCR.Height - PartialThickness);
            path.Transform(matrix);
            if (Focused)
            {
                e.Graphics.FillPath(new SolidBrush(FocusedBorder), path);
            }
            else
            {
                e.Graphics.FillPath(new SolidBrush(Border), path);
            }
        }

        Point textLocation = new Point(Height / 8, (Height / 2) - (Font.Height / 2));
        if (Content == string.Empty && Focused == false)
        {
            e.Graphics.DrawString(Placeholder, Font, new SolidBrush(PlaceholderColor), textLocation);
        }
        else
        {
            if (UsePasswordChar)
            {
                e.Graphics.DrawString(StringToPassword(Content), Font, new SolidBrush(ForeColor), textLocation);
            }
            else
            {
                e.Graphics.DrawString(Content, Font, new SolidBrush(ForeColor), textLocation);
            }
        }

        if (showCaret)
        {
            int newXLoc = 0;
            if (UsePasswordChar)
            {
                newXLoc = textLocation.X + (int)Math.Ceiling(e.Graphics.MeasureString(StringToPassword(Content), Font).Width);

            }
            else
            {
                newXLoc = textLocation.X + (int)Math.Ceiling(e.Graphics.MeasureString(Content, Font).Width);
            }
            if (Content != string.Empty)
            {
                newXLoc -= 2;
                int countedWhiteSpaces = 1;
                int whiteSpaceWidth = 0;
                whiteSpaceWidth = (int)Math.Floor(e.Graphics.MeasureString(" ", Font).Width);
                foreach (char character in Content)
                {
                    if (Content[Content.Length - countedWhiteSpaces] == ' ')
                    {
                        newXLoc += whiteSpaceWidth;
                        countedWhiteSpaces++;
                    }
                }
            }
            Point textCaretStartLocation = new Point(newXLoc, textLocation.Y);
            Point textCaretEndLocation = new Point(textCaretStartLocation.X, textCaretStartLocation.Y + Font.Height);
            e.Graphics.DrawLine(new Pen(ForeColor, 1f), textCaretStartLocation, textCaretEndLocation);
        }

        base.OnPaint(e);
    }

    private string StringToPassword(string content)
    {
        string passwordString = "";
        foreach (char character in Content)
        {
            passwordString += "•";
        }
        return passwordString;
    }

    public bool ResetTextOnClick = false;

    protected override void OnMouseDown(MouseEventArgs e)
    {
        base.OnMouseDown(e);


        if (e.Button == MouseButtons.Left)
        {
            if (ResetTextOnClick)
            {
                Content = string.Empty;
            }
            Focus();
            showCaret = true;
        }

        Refresh();
    }

    public enum Styles
    {
        Full,
        Partial
    }

    public Styles privateDesignStyle = Styles.Partial;
    public Styles DesignStyle
    {
        get
        {
            return privateDesignStyle;
        }
        set
        {
            privateDesignStyle = value;
            Refresh();
        }
    }

    protected override void OnKeyPress(KeyPressEventArgs e)
    {
        base.OnKeyPress(e);

        if (e.KeyChar == '\b')
        {
            if (Content.Length > 0)
            {
                Content = Content.Remove(Content.Length - 1);
                ContentChanged?.Invoke(this, new EventArgs());
            }
        }
        else if (e.KeyChar == (char)3 && ModifierKeys == Keys.Control)
        {
            if (Content != string.Empty || Content != null)
            {
                Clipboard.SetText(Content + "\r");
            }
        }
        else if (e.KeyChar == (char)1 && ModifierKeys == Keys.Control)
        {
            if (Content != string.Empty || Content != null)
            {
                Clipboard.SetText(Content);
                Content = string.Empty + "\r";
                ContentChanged?.Invoke(this, new EventArgs());
            }
        }
        else if (e.KeyChar == (char)22 && ModifierKeys == Keys.Control)
        {
            if (Clipboard.ContainsText())
            {
                string textBefore = Clipboard.GetText();
                string sanitized = textBefore.Replace("\r", "").Replace("\n", "");
                Content += sanitized;
                ContentChanged?.Invoke(this, new EventArgs());
            }
        }
        else if (e.KeyChar == (char)Keys.Escape || e.KeyChar == (char)Keys.Enter)
        {
            Parent.FindForm().ActiveControl = null;
            showCaret = false;
            Refresh();

        }
        else if (Content.Length > 0 && e.KeyChar == ' ' && Content[Content.Length - 1] == ' ')
        {
            return;
        }
        else
        {
            if (ModifierKeys == Keys.None || ModifierKeys == Keys.Shift)
            {
                Content += e.KeyChar;
                ContentChanged?.Invoke(this, new EventArgs());
                showCaret = true;
                Refresh();
            }
        }
    }

    private void InitializeComponent()
    {
        this.SuspendLayout();
        // 
        // cuiTextBox
        // 
        this.Name = "cuiTextBox";
        this.Size = new System.Drawing.Size(183, 33);
        this.ResumeLayout(false);

    }
}