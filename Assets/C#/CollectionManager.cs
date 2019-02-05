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
    public static Dictionary<string, List<CuiElement>> Functions = new Dictionary<string, List<CuiElement>>();
    private List<CuiElement> m_Current { get { return Functions.ElementAt( m_FunctionIndex ).Value; } }


    private void Awake()
    {
        CuiHelper.RegisterComponent( "LayoutGroup", typeof( CuiLayoutGroupComponent ) );
        CuiHelper.RegisterComponent( "LayoutElement", typeof( CuiLayoutElementComponent ) );
    }

    private void OnPreAspectChange( int aspectIndex )
    {
        Save( );
    }

    private IEnumerator Start()
    {
        yield return new WaitForFixedUpdate();
        LoadFromSave();
        m_Dropdown.onValueChanged.AddListener( functionIndex =>
         {
             if (m_FunctionIndex == functionIndex) return;
             Save( );
             Change( functionIndex );
         } );
    }


    public void Add( string funcName )
    {
        Functions.Add( funcName, new CuiElementContainer() );
        UpdateOptions();
    }

    public void OnAddClick()
    {
        Add( m_InputField.text );
    }

    private void Load( )
    {
        foreach (var element in m_Current)
        {
            CuiManager.Create().Load( element );
        }
    }
    
    private void SaveCurrent( )
    {
        var key = Functions.ElementAt(m_FunctionIndex).Key;
        Functions[key] = GetCurrentCui();
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
    private void Change( int functionIndex )
    {
        m_FunctionIndex = functionIndex;
        UnloadCurrent();
        Load( );
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
        return new CuiElementContainer( HierarchyView.GetCurrent().Where( selected.Contains ).Select( p => p.GetCuiElement() ).ToList() );
    }

    private static void UnloadCurrent()
    {
        var objs = HierarchyView.GetCurrent();
        objs.Reverse();
        foreach (var obj in objs)
        {
            PoolManager.Release( PrefabType.Cui, obj.gameObject );
        }
    }

    public void Save( )
    {
        SaveCurrent( );
        FileHelper.SaveJson( "save", CuiHelper.ToJson( Functions, true ) );
    }
    
    public void Export()
    {
        Save( );
        var production = Functions.ToDictionary( p => p.Key, p => CuiHelper.GetProduction( CuiHelper.ToJson( p.Value ) ) );
        FileHelper.SaveJson( "CuiDB", CuiHelper.ToJson( production ) );
        
        // Save debug information
        foreach (var prod in production)
        {
            FileHelper.SaveJson("Debug/" + prod.Key, JsonPrettify(prod.Value));
            FileHelper.SaveJson("Release/" + prod.Key, prod.Value);
        }
    }
    private void LoadFromSave()
    {
        if (!File.Exists( "save.json" ))
        {
            File.WriteAllText( "save.json", CuiHelper.ToJson( new Dictionary<string, string>()
            {
                {"test", "{}" }
            } ) );
        }
        Functions = CuiHelper.DeserializeObject<Dictionary<string, List<CuiElement>>>( File.ReadAllText( "save.json" ) );

        UpdateOptions();
        Load();
    }
    
    public static string JsonPrettify(string json)
    {
        using (var stringReader = new StringReader(json))
        using (var stringWriter = new StringWriter())
        {
            var jsonReader = new JsonTextReader(stringReader);
            var jsonWriter = new JsonTextWriter(stringWriter) { Formatting = Formatting.Indented };
            jsonWriter.WriteToken(jsonReader);
            return stringWriter.ToString();
        }
    }
}