using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace UITest
{
    /// <summary>
    /// IPhoneMessageView.xaml 的交互逻辑
    /// </summary>
    public partial class IPhoneMessageView : Window
    {
        public IPhoneMessageView()
        {
            InitializeComponent();
        }
    }

    public class MessageDecorator : Decorator
    {
        private Thickness _border = new Thickness(25, 6, 25, 6);

        public MessageDecorator()
        {
            this.HorizontalAlignment = HorizontalAlignment.Right;
        }

        public bool Direction
        {
            get { return (bool)GetValue(DirectionProperty); }
            set { SetValue(DirectionProperty, value); }
        }

        public static readonly DependencyProperty DirectionProperty =
            DependencyProperty.Register("Direction", typeof(bool), typeof(MessageDecorator),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender, ChangeCallbcak));

        private static void ChangeCallbcak(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var messageDecorator = d as MessageDecorator;

            messageDecorator.HorizontalAlignment = (bool)e.NewValue ? HorizontalAlignment.Right : HorizontalAlignment.Left;
        }


        protected override Size MeasureOverride(Size constraint)
        {
            Size size = new Size();
            if (Child != null)
            {
                Child.Measure(constraint);

                size.Width = Child.DesiredSize.Width + _border.Left + _border.Right;
                size.Height = Child.DesiredSize.Height + _border.Top + _border.Bottom;
                if (size.Height < 35)
                {
                    size.Height = 35;
                    _border.Top = _border.Bottom = (size.Height - Child.DesiredSize.Height) / 2;
                }

            }
            return size;
        }
        protected override Size ArrangeOverride(Size arrangeSize)
        {
            if (Child != null)
            {
                Child.Arrange(new Rect(new Point(_border.Left, _border.Top), Child.DesiredSize));
            }
            return arrangeSize;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            if (Child != null)
            {
                Geometry geometry = null;
                Brush brush = null;
                Pen pen = new Pen();
                pen.Brush = new SolidColorBrush(Colors.Black);
                pen.Thickness = 1;

                if (Direction)
                {
                    geometry = CreateGeometry("right");
                    brush = createbrush("right");
                }
                else
                {
                    geometry = CreateGeometry("left");
                    brush = createbrush("left");
                }
                drawingContext.DrawGeometry(brush, pen, geometry);

                //GradientStopCollection collection = new GradientStopCollection();
                //collection.Add(new GradientStop(Colors.LightBlue, 0));
                //collection.Add(new GradientStop(Colors.White, 1));
                //Brush lightBrush = new LinearGradientBrush(collection, new Point(0, 0),
                //                                           new Point(0, 1));
                //drawingContext.DrawRoundedRectangle(lightBrush, null, new Rect(22, 1, this.ActualWidth - 45, 20), 10, 10);
            }
            //  base.OnRender(drawingContext);
        }

        private Brush createbrush(string right)
        {
            GradientStopCollection collection1 = new GradientStopCollection();
            collection1.Add(new GradientStop(Colors.Green, 0));
            collection1.Add(new GradientStop(Colors.White, 1));
            Brush brush = new LinearGradientBrush(collection1);
            GradientStopCollection collection2 = new GradientStopCollection();
            collection2.Add(new GradientStop(Colors.Brown, 0));
            collection2.Add(new GradientStop(Colors.White, 1));
            Brush brush2 = new LinearGradientBrush(collection1);
            return right == "right" ? brush : brush2;
        }

        private Geometry CreateGeometry(string right)
        {
        //    PathGeometry geometry = new PathGeometry();
        //    PathFigure figure = new PathFigure();
        //    figure.StartPoint = new Point(0, 0);
        //    figure.Segments.Add(new BezierSegment(
        //new Point(100, 0),
        //new Point(120, 50),
        //new Point(0, 50), true));
        //    geometry.Figures.Add(figure);
        //    return geometry;
            return null;
        }
    }
}
