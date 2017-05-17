using Oxide.Game.Rust.Cui;

public class TextEditor : ComponentEditor<TextComponent, CuiTextComponent>
{
    public override void Load( CuiTextComponent component )
    {
        GetField( "text" ).SetValue( component.Text );
        GetField( "fontsize" ).SetValue( component.FontSize );
        GetField( "font" ).SetValue( component.FontSize );
        GetField( "align" ).SetValue( (int) component.Align );
        GetField( "color" ).SetValue( component.Color );
        GetField( "fadein" ).SetValue( component.FadeIn );
    }
}
