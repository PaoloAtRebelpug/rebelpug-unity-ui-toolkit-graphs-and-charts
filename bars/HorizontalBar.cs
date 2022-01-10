using BehaviorDesigner.Runtime.Tasks.Unity.UnityLight;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Scripting;
using UnityEngine.UIElements;

namespace ui.Generics
{
    public class HorizontalBar : VisualElement
    {
        private VisualElement _bar;
        
        #region UXML
        [Preserve]
        public new class UxmlFactory : UxmlFactory<HorizontalBar, UxmlTraits> { }

        [Preserve]
        public new class UxmlTraits : VisualElement.UxmlTraits { }
        #endregion

        public HorizontalBar()
        {
            _bar = new VisualElement();
            _bar.AddToClassList("horizontal-bar");
            Add(_bar);
            UpdateBarPosition(50);
        }

        public void SetupColorBar(Color setColor, float position)
        {
            _bar.style.backgroundColor = new StyleColor(setColor);
            UpdateBarPosition(position);
        }

        public void UpdateBarPosition(float position)
        {
            _bar.style.width = new StyleLength(new Length(position, LengthUnit.Percent));
        }
    }
}