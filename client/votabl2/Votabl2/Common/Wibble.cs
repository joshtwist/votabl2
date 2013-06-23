using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace Votabl2
{
    public class Wibble : DependencyObject
    {
        public static object GetProperty(DependencyObject obj)
        {
            return (object)obj.GetValue(PropertyProperty);
        }

        public static void SetProperty(DependencyObject obj, object value)
        {
            obj.SetValue(PropertyProperty, value);
        }

        // Using a DependencyProperty as the backing store for Property.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PropertyProperty =
            DependencyProperty.RegisterAttached("Property", typeof(object), typeof(Wibble), new PropertyMetadata(null, PropertyChanged));

        public static void PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            int half = 30;
            int arc = 5;

            if (e.OldValue == null)
            {
                return;
            }

            FrameworkElement fe = d as FrameworkElement;
            fe.RenderTransformOrigin = new Point(0.5, 0.5);

            RotateTransform rotate = fe.RenderTransform as RotateTransform;
            if (rotate == null)
            {
                rotate = new RotateTransform();
                rotate.CenterX = 0.5;
                rotate.CenterY = 0.5;
                fe.RenderTransform = rotate;
            }

            DoubleAnimationUsingKeyFrames da = new DoubleAnimationUsingKeyFrames();
            da.KeyFrames.Add(new EasingDoubleKeyFrame() { KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(0)), Value = 0 });
            da.KeyFrames.Add(new EasingDoubleKeyFrame() { KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(half)), Value = arc });
            da.KeyFrames.Add(new EasingDoubleKeyFrame() { KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(half * 3)), Value = -arc });
            da.KeyFrames.Add(new EasingDoubleKeyFrame() { KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(half * 4)), Value = 0 });

            Storyboard sb = new Storyboard();
            Storyboard.SetTarget(da, rotate);
            Storyboard.SetTargetProperty(da, "Angle");

            sb.Children.Add(da);

            sb.Begin();
        }
    }
}
