using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Oxide.Game.Rust.Cui;
using UnityEngine;
using UnityEngine.UI;

public class CollectionManager : MonoBehaviour
{
    [SerializeField] private Dropdown m_Dropdown;
    [SerializeField] private InputField m_InputField;

    private int m_FunctionIndex = 0;
    public static Dictionary<string, Cui> Functions = new Dictionary<string, Cui>();
    private Cui m_Current { get { return Functions.ElementAt( m_FunctionIndex ).Value; } }


    private void Awake()
    {
        AspectManager.OnChanged += OnAspectChanged;
        AspectManager.OnPreChange += OnPreAspectChange;
    }

    private void OnPreAspectChange( int aspectIndex )
    {
        Save(aspectIndex);
    }

    private IEnumerator Start()
    {
        yield return new WaitForFixedUpdate();
        LoadFromSave();
        m_Dropdown.onValueChanged.AddListener(functionIndex =>
        {
            if (m_FunctionIndex == functionIndex) return;
            Save( AspectManager.AspectIndex );
            Change(functionIndex, AspectManager.AspectIndex);
        });
    }


    public void Add( string funcName )
    {
        Functions.Add( funcName, Cui.Default );
        UpdateOptions();
    }

    private void OnAspectChanged( int aspectIndex )
    {
        Change( m_FunctionIndex, aspectIndex );
    }

    public void OnAddClick()
    {
        Add( m_InputField.text );
    }

    private void Load( int aspect = -1 )
    {
        if (aspect == -1)
        {
            aspect = AspectManager.AspectIndex;
        }
        foreach (var element in CuiHelper.FromJson( m_Current.Get( aspect ) ))
        {
            CuiManager.Create().Load( element );
        }
    }
    private void SaveCurrent(int aspectIndex)
    {
        m_Current.Set( aspectIndex, GetCurrentCui().ToJson() );
    }
    public void RemoveCurrent()
    {
        if (Functions.Count == 1)
        {
            return;
        }
        UnloadCurrent();
        Functions.Remove( Functions.ElementAt( m_FunctionIndex ).Key );
        if (m_FunctionIndex == Functions.Count)
        {
            m_FunctionIndex--;
        }
        UpdateOptions();
        Load();
    }
    private void Change( int functionIndex, int aspectIndex )
    {
        m_FunctionIndex = functionIndex;
        UnloadCurrent();
        Load( aspectIndex );
    }

    private void UpdateOptions()
    {
        m_Dropdown.options = Functions.Keys.Select( p => new Dropdown.OptionData( p ) ).ToList();
        m_Dropdown.value = m_FunctionIndex;
    }

    public static CuiElementContainer GetCurrentCui()
    {
        return new CuiElementContainer( HierarchyView.GetCurrent().Select( p => p.GetCuiElement() ).ToList() );
    }
    public static CuiElementContainer GetSelectedCui()
    {
        var selected = CUIObject.Selection;
        return new CuiElementContainer( HierarchyView.GetCurrent().Where(selected.Contains).Select( p => p.GetCuiElement() ).ToList() );
    }

    private static void UnloadCurrent()
    {
        var objs = HierarchyView.GetCurrent();
        objs.Reverse();
        foreach (var obj in objs)
        {
            PoolManager.Release( obj.gameObject );
        }
    }

    public void Save(int aspectIndex)
    {
        SaveCurrent( aspectIndex );
        FileHelper.Save( "save.json", CuiHelper.ToJson( Functions ) );
    }
    public void Export()
    {
        Save(AspectManager.AspectIndex);
        var production = Functions.ToDictionary( p => p.Key, p => p.Value.GetProduction() );
        FileHelper.Save( "save.json", CuiHelper.ToJson( production ) );
    }
    private void LoadFromSave()
    {
        if (!File.Exists( "save.json" ))
        {
            File.WriteAllText( "save.json", CuiHelper.ToJson( new Dictionary<string, Cui>()
            {
                {"test", Cui.Default }
            } ) );
        }
        Functions = CuiHelper.DeserializeObject<Dictionary<string, Cui>>( File.ReadAllText( "save.json" ) );

        UpdateOptions();
        Load();
    }
}

[Serializable]
public class Cui
{
    [JsonProperty( "16x9" )] public string _16x9 = "";
    [JsonProperty( "16x10" )] public string _16x10 = "";
    [JsonProperty( "5x4" )] public string _5x4 = "";
    [JsonProperty( "4x3" )] public string _4x3 = "";

    public string Get( int aspect )
    {
        switch (aspect)
        {
            case 0:
                return _16x9;
            case 1:
                return _16x10;
            case 2:
                return _5x4;
            case 3:
                return _4x3;
        }
        return "";
    }

    public void Set( int aspect, string json )
    {
        switch (aspect)
        {
            case 0:
                _16x9 = json;
                break;
            case 1:
                _16x10 = json;
                break;
            case 2:
                _5x4 = json;
                break;
            case 3:
                _4x3 = json;
                break;
        }
    }

    public static Cui Default { get { return new Cui() { _16x9 = "[]", _16x10 = "[]", _5x4 = "[]", _4x3 = "[]" }; } }

    public Cui GetProduction()
    {
        return new Cui()
        {
            _16x9 = CuiHelper.GetProduction( _16x9 ),
            _16x10 = CuiHelper.GetProduction( _16x10 ),
            _5x4 = CuiHelper.GetProduction( _5x4 ),
            _4x3 = CuiHelper.GetProduction( _4x3 ),
        };
    }
}