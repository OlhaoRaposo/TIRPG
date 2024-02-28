using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;

[Serializable] 
public abstract class Module
    {
#if UNITY_EDITOR
        [SerializeField, HideInInspector] public bool editorOpen = true;
#endif
        public bool enabled
        {
            get => _enabled;
            set
            {
                _enabled = value;

#if UNITY_EDITOR
                if (Application.isPlaying)
                {
                    if (value)
                        OnEnable();
                    else
                        OnDisable();
                }
#else
            if(value)
                    OnEnable();
                else
                    OnDisable();
#endif
            }
        }

        [SerializeField, HideInInspector] private bool _enabled = true;

        public void SetEnabled(bool value) => this.enabled = value;

        public virtual void OnEnable()
        {
        }

        public virtual void OnDisable()
        {
        }
    }

    public interface IModuleSet
    {
        public IEnumerable<Type> GetModuleTypes();

        public TM AddModule<TM>() where TM : Module;
        public bool AddModule(Module module);
        public void RemoveModuleAt(int index);
        public Module GetModuleAt(int index);

        public void SwapModuleOrder(int a, int b);
        public int GetModuleCount();
    }

    [Serializable]
    public class ModuleSet<T> : IEnumerable, IModuleSet where T : Module
    {
#if UNITY_EDITOR
        [SerializeField, HideInInspector] public bool editorOpen = true;
#endif

        private Action<T> _onAddModule;
        private Action<T> _onRemoveModule;

        [SerializeReference] private T[] _modules = Array.Empty<T>();

        public IEnumerable<Type> GetModuleTypes()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(domainAssembly => domainAssembly.GetTypes())
                .Where(type =>
                    typeof(T).IsAssignableFrom(type)
                    && !type.IsAbstract
                    && type.IsSerializable
                );
        }

        public TM AddModule<TM>() where TM : Module
        {
            if (!typeof(T).IsAssignableFrom(typeof(TM))) return default(TM);
            var module = Activator.CreateInstance<TM>();
            return AddModule(module) ? module : null;
        }

        public bool AddModule(Module module)
        {
            if (!(module is T)) return false;

            Array.Resize(ref _modules, _modules.Length + 1);
            _modules[^1] = (T)module;
            _onAddModule?.Invoke((T)module);

            return true;
        }

        public TM GetModule<TM>(bool enabledOnly = false)
        {
            if (enabledOnly)
            {
                foreach (var m in _modules)
                    if (m is TM tm && m.enabled)
                        return tm;
            }
            else
            {
                foreach (var m in _modules)
                    if (m is TM tm)
                        return tm;
            }

            return default(TM);
        }

        public Module GetModuleAt(int index)
        {
            return _modules[index];
        }

        public IEnumerable<TM> GetModules<TM>() where TM : T
        {
            foreach (var m in _modules)
                if (m is TM)
                    yield return (TM)m;
        }

        public bool RemoveModule<TM>(TM module) where TM : T
        {
            var i = Array.IndexOf(_modules, module);
            if (i == -1) return false;
            RemoveModuleAt(i);
            return true;
        }

        public void RemoveModuleAt(int index)
        {
            var module = _modules[index];

            for (int j = index + 1; j < _modules.Length; j++)
                _modules[j - 1] = _modules[j];

            _onRemoveModule?.Invoke((T)module);

            Array.Resize(ref _modules, _modules.Length - 1);
        }

        public void InitModuleCallbacks(Action<T> onAdd, Action<T> onRemove)
        {
            this._onAddModule = onAdd;
            this._onRemoveModule = onRemove;
        }

        public IEnumerator GetEnumerator() => _modules.GetEnumerator();
        public int GetModuleCount() => _modules.Length;

        public void SwapModuleOrder(int a, int b)
        {
            (_modules[a], _modules[b]) = (_modules[b], _modules[a]);
        }
    }

[Serializable]
    public abstract class ModularBehaviour<T> : MonoBehaviour, IEnumerable where T : Module
    {
        [SerializeField] private ModuleSet<T> _modules = new ModuleSet<T>();

        #region Methods

        public TM AddModule<TM>() where TM : T => _modules.AddModule<TM>();
        public TM GetModule<TM>(bool enabledOnly = false) => _modules.GetModule<TM>(enabledOnly);
        public T GetModuleAt(int index) => (T)_modules.GetModuleAt(index);
        public IEnumerable<TM> GetModules<TM>() where TM : T => _modules.GetModules<TM>();
        public bool RemoveModule(T module) => _modules.RemoveModule(module);
        public IEnumerator GetEnumerator() => _modules.GetEnumerator();
        public int GetModuleCount() => _modules.GetModuleCount();
        public TM GetOrAddModule<TM>() where TM : T => GetModule<TM>() ?? AddModule<TM>();

        #endregion

        #region Callbacks

        protected virtual void LoadModuleSetCallbacks()
        {
            _modules.InitModuleCallbacks(OnAddModule, OnRemoveModule);
        }

        public virtual void OnValidate()
        {
            LoadModuleSetCallbacks();
        }

        public virtual void Reset()
        {
            LoadModuleSetCallbacks();
        }

        public virtual void OnEnable()
        {
            foreach (T module in _modules)
                if (module.enabled)
                    module.OnEnable();
        }

        public virtual void OnDisable()
        {
            foreach (T module in _modules)
                if (module.enabled)
                    module.OnDisable();
        }

        public virtual void OnAddModule(T module)
        {
        }

        public virtual void OnRemoveModule(T module)
        {
        }

        #endregion
    }

[Serializable]
public abstract class ModularNoBehaviour<T> : IEnumerable where T : Module
{
    [SerializeField] private ModuleSet<T> _modules = new ModuleSet<T>();
    protected internal int ModuleCount => _modules.GetModuleCount();
    
    #region Methods

    public TM AddModule<TM>() where TM : T => _modules.AddModule<TM>();
    public TM GetModule<TM>(bool enabledOnly = false) => _modules.GetModule<TM>(enabledOnly);
    public T GetModuleAt(int index) => (T)_modules.GetModuleAt(index);
    public IEnumerable<TM> GetModules<TM>() where TM : T => _modules.GetModules<TM>();
    public bool RemoveModule(T module) => _modules.RemoveModule(module);
    public IEnumerator GetEnumerator() => _modules.GetEnumerator();
    public int GetModuleCount() => _modules.GetModuleCount();
    public TM GetOrAddModule<TM>() where TM : T => GetModule<TM>() ?? AddModule<TM>();

    #endregion

    #region Callbacks

    protected virtual void LoadModuleSetCallbacks()
    {
        _modules.InitModuleCallbacks(OnAddModule, OnRemoveModule);
    }

    public virtual void OnValidate()
    {
        LoadModuleSetCallbacks();
    }

    public virtual void Reset()
    {
        LoadModuleSetCallbacks();
    }

    public virtual void OnEnable()
    {
        foreach (T module in _modules)
            if (module.enabled)
                module.OnEnable();
    }

    public virtual void OnDisable()
    {
        foreach (T module in _modules)
            if (module.enabled)
                module.OnDisable();
    }

    public virtual void OnAddModule(T module)
    {
    }

    public virtual void OnRemoveModule(T module)
    {
    }

    #endregion
}

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(ModuleSet<>), true)]
    public class ModuleSetDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            GUILayout.Space(-20);

            var boxStyle = new GUIStyle(EditorStyles.helpBox);
            boxStyle.normal.background = MakeTex(8, 8, new Color(.25f, 0.25f, 0.25f, 1f), new Color(0, 0, 0, 0.5f));

            GUILayout.BeginVertical(boxStyle);

            //position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            SerializedProperty items = property.FindPropertyRelative("_modules");
            SerializedProperty editorOpen = property.FindPropertyRelative("editorOpen");

            int length = items?.arraySize ?? 0;

            editorOpen.boolValue = EditorGUILayout.BeginFoldoutHeaderGroup(editorOpen.boolValue, label);
            EditorGUILayout.EndFoldoutHeaderGroup();

            List<Module> toEnable = new List<Module>();
            List<Module> toDisable = new List<Module>();

            if (editorOpen.boolValue)
            {
                for (var i = 0; i < length; i++)
                {
                    var item = items!.GetArrayElementAtIndex(i);
                    var module = ((Module)item.managedReferenceValue);

                    var itemEnabled = item.FindPropertyRelative("_enabled");
                    var itemEditorOpen = item.FindPropertyRelative("editorOpen");

                    var moduleBoxStyle = new GUIStyle(EditorStyles.helpBox);
                    moduleBoxStyle.normal.background = MakeTex(8, 8, new Color(.15f, 0.15f, 0.15f, 1f), new Color(0, 0, 0, 0.5f));

                    GUILayout.BeginVertical(moduleBoxStyle);

                    #region Header

                    var isOpen =
                        EditorGUILayout.BeginFoldoutHeaderGroup(itemEditorOpen.boolValue, GUIContent.none, null,
                            (rect) =>
                            {
                                int index = i;
                                ShowDropdown(rect, property, index, length);
                            });
                    EditorGUILayout.EndFoldoutHeaderGroup();

                    Rect headerRect = GUILayoutUtility.GetLastRect();
                    headerRect.x += 20;
                    GUI.Label(headerRect, EditorGUIUtility.FindTexture("cs Script Icon"));

                    Rect rectToggle = new Rect(headerRect.x + 20, headerRect.y, 20, headerRect.height);
                    bool newValue = EditorGUI.Toggle(rectToggle, GUIContent.none, itemEnabled.boolValue);

                    Rect rectLabel = new Rect(headerRect.x + 38, headerRect.y - 1, headerRect.width - 60,
                        headerRect.height);
                    GUI.Label(rectLabel, item.managedReferenceValue.GetType().Name, EditorStyles.boldLabel);

                    if (isOpen != itemEditorOpen.boolValue)
                    {
                        Vector2 mousePosition = Event.current.mousePosition;
                        if (rectToggle.x <= mousePosition.x && rectToggle.x + rectToggle.width >= mousePosition.x)
                        {
                            Undo.RecordObject(property.serializedObject.targetObject, "Toggle Module Enabled");


                            if (!itemEnabled.boolValue) toEnable.Add(module);
                            else toDisable.Add(module);
                        }
                        else
                        {
                            itemEditorOpen.boolValue = isOpen;
                        }
                    }

                    #endregion

                    #region Contents

                    if (itemEditorOpen.boolValue)
                    {
                        var enumerator = item.GetEnumerator();
                        int dots = item.propertyPath.Count(c => c == '.');
                        while (enumerator.MoveNext())
                        {
                            var prop = enumerator.Current as SerializedProperty;
                            if (prop == null) continue;
                            if (prop.propertyPath.Count(c => c == '.') != dots + 1) continue;
                            EditorGUILayout.PropertyField(prop);
                        }
                    }

                    #endregion

                    GUILayout.EndVertical();
                    GUILayout.Space(5);
                }

                using var hScope = new EditorGUILayout.HorizontalScope();

                if (GUILayout.Button(EditorGUIUtility.TrTextContent("Add Module"), EditorStyles.miniButton))
                {
                    var r = hScope.rect;
                    var pos = new Vector2(r.x + r.width / 2f, r.yMax + 18f);
                    FilterWindow.Show(pos,
                        new ModuleSetProvider((IModuleSet)property.GetUnderlyingValue(), property.serializedObject));
                }
            }

            GUILayout.EndVertical();
            EditorGUI.EndProperty();

            foreach (Module module in toEnable)
                module.enabled = true;

            foreach (Module module in toDisable)
                module.enabled = false;
        }

        public void ShowDropdown(Rect position, SerializedProperty property, int index, int length)
        {
            var menu = new GenericMenu();
            menu.AddItem(new GUIContent("Remove"), false, () =>
            {
                Undo.RecordObject(property.serializedObject.targetObject, "Remove Module");

                ((IModuleSet)property.GetUnderlyingValue()).RemoveModuleAt(index);
            });

            if (index == 0)
                menu.AddDisabledItem(new GUIContent("Move Up"));
            else
                menu.AddItem(new GUIContent("Move Up"), false, () =>
                {
                    Undo.RecordObject(property.serializedObject.targetObject, "Swap Module Order");

                    ((IModuleSet)property.GetUnderlyingValue()).SwapModuleOrder(index, index - 1);
                });

            if (index == length - 1)
                menu.AddDisabledItem(new GUIContent("Move Down"));
            else
                menu.AddItem(new GUIContent("Move Down"), false, () =>
                {
                    Undo.RecordObject(property.serializedObject.targetObject, "Swap Module Order");

                    ((IModuleSet)property.GetUnderlyingValue()).SwapModuleOrder(index, index + 1);
                });

            menu.DropDown(position);
        }

        private Texture2D MakeTex(int width, int height, Color color, Color border)
        {
            Color[] pix = new Color[width * height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                        pix[y * width + x] = border;
                    else
                        pix[y * width + x] = color;
                }
            }

            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();
            return result;
        }
    }

    public class ModuleSetProvider : FilterWindow.IProvider
    {
        class ModuleSetElement : FilterWindow.Element
        {
            public Type type;

            public ModuleSetElement(int level, string label, Type type)
            {
                this.level = level;
                this.type = type;

                content = new GUIContent(label);
            }
        }

        class PathNode : IComparable<PathNode>
        {
            public List<PathNode> nodes = new List<PathNode>();
            public string name;
            public Type type;

            public int CompareTo(PathNode other)
            {
                return name.CompareTo(other.name);
            }
        }

        public Vector2 position { get; set; }

        private IModuleSet _moduleSet;
        private SerializedObject _serializedObject;


        public void CreateComponentTree(List<FilterWindow.Element> tree)
        {
            tree.Add(new FilterWindow.GroupElement(0, "Module List"));

            var rootNode = new PathNode();

            foreach (Type t in _moduleSet.GetModuleTypes())
                AddNode(rootNode, t.Name, t);

            // Recursively add all elements to the tree
            Traverse(rootNode, 1, tree);
        }

        public ModuleSetProvider(IModuleSet moduleSet, SerializedObject serializedObject)
        {
            this._moduleSet = moduleSet;
            this._serializedObject = serializedObject;
        }

        public bool GoToChild(FilterWindow.Element element, bool addIfComponent)
        {
            if (element is ModuleSetElement)
            {
                ModuleSetElement e = element as ModuleSetElement;

                Undo.RecordObject(_serializedObject.targetObject, "Add Module");

                _moduleSet.AddModule((Module)Activator.CreateInstance(e.type));

                return true;
            }

            return false;
        }

        void AddNode(PathNode root, string path, Type type)
        {
            var current = root;
            var parts = path.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var part in parts)
            {
                var child = current.nodes.Find(x => x.name == part);

                if (child == null)
                {
                    child = new PathNode { name = part, type = type };
                    current.nodes.Add(child);
                }

                current = child;
            }
        }

        void Traverse(PathNode node, int depth, List<FilterWindow.Element> tree)
        {
            node.nodes.Sort();

            foreach (var n in node.nodes)
            {
                if (n.nodes.Count > 0) // Group
                {
                    tree.Add(new FilterWindow.GroupElement(depth, n.name));
                    Traverse(n, depth + 1, tree);
                }
                else // Element
                {
                    tree.Add(new ModuleSetElement(depth, n.name, n.type));
                }
            }
        }
    }
#endif
