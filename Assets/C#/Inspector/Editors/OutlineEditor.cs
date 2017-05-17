using Oxide.Game.Rust.Cui;

public class OutlineEditor : ComponentEditor<OutlineComponent, CuiOutlineComponent>
{
    public override void Load( CuiOutlineComponent component )
    {
        GetField( "distance" ).SetValue( component.Distance );
        GetField( "usegraphicalpha" ).SetValue( component.UseGraphicAlpha );
        GetField( "color" ).SetValue( component.Color );
    }
}
