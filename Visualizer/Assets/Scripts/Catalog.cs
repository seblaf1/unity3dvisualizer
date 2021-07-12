using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

namespace Assets.Scripts
{
    public class Catalog : MonoBehaviour
    {
        [SerializeField] private GridLayoutGroup _layout;
        [SerializeField] private Button _buttonPrefab;

        private void Awake()
        {
            Assert.IsNotNull(this._layout, $"{this.gameObject.name} had no reference to its layout, but it requires one.");
            Assert.IsNotNull(this._buttonPrefab, $"{this.gameObject.name} had no reference to its button prefab, but it requires one.");
        }

        /// <summary>
        /// Setup this catalog.
        /// </summary>
        /// <typeparam name="T">Type of the items represented by this catalog.</typeparam>
        /// <param name="items">Items that will appear inside this catalog.</param>
        /// <param name="onSelected">What happens when an item is selected.</param>
        /// <param name="onButtonSpawned">What to do with the button and an item when spawning a catalog entry that represents that item.</param>
        public void Setup<T>(IReadOnlyList<T> items, Action<T> onSelected, Action<Button, T> onButtonSpawned)
        {
            this.gameObject.SetActive(true);

            foreach (var item in items)
            {
                var button = Instantiate(this._buttonPrefab, this._layout.transform);
                button.onClick.AddListener(() => onSelected(item));
                onButtonSpawned(button, item);
            }

            // Compute the desired height for the Grid Layout Group, depending on the amount of items shown in the catalog,
            // their size, the spacing and the width of the catalog.
            var layoutRectTransform = this._layout.GetComponent<RectTransform>();
            var cellSize = this._layout.cellSize;
            var cellSpacing = this._layout.spacing;
            var cellsPerRow = (int)(layoutRectTransform.sizeDelta.x / cellSize.x);
            var amountOfRows = items.Count / cellsPerRow + (items.Count % cellsPerRow > 0 ? 1 : 0);
            var targetHeight = amountOfRows * (cellSize.y + cellSpacing.y) - cellSpacing.y;

            layoutRectTransform.sizeDelta = new Vector2(layoutRectTransform.sizeDelta.x, targetHeight);

            this.gameObject.SetActive(false);
        }
    }
}
