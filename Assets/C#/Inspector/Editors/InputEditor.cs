using Oxide.Game.Rust.Cui;

public class InputEditor : ComponentEditor<InputComponent, CuiInputFieldComponent>
{
    public override void Load( CuiInputFieldComponent component )
    {
        GetField( "fontsize" ).SetValue( component.FontSize );
        GetField( "font" ).SetValue( component.Font );
        GetField( "align" ).SetValue( (int) component.Align );
        GetField( "color" ).SetValue( component.Color );
        GetField( "charlimit" ).SetValue( component.CharsLimit );
        GetField( "command" ).SetValue( component.Command );
        GetField( "password" ).SetValue( component.IsPassword );
    }
}
