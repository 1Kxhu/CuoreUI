using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using static CuoreUI.Drawing;

namespace CuoreUI.Components
{
    [ToolboxBitmap(typeof(TrackBar))]
    public partial class cuiControlAnimator : Component
    {
        public cuiControlAnimator()
        {
            InitializeComponent();
        }

        [Description("The control to animate.")]
        public Control TargetControl
        {
            get; set;
        }

        private int privateDuration = 1000;

        [Description("How long the animation should last in milliseconds. (ms)")]
        public int Duration
        {
            get
            {
                return privateDuration;
            }
            set
            {
                privateDuration = value;
            }
        }

        [Description("Choose the easing type that suits the best.")]
        public EasingTypes EasingType
        {
            get;
            set;
        } = EasingTypes.QuadInOut;

        [Description("Where the TargetControl should be moved to.")]
        public Point TargetLocation
        {
            get;
            set;
        } = Point.Empty;

        private int startX;
        private int startY;
        private double xDistance;
        private double yDistance;

        private double elapsedTime = 0;
        private bool animating = false;
        private bool animationFinished = true;

        public async Task AnimateLocation()
        {

            if (animating || TargetControl == null)
                return;
            animating = true;

            startX = TargetControl.Left;
            startY = TargetControl.Top;

            xDistance = -(startX - TargetLocation.X);
            yDistance = -(startY - TargetLocation.Y);

            DateTime lastFrameTime = DateTime.Now;

            double durationRatio = Duration / (double)1000;

            //MessageBox.Show(durationRatio.ToString() + $", d:{Duration}, recalc:{Duration/(double)1000}");

            EmergencySetLocation(Duration);

            animationFinished = false;

            AnimationStarted?.Invoke(this, EventArgs.Empty);

            while (true)
            {

                DateTime rightnow = DateTime.Now;
                double elapsedMilliseconds = (rightnow - lastFrameTime).TotalMilliseconds;
                lastFrameTime = rightnow;

                // uhhhhh this is so weird but it works..
                elapsedTime += (elapsedMilliseconds / Duration);

                if (elapsedTime >= Duration || IsAnimationFinished())
                {
                    animating = false;
                    animationFinished = false;
                    elapsedTime = 0;
                    //MessageBox.Show($"{elapsedTime}, {privateDuration}");
                    return;
                }

                double quad = CuoreUI.Drawing.EasingFunctions.FromEasingType(EasingType, elapsedTime, Duration / (double)1000) * durationRatio;
                TargetControl.Left = startX + (int)(xDistance * quad);
                TargetControl.Top = startY + (int)(yDistance * quad);

                await Task.Delay(1000 / Drawing.GetHighestRefreshRate());
            }
        }

        public bool IsAnimationFinished()
        {
            return animationFinished;
        }

        private async void EmergencySetLocation(int Duration)
        {
            animationFinished = false;
            await Task.Delay(Duration + (1000 / Drawing.GetHighestRefreshRate()));
            TargetControl.Left = startX + (int)(xDistance);
            TargetControl.Top = startY + (int)(yDistance);
            animationFinished = true;
            animating = false;
            elapsedTime = 0;
            AnimationEnded?.Invoke(this, EventArgs.Empty);
        }

        public EventHandler AnimationEnded;
        public EventHandler AnimationStarted;
    }
}