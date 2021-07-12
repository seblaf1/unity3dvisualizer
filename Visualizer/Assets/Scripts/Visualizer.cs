using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.Gestures;
using Assets.Scripts.ModeHandlers;
using Assets.Scripts.Extensions;
using Assets.Scripts.Types;

public class Visualizer : MonoBehaviour
{
    [SerializeField] private Button _cameraModeButton;
    [SerializeField] private Button _objectModeButton;
    [SerializeField] private Button _addObjectButton;
    [SerializeField] private Button _textureObjectButton;
    [SerializeField] private Button _deleteObjectButton;
    [SerializeField] private Catalog _modelsCatalog;
    [SerializeField] private Catalog _texturesCatalog;
    [SerializeField] private EventSystem _eventSystem;
    [SerializeField] private Camera _camera;
    [SerializeField] private List<GameObject> _models;
    [SerializeField] private List<Texture> _textures;
    [SerializeField, Range(1f, 100f)] private float _defaultSensitivity = 10f;

    private ModeHandler ActiveHandler { get; set; }

    private Observable<GameObject> ObjectInPreview { get; } = new Observable<GameObject>(null);

    // TODO: Allow player to modify sensibility in settings.
    private Reference<float> Sensitivity { get; } = new Reference<float>(0f);

    private ModeHandler _previouslyActiveHandler;
    private CameraModeHandler _cameraModeHandler;
    private ObjectModeHandler _objectModeHandler;

    private void Awake()
    {
        Assert.IsNotNull(this._cameraModeButton, $"{this.GetType().Name} had no reference to a Camera Mode Button, but it requires one.");
        Assert.IsNotNull(this._objectModeButton, $"{this.GetType().Name} had no reference to a Object Mode Button, but it requires one.");
        Assert.IsNotNull(this._addObjectButton, $"{this.GetType().Name} had no reference to an Add Object Button, but it requires one.");
        Assert.IsNotNull(this._textureObjectButton, $"{this.GetType().Name} had no reference to an Texture Object Button, but it requires one.");
        Assert.IsNotNull(this._deleteObjectButton, $"{this.GetType().Name} had no reference to a Delete Object Button, but it requires one.");
        Assert.IsNotNull(this._modelsCatalog, $"{this.GetType().Name} had no reference to the Models Catalog, but it requires it.");
        Assert.IsNotNull(this._texturesCatalog, $"{this.GetType().Name} had no reference to the Textures Catalog, but it requires it.");
        Assert.IsNotNull(this._eventSystem, $"{this.GetType().Name} had no reference to the Event System, but it requires it.");
        Assert.IsNotNull(this._camera, $"{this.GetType().Name} had no reference to the Main Camera, but it requires it.");

        // Set sentitivity to default value.
        this.Sensitivity.Value = this._defaultSensitivity;

        this._cameraModeHandler = new CameraModeHandler(this._camera, this.Sensitivity);
        this._objectModeHandler = new ObjectModeHandler(this._camera, this.ObjectInPreview, this.Sensitivity);

        this._cameraModeButton.onClick.AddListener(() => this.SetMode(this._cameraModeButton, this._cameraModeHandler));
        this._objectModeButton.onClick.AddListener(() => this.SetMode(this._objectModeButton, this._objectModeHandler));
        this._addObjectButton.onClick.AddListener(() => this.SetCatalogOpened(this._modelsCatalog, true));
        this._textureObjectButton.onClick.AddListener(() => this.SetCatalogOpened(this._texturesCatalog, true));
        this._deleteObjectButton.onClick.AddListener(this.DeleteObjectInPreview);

        // By default, we are in camera mode.
        this.SetMode(this._cameraModeButton, this._cameraModeHandler);

        this._modelsCatalog.Setup(
           this._models,
           this.SpawnObject,
           (btn, mod) => btn.GetComponentInChildren<Text>().text = mod.name);

        this._texturesCatalog.Setup(
            this._textures,
            this.SetTexture,
            (btn, tex) => btn.GetComponent<RawImage>().texture = tex);

        // Depending on if there's an object in preview or not, some buttons should be deactivated or activated.
        this.ObjectInPreview.OnValueChanged((oldVal, newVal) =>
        {
            var objSpawned = newVal != null;

            this._objectModeButton.gameObject.SetActive(objSpawned);
            this._deleteObjectButton.gameObject.SetActive(objSpawned);
            this._textureObjectButton.gameObject.SetActive(objSpawned);
            this._addObjectButton.gameObject.SetActive(!objSpawned);
        });

        // Setup those buttons correctly.
        this.ObjectInPreview.Value = null;
    }

    private void FixedUpdate()
    {
        if (this.ActiveHandler == null)
            return;

        var gesture = GetGesture();

        if (gesture != null)
            this.ActiveHandler.Handle(gesture);
    }

    private void SetCatalogOpened(Catalog catalog, bool opened)
    {
        if (opened)
        {
            this._previouslyActiveHandler = this.ActiveHandler;
            this.ActiveHandler = null;
        }
        else
        {
            this.ActiveHandler = this._previouslyActiveHandler;
            this._previouslyActiveHandler = null;
        }

        catalog.gameObject.SetActive(opened);
    }

    private void DeleteObjectInPreview()
    {
        Assert.IsNotNull(this.ObjectInPreview, "Attempted to delete object in preview while there's is no object in preview!");

        Destroy(this.ObjectInPreview.Value);

        this.ObjectInPreview.Value = null;

        // Fallback to camera handler.
        this.SetMode(this._cameraModeButton, this._cameraModeHandler);
    }

    private void SpawnObject(GameObject obj)
    {
        this.SetCatalogOpened(this._modelsCatalog, false);
        this.ObjectInPreview.Value = Instantiate(obj, Vector3.zero, Quaternion.identity);
    }

    private void SetTexture(Texture texture)
    {
        Assert.IsNotNull(this.ObjectInPreview.Value, "Attempted to set the object in preview's texture but there aren't any objects in preview!");

        this.SetCatalogOpened(this._texturesCatalog, false);
        this.ObjectInPreview.Value.GetComponent<Renderer>().material.mainTexture = texture;
    }

    private void SetMode(Button button, ModeHandler handler)
    {
        this.ActiveHandler = handler;
        this._eventSystem.SetSelectedGameObject(button.gameObject, null);

        Debug.Log($"Switched to {handler.GetType().Name.Truncate("ModeHandler".Length)} mode.");
    }

    private Gesture GetGesture()
    {
        var touches = Input.touches;
        if (touches.Length == 0)
            return null;

        if (touches.Length == 1)
            return SingleTouchGesture.FromTouch(touches[0]);

        return MultiTouchGesture.FromTouches(touches);
    }
}
