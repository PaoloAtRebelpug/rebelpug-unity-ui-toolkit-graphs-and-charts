using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UIElements;

namespace rebelpug_unity_ui_toolkit_graphs_and_charts.charts
{
    public class LineChart : VisualElement
    {
        private VisualElement _bar;
        private VisualElement _axes;
        private VisualElement _grid;
        private VisualElement _dots;
        private VisualElement _lines;
        private VisualElement _externalBackground;
        private VisualElement _background;

        private List<VisualElement> _dotsItems;
        private List<Label> _dotsLabelsItems;
        private List<VisualElement> _linesItems;

        private bool _isSetup;

        #region UXML
        [Preserve]
        public new class UxmlFactory : UxmlFactory<LineChart, UxmlTraits> { }

        [Preserve]
        public new class UxmlTraits : VisualElement.UxmlTraits { }
        #endregion

        public LineChart()
        {
            SetupExternalBackground();
            SetupBackground();
            
            _axes = new VisualElement
            {
                name = "axes",
                style =
                {
                    position = new StyleEnum<Position>(Position.Absolute),
                    width = new StyleLength(new Length(100, LengthUnit.Percent)),
                    height = new StyleLength(new Length(100, LengthUnit.Percent)),
                }
            };
            _background.Add(_axes);
            SetUpAxe("horizontal-graph-axe");
            SetUpAxe("vertical-graph-axe");
            
            _grid = new VisualElement()
            {
                name = "grid",
                style =
                {
                    position = new StyleEnum<Position>(Position.Absolute),
                    width = new StyleLength(new Length(100, LengthUnit.Percent)),
                    height = new StyleLength(new Length(100, LengthUnit.Percent)),
                }
            };
            _background.Add(_grid);
            SetupGrid();
            
            _linesItems = new List<VisualElement>();
            _lines = new VisualElement()
            {
                name = "lines",
                style =
                {
                    position = new StyleEnum<Position>(Position.Absolute),
                    width = new StyleLength(new Length(100, LengthUnit.Percent)),
                    height = new StyleLength(new Length(100, LengthUnit.Percent)),
                }
            };
            _background.Add(_lines);
            
            _dotsItems = new List<VisualElement>();
            _dotsLabelsItems = new List<Label>();
            _dots = new VisualElement()
            {
                name = "dots",
                style =
                {
                    position = new StyleEnum<Position>(Position.Absolute),
                    width = new StyleLength(new Length(100, LengthUnit.Percent)),
                    height = new StyleLength(new Length(100, LengthUnit.Percent)),
                }
            };
            _background.Add(_dots);
        }

        #region Initializers
        public void EnableGraph(List<Vector2> points, List<string> labels)
        {
            for (var i = 0; i < points.Count - 1; i++)
            {
                var point1 = points[i];
                var point2 = points[i + 1];
                SetUpLine(i, point1, point2);
            }
            
            for (var i = 1; i < points.Count; i++)
            {
                var point1 = points[i];
                var label = labels[i];
                SetUpDot(i, point1, label);
            }

            _isSetup = true;
        }
        #endregion

        #region Update Graph
        /*
         * Here pass a list of points and labels to be used in the graph
         */
        public void UpdateGraph(List<Vector2> points, List<string> labels)
        {
            if (!_isSetup)
            {
                EnableGraph(points, labels);
            }
            else
            {
                UpdateGraphValues(points, labels);
            }
        }
        
        private void UpdateGraphValues(List<Vector2> points, List<string> labels)
        {
            for (var i = 0; i < points.Count - 1; i++)
            {
                var point1 = points[i];
                var point2 = points[i + 1];
                var line = _linesItems[i];
                SetLinePosition(ref line, point1, point2);
            }
            
            for (var i = 1; i < points.Count; i++)
            {
                var newPoint = points[i];
                var newLabel = labels[i];
                var dot = _dotsItems[i - 1];
                var label = _dotsLabelsItems[i - 1];
                SetDotPosition(ref dot, newPoint);
                SetDotMessage(ref label, newLabel);
            }
        }
        #endregion

        #region Graph Layout
        private void SetupExternalBackground()
        {
            _externalBackground = new VisualElement()
            {
                name = "background",
                style =
                {
                    position = new StyleEnum<Position>(Position.Absolute),
                    width = new StyleLength(new Length(100, LengthUnit.Percent)),
                    height = new StyleLength(new Length(100, LengthUnit.Percent)),
                }
            };
            _externalBackground.name = "graph-external-background";
            _externalBackground.AddToClassList("graph-external-background");
            Add(_externalBackground);
        }
        
        private void SetupBackground()
        {
            _background = new VisualElement()
            {
                name = "background",
                style =
                {
                    position = new StyleEnum<Position>(Position.Absolute),
                    width = new StyleLength(new Length(100, LengthUnit.Percent)),
                    height = new StyleLength(new Length(100, LengthUnit.Percent)),
                }
            };
            _background.name = "graph-background";
            _background.AddToClassList("graph-background");
            _externalBackground.Add(_background);
        }
        
        private void SetUpAxe(string className)
        {
            var axe = new VisualElement
            {
                style =
                {
                    position = new StyleEnum<Position>(Position.Absolute),
                    bottom = new StyleLength(new Length(0, LengthUnit.Percent)),
                    left = new StyleLength(new Length(0, LengthUnit.Percent)),
                },
                name = "axe"
            };

            axe.AddToClassList(className);
            _axes.Add(axe);
        }

        private void SetupGrid()
        {
            for (var i = 1; i <= 10; i++)
            {
                SetUpGridLine(0, i * 10,"vertical-grid-line");
            }
            
            for (var j = 1; j <= 10; j++)
            {
                SetUpGridLine(j * 10, 0, "horizontal-grid-line");
            }
        }
        
        private void SetUpGridLine(float leftPos, float bottomPos, string className)
        {
            var gridLine = new VisualElement
            {
                style =
                {
                    position = new StyleEnum<Position>(Position.Absolute),
                    bottom = new StyleLength(new Length(leftPos, LengthUnit.Percent)),
                    left = new StyleLength(new Length(bottomPos, LengthUnit.Percent)),
                },
                name = "grid-line"
            };
            gridLine.AddToClassList(className);
            _grid.Add(gridLine);
        }
        #endregion

        #region Linear Chart
        private void SetUpLine(int id, Vector2 point1, Vector2 point2)
        {
            var line = new VisualElement
            {
                name = "graph-line-" + id
            };

            SetLinePosition(ref line, point1, point2);

            line.AddToClassList("graph-line");
            
            _linesItems.Add(line);
            _lines.Add(line);
        }

        private void SetLinePosition(ref VisualElement line, Vector2 point1, Vector2 point2)
        {
            var length = Mathf.Sqrt((point2.x - point1.x) * (point2.x - point1.x) + (point2.y - point1.y) * (point2.y - point1.y));
            var inclDeg = Mathf.Atan2(point2.y - point1.y, point2.x - point1.x) * Mathf.Rad2Deg;
            line.style.position = new StyleEnum<Position>(Position.Absolute);
            line.style.bottom = new StyleLength(new Length(point1.y, LengthUnit.Percent));
            line.style.left = new StyleLength(new Length(point1.x, LengthUnit.Percent));
            line.style.rotate = new StyleRotate(new Rotate(-inclDeg));
            line.style.width = new StyleLength(new Length(length, LengthUnit.Percent));
        }
        
        private void SetUpDot(int id, Vector2 point, string message)
        {
            var dot = new VisualElement
            {
                name = "graph-dot-" + id
            };

            SetDotPosition(ref dot, point);

            dot.AddToClassList("graph-dot");
            _dotsItems.Add(dot);
            _dots.Add(dot);
            
            var messageLabel = new Label
            {
                text = message
            };
            SetDotMessage(ref messageLabel, message);

            messageLabel.AddToClassList("graph-dot-message");
            _dotsLabelsItems.Add(messageLabel);
            dot.Add(messageLabel);
        }
        
        private void SetDotPosition(ref VisualElement dot, Vector2 point)
        {
            dot.style.position = new StyleEnum<Position>(Position.Absolute);
            dot.style.bottom = new StyleLength(new Length(point.y - 1, LengthUnit.Percent));
            dot.style.left = new StyleLength(new Length(point.x - 1, LengthUnit.Percent));
        }
        
        private void SetDotMessage(ref Label dotLabel, string message)
        {
            dotLabel.text = message;
        }
        #endregion
    }
}