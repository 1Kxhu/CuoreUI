using CuoreUI;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

public class cuiTextBox : UserControl
{
    private int borderRadius = 10; // Default border radius

    public int BorderRadius
    {
        get
        {
            return borderRadius;
        }
        set
        {
            if (value >= 0)
            {
                borderRadius = value;
                Invalidate();
            }
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

    Timer caretBlinkTimer = new Timer();

    public cuiTextBox()
    {
        DoubleBuffered = true;
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
            Refresh();
        };
        caretBlinkTimer.Start();
    }

    private Color privateBackground = Color.White;
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


    private Color privateBorder = Color.White;
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

    private float privateBorderSize = 1.0f;
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

    private Color privateFocusedBorder = Color.White;
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


    private Color privateFocusedBackground = Color.White;
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

    bool showCaret = false;

    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        e.Graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
        e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

        GraphicsPath path = Helper.RoundRect(ClientRectangle, borderRadius);

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

        Point textLocation = new Point(Height / 8, (Height / 2) - (Font.Height / 2));
        e.Graphics.DrawString(Content, Font, new SolidBrush(ForeColor), textLocation);

        // caret code
        if (showCaret)
        {
            int newXLoc = textLocation.X + (int)Math.Ceiling(e.Graphics.MeasureString(Content, Font).Width);
            if (Content != string.Empty)
            {
                newXLoc -= 2;
                int countedWhiteSpaces = 1;
                int whiteSpaceWidth = (int)Math.Floor(e.Graphics.MeasureString(" ", Font).Width);
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
        }

        Refresh();
    }


    protected override void OnKeyPress(KeyPressEventArgs e)
    {
        base.OnKeyPress(e);

        if (e.KeyChar == '\b')
        {
            if (Content.Length > 0)
            {
                Content = Content.Remove(Content.Length - 1);
            }
        }
        else if (e.KeyChar == (char)3 && ModifierKeys == Keys.Control) // Ctrl + C
        {
            if (Content != string.Empty || Content != null)
            {
                Clipboard.SetText(Content + "\r");
            }
        }
        else if (e.KeyChar == (char)1 && ModifierKeys == Keys.Control) // Ctrl + A
        {
            if (Content != string.Empty || Content != null)
            {
                Clipboard.SetText(Content);
                Content = string.Empty + "\r";
            }
        }
        else if (e.KeyChar == (char)22 && ModifierKeys == Keys.Control) // Ctrl + V
        {
            if (Clipboard.ContainsText())
            {
                string textBefore = Clipboard.GetText();
                string sanitized = textBefore.Replace("\r", "").Replace("\n", "");
                Content += sanitized;
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
                showCaret = true;
                Refresh();
            }
        }
    }
}