using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class Debugger : MonoBehaviour
    {
        public static Debugger Instance { get; private set; }

        private bool IsShowingDebug { get; set; }

        [SerializeField] private GameObject _debug;
        [SerializeField] private Text _debugTitle;
        [SerializeField] private Text _debugText;

        private void Awake()
        {
            Assert.IsNotNull(this._debug, $"{this.GetType().Name} had no reference to the Debug game object, but it requires it.");
            Assert.IsNotNull(this._debugTitle, $"{this.GetType().Name} had no reference to the Debug Title text, but it requires it.");
            Assert.IsNotNull(this._debugText, $"{this.GetType().Name} had no reference to the Debug Text, but it requires it.");

            Instance = this;

            this.IsShowingDebug = false;
            this._debug.gameObject.SetActive(false);
        }

        /// <summary>
        /// Shows a debug text.
        /// </summary>
        /// <param name="title">Title of the debug.</param>
        /// <param name="debugText">Debug text to show.</param>
        public void Show(string title, string debugText)
        {
            if (!this.IsShowingDebug)
            {
                this._debug.gameObject.SetActive(true);
                this.IsShowingDebug = true;
            }

            this._debugTitle.text = title;
            this._debugText.text = debugText;
        }

        /// <summary>
        /// Hides the debug information on screen.
        /// </summary>
        public void Hide()
        {
            this.IsShowingDebug = false;
            this._debug.gameObject.SetActive(false);
        }
    }
}
