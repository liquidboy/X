using System;
using System.Globalization;
using Microsoft.Graphics.Canvas.Geometry;
using Windows.Foundation;


namespace X.UI.EffectLayer
{
    public class PathToD2DPathGeometryConverter
    {

        const bool AllowSign = true;
        const bool AllowComma = true;
        const bool IsFilled = true;
        const bool IsClosed = true;

        IFormatProvider _formatProvider;

        //GeometrySink _figure = null;     // Figure object, which will accept parsed segments
        //CanvasGeometry _pathGeometry = null;

        string _pathString;        // Input string to be parsed
        int _pathLength;
        int _curIndex;          // Location to read next character from
        bool _figureStarted;     // StartFigure is effective 

        Point _lastStart;         // Last figure starting point
        Point _lastPoint;         // Last point 
        Point _secondLastPoint;   // The point before last point

        char _token;             // Non whitespace character returned by ReadToken




        public CanvasGeometry parse(string path, CanvasPathBuilder builder)
        {
            var _builder = builder;

            //var _pathGeometry = Microsoft.Graphics.Canvas.Geometry.CanvasGeometry.CreatePath(_builder);

            //_figure = _builder.BeginFigure();
            ////GeometrySink _sink  = _pathGeometry.Open();

            _formatProvider = CultureInfo.InvariantCulture;
            _pathString = path;
            _pathLength = path.Length;
            _curIndex = 0;

            _secondLastPoint = new Point(0, 0);
            _lastPoint = new Point(0, 0);
            _lastStart = new Point(0, 0);

            _figureStarted = false;

            bool first = true;

            char last_cmd = ' ';

            while (ReadToken()) // Empty path is allowed in XAML
            {
                char cmd = _token;

                if (first)
                {
                    if ((cmd != 'M') && (cmd != 'm'))  // Path starts with M|m 
                    {
                        ThrowBadToken();
                    }

                    first = false;
                }

                switch (cmd)
                {
                    case 'm':
                    case 'M':
                        // XAML allows multiple points after M/m
                        _lastPoint = ReadPoint(cmd, !AllowComma);


                        _builder.BeginFigure((float)_lastPoint.X, (float)_lastPoint.Y, IsFilled ? CanvasFigureFill.Default : CanvasFigureFill.DoesNotAffectFills);   // FigureBegin.Filled : FigureBegin.Hollow);
                        //_figure.StartPoint = _lastPoint;
                        //_figure.IsFilled = IsFilled;
                        //if (!IsClosed) _figure.Close();
                        //_figure.IsClosed = !IsClosed;
                        //context.BeginFigure(_lastPoint, IsFilled, !IsClosed);
                        _figureStarted = true;
                        _lastStart = _lastPoint;
                        last_cmd = 'M';

                        while (IsNumber(AllowComma))
                        {
                            _lastPoint = ReadPoint(cmd, !AllowComma);

                            //LineSegment _lineSegment = new LineSegment();
                            //_lineSegment.Point = _lastPoint;
                            _builder.AddLine((float)_lastPoint.X, (float)_lastPoint.Y);
                            //_figure.Segments.Add(_lineSegment);
                            //context.LineTo(_lastPoint, IsStroked, !IsSmoothJoin);
                            last_cmd = 'L';
                        }
                        break;

                    case 'l':
                    case 'L':
                    case 'h':
                    case 'H':
                    case 'v':
                    case 'V':
                        EnsureFigure(_builder);

                        do
                        {
                            switch (cmd)
                            {
                                case 'l': _lastPoint = ReadPoint(cmd, !AllowComma); break;
                                case 'L': _lastPoint = ReadPoint(cmd, !AllowComma); break;
                                case 'h': _lastPoint.X += ReadNumber(!AllowComma); break;
                                case 'H': _lastPoint.X = ReadNumber(!AllowComma); break;
                                case 'v': _lastPoint.Y += ReadNumber(!AllowComma); break;
                                case 'V': _lastPoint.Y = ReadNumber(!AllowComma); break;
                            }

                            //LineSegment _lineSegment = new LineSegment();
                            //_lineSegment.Point = _lastPoint;
                            //_figure.Segments.Add(_lineSegment);
                            _builder.AddLine((float)_lastPoint.X, (float)_lastPoint.Y);
                            //context.LineTo(_lastPoint, IsStroked, !IsSmoothJoin);
                        }
                        while (IsNumber(AllowComma));

                        last_cmd = 'L';
                        break;

                    case 'c':
                    case 'C': // cubic Bezier 
                    case 's':
                    case 'S': // smooth cublic Bezier
                        EnsureFigure(_builder);

                        do
                        {
                            Point p;

                            if ((cmd == 's') || (cmd == 'S'))
                            {
                                if (last_cmd == 'C')
                                {
                                    p = Reflect();
                                }
                                else
                                {
                                    p = _lastPoint;
                                }

                                _secondLastPoint = ReadPoint(cmd, !AllowComma);
                            }
                            else
                            {
                                p = ReadPoint(cmd, !AllowComma);

                                _secondLastPoint = ReadPoint(cmd, AllowComma);
                            }

                            _lastPoint = ReadPoint(cmd, AllowComma);

                            //BezierSegment _bizierSegment = new BezierSegment();
                            //_bizierSegment.Point1 = p;
                            //_bizierSegment.Point2 = _secondLastPoint;
                            //_bizierSegment.Point3 = _lastPoint;
                            //_figure.Segments.Add(_bizierSegment);
                            _builder.AddCubicBezier(
                                new System.Numerics.Vector2((float)p.X, (float)p.Y),
                                new System.Numerics.Vector2((float)_secondLastPoint.X, (float)_secondLastPoint.Y),
                                new System.Numerics.Vector2((float)_lastPoint.X, (float)_lastPoint.Y)
                            );
                            //context.BezierTo(p, _secondLastPoint, _lastPoint, IsStroked, !IsSmoothJoin);

                            last_cmd = 'C';
                        }
                        while (IsNumber(AllowComma));

                        break;

                    case 'q':
                    case 'Q': // quadratic Bezier 
                    case 't':
                    case 'T': // smooth quadratic Bezier
                        EnsureFigure(_builder);

                        do
                        {
                            if ((cmd == 't') || (cmd == 'T'))
                            {
                                if (last_cmd == 'Q')
                                {
                                    _secondLastPoint = Reflect();
                                }
                                else
                                {
                                    _secondLastPoint = _lastPoint;
                                }

                                _lastPoint = ReadPoint(cmd, !AllowComma);
                            }
                            else
                            {
                                _secondLastPoint = ReadPoint(cmd, !AllowComma);
                                _lastPoint = ReadPoint(cmd, AllowComma);
                            }

                            //QuadraticBezierSegment _quadraticBezierSegment = new QuadraticBezierSegment();
                            //_quadraticBezierSegment.Point1 = _secondLastPoint;
                            //_quadraticBezierSegment.Point2 = _lastPoint;
                            //_figure.Segments.Add(_quadraticBezierSegment);
                            _builder.AddQuadraticBezier(

                                new System.Numerics.Vector2((float)_secondLastPoint.X, (float)_secondLastPoint.Y),
                                new System.Numerics.Vector2((float)_lastPoint.X, (float)_lastPoint.Y)
                            );
                            //context.QuadraticBezierTo(_secondLastPoint, _lastPoint, IsStroked, !IsSmoothJoin);

                            last_cmd = 'Q';
                        }
                        while (IsNumber(AllowComma));

                        break;

                    case 'a':
                    case 'A':
                        EnsureFigure(_builder);

                        do
                        {
                            // A 3,4 5, 0, 0, 6,7
                            double w = ReadNumber(!AllowComma);
                            double h = ReadNumber(AllowComma);
                            double rotation = ReadNumber(AllowComma);
                            bool large = ReadBool();
                            bool sweep = ReadBool();

                            _lastPoint = ReadPoint(cmd, AllowComma);

                            //ArcSegment _arcSegment = new ArcSegment();
                            //_arcSegment.Point = _lastPoint;
                            //_arcSegment.Size = new Size(w, h);
                            //_arcSegment.RotationAngle = rotation;
                            //_arcSegment.IsLargeArc = large;
                            //_arcSegment.SweepDirection = sweep ? SweepDirection.Clockwise : SweepDirection.Counterclockwise;
                            //_figure.Segments.Add(_arcSegment);

                            _builder.AddArc(
                                new System.Numerics.Vector2((float)_lastPoint.X, (float)_lastPoint.Y),
                                (float)w,
                                (float)h,
                                (float)rotation,
                                sweep ? CanvasSweepDirection.Clockwise : CanvasSweepDirection.CounterClockwise,
                                large ? CanvasArcSize.Large : CanvasArcSize.Small

                            );
                            //context.ArcTo(
                            //    _lastPoint,
                            //    new Size(w, h),
                            //    rotation,
                            //    large,
                            //    sweep ? SweepDirection.Clockwise : SweepDirection.Counterclockwise,
                            //    IsStroked,
                            //    !IsSmoothJoin
                            //    );
                        }
                        while (IsNumber(AllowComma));

                        last_cmd = 'A';
                        break;

                    case 'z':
                    case 'Z':
                        EnsureFigure(_builder);

                        //_figure.IsClosed = IsClosed;
                        //context.SetClosedState(IsClosed);
                        _builder.EndFigure(IsClosed ? CanvasFigureLoop.Closed : CanvasFigureLoop.Open);
                        _figureStarted = false;
                        last_cmd = 'Z';

                        _lastPoint = _lastStart; // Set reference point to be first point of current figure
                        break;

                    default:
                        ThrowBadToken();
                        break;
                }
            }

            //if (null != _figure)
            //{
            //    _pathGeometry = new PathGeometry(d2dfactory);
            //    _pathGeometry.Figures.Add(_figure);

            //}

            //_builder.EndFigure();
            //_sink.Close();
            var _pathGeometry = Microsoft.Graphics.Canvas.Geometry.CanvasGeometry.CreatePath(_builder);

            _builder = null;

            return _pathGeometry;
        }

        void SkipDigits(bool signAllowed)
        {
            // Allow for a sign 
            if (signAllowed && More() && ((_pathString[_curIndex] == '-') || _pathString[_curIndex] == '+'))
            {
                _curIndex++;
            }

            while (More() && (_pathString[_curIndex] >= '0') && (_pathString[_curIndex] <= '9'))
            {
                _curIndex++;
            }
        }

        bool ReadBool()
        {
            SkipWhiteSpace(AllowComma);

            if (More())
            {
                _token = _pathString[_curIndex++];

                if (_token == '0')
                {
                    return false;
                }
                else if (_token == '1')
                {
                    return true;
                }
            }

            ThrowBadToken();

            return false;
        }

        private Point Reflect()
        {
            return new Point(2 * _lastPoint.X - _secondLastPoint.X,
                             2 * _lastPoint.Y - _secondLastPoint.Y);
        }

        private void EnsureFigure(CanvasPathBuilder builder)
        {
            if (!_figureStarted)
            {
                //_figure = _builder.BeginFigure();
                //_figure = new PathFigure();
                //_figure.StartPoint = _lastStart;
                builder.BeginFigure((float)_lastStart.X, (float)_lastStart.Y, CanvasFigureFill.Default);

                //_context.BeginFigure(_lastStart, IsFilled, !IsClosed);
                _figureStarted = true;
            }
        }

        double ReadNumber(bool allowComma)
        {
            if (!IsNumber(allowComma))
            {
                ThrowBadToken();
            }

            bool simple = true;
            int start = _curIndex;

            //
            // Allow for a sign
            //
            // There are numbers that cannot be preceded with a sign, for instance, -NaN, but it's 
            // fine to ignore that at this point, since the CLR parser will catch this later.
            // 
            if (More() && ((_pathString[_curIndex] == '-') || _pathString[_curIndex] == '+'))
            {
                _curIndex++;
            }

            // Check for Infinity (or -Infinity).
            if (More() && (_pathString[_curIndex] == 'I'))
            {
                // 
                // Don't bother reading the characters, as the CLR parser will 
                // do this for us later.
                // 
                _curIndex = Math.Min(_curIndex + 8, _pathLength); // "Infinity" has 8 characters
                simple = false;
            }
            // Check for NaN 
            else if (More() && (_pathString[_curIndex] == 'N'))
            {
                // 
                // Don't bother reading the characters, as the CLR parser will
                // do this for us later. 
                //
                _curIndex = Math.Min(_curIndex + 3, _pathLength); // "NaN" has 3 characters
                simple = false;
            }
            else
            {
                SkipDigits(!AllowSign);

                // Optional period, followed by more digits 
                if (More() && (_pathString[_curIndex] == '.'))
                {
                    simple = false;
                    _curIndex++;
                    SkipDigits(!AllowSign);
                }

                // Exponent
                if (More() && ((_pathString[_curIndex] == 'E') || (_pathString[_curIndex] == 'e')))
                {
                    simple = false;
                    _curIndex++;
                    SkipDigits(AllowSign);
                }
            }

            if (simple && (_curIndex <= (start + 8))) // 32-bit integer
            {
                int sign = 1;

                if (_pathString[start] == '+')
                {
                    start++;
                }
                else if (_pathString[start] == '-')
                {
                    start++;
                    sign = -1;
                }

                int value = 0;

                while (start < _curIndex)
                {
                    value = value * 10 + (_pathString[start] - '0');
                    start++;
                }

                return value * sign;
            }
            else
            {
                string subString = _pathString.Substring(start, _curIndex - start);

                try
                {
                    return System.Convert.ToDouble(subString, _formatProvider);
                }
                catch (FormatException except)
                {
                    throw new FormatException(string.Format("Unexpected character in path '{0}' at position {1}", _pathString, _curIndex - 1), except);
                }
            }
        }

        private bool IsNumber(bool allowComma)
        {
            bool commaMet = SkipWhiteSpace(allowComma);

            if (More())
            {
                _token = _pathString[_curIndex];

                // Valid start of a number
                if ((_token == '.') || (_token == '-') || (_token == '+') || ((_token >= '0') && (_token <= '9'))
                    || (_token == 'I')  // Infinity
                    || (_token == 'N')) // NaN 
                {
                    return true;
                }
            }

            if (commaMet) // Only allowed between numbers
            {
                ThrowBadToken();
            }

            return false;
        }

        private Point ReadPoint(char cmd, bool allowcomma)
        {
            double x = ReadNumber(allowcomma);
            double y = ReadNumber(AllowComma);

            if (cmd >= 'a') // 'A' < 'a'. lower case for relative
            {
                x += _lastPoint.X;
                y += _lastPoint.Y;
            }

            return new Point(x, y);
        }

        private bool ReadToken()
        {
            SkipWhiteSpace(!AllowComma);

            // Check for end of string 
            if (More())
            {
                _token = _pathString[_curIndex++];

                return true;
            }
            else
            {
                return false;
            }
        }

        bool More()
        {
            return _curIndex < _pathLength;
        }

        // Skip white space, one comma if allowed
        private bool SkipWhiteSpace(bool allowComma)
        {
            bool commaMet = false;

            while (More())
            {
                char ch = _pathString[_curIndex];

                switch (ch)
                {
                    case ' ':
                    case '\n':
                    case '\r':
                    case '\t': // SVG whitespace 
                        break;

                    case ',':
                        if (allowComma)
                        {
                            commaMet = true;
                            allowComma = false; // one comma only
                        }
                        else
                        {
                            ThrowBadToken();
                        }
                        break;

                    default:
                        // Avoid calling IsWhiteSpace for ch in (' ' .. 'z']
                        if (((ch > ' ') && (ch <= 'z')) || !Char.IsWhiteSpace(ch))
                        {
                            return commaMet;
                        }
                        break;
                }

                _curIndex++;
            }

            return commaMet;
        }

        private void ThrowBadToken()
        {
            throw new FormatException(string.Format("Unexpected character in path '{0}' at position {1}", _pathString, _curIndex - 1));
        }

        static internal char GetNumericListSeparator(IFormatProvider provider)
        {
            char numericSeparator = ',';

            // Get the NumberFormatInfo out of the provider, if possible
            // If the IFormatProvider doesn't not contain a NumberFormatInfo, then 
            // this method returns the current culture's NumberFormatInfo. 
            NumberFormatInfo numberFormat = NumberFormatInfo.GetInstance(provider);

            // Is the decimal separator is the same as the list separator?
            // If so, we use the ";". 
            if ((numberFormat.NumberDecimalSeparator.Length > 0) && (numericSeparator == numberFormat.NumberDecimalSeparator[0]))
            {
                numericSeparator = ';';
            }

            return numericSeparator;
        }



    }
}
