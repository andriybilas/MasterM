function PlusClick( evt )
{
	var quantity = parseInt( $( "#pl-" + evt ).val() );

	if ( isNaN( quantity ) )
		quantity = 0;

	quantity = quantity + 1;
	if ( quantity < 999 )
		$( "#pl-" + evt ).val( quantity );
}

function MinusClick( evt )
{
	var quantity = parseInt( $( "#pl-" + evt ).val() );
	quantity = quantity - 1;
	if ( quantity >= 0 )
		$( "#pl-" + evt ).val( quantity );
}

function CountControlPlusClick( pid, campaignPrice, price, name )
{
    var quantity = parseInt( $( "#pl-" + pid ).val() );

    if ( isNaN( quantity ) )
        quantity = 0;

    quantity = quantity + 1;
    if ( quantity < 999 )
        $( "#pl-" + pid ).val( quantity );

    AddProductToCart( pid, campaignPrice, price, name, 1 );
}

function CountControlMinusClick( pid, campaignPrice, price, name )
{
    var quantity = parseInt( $( "#pl-" + pid ).val() );
    quantity = quantity - 1;

    if ( quantity > 0 )
    {
        $( "#pl-" + pid ).val( quantity );
        AddProductToCart( pid, campaignPrice, price, name, -1 );
    }

    if ( quantity == 0 )
        DeleteCartItem( pid );
}

function BlockNonNumbers( obj, evt, allowDecimal, allowNegative, decimalPlaces )
{
	var key;
	var isCtrl = false;
	var keychar;
	var reg;

	decimalChar = ",";

	var e = evt || window.event;

	if ( window.event )
	{
		key = e.keyCode;
		isCtrl = window.event.ctrlKey;
	}
	else if ( e.which )
	{
		key = e.which;
		isCtrl = e.ctrlKey;
	}

	if ( isNaN( key ) ) return true;

	keychar = String.fromCharCode( key );

	// check for backspace or delete, or if Ctrl was pressed
	if ( key == 8 || isCtrl )
	{
		return true;
	}

	reg = /\d/;
	var isFirstN = allowNegative ? keychar == '-' && obj.value.indexOf( '-' ) == -1 : false;
	var isFirstD = allowDecimal ? keychar == decimalChar && obj.value.indexOf( decimalChar ) == -1 : false;
	var isMoreThen = obj.value.substring( obj.value.indexOf( decimalChar ) ).length >= decimalPlaces;

	if ( isMoreThen || !( isFirstN || isFirstD || reg.test( keychar ) ) )
	{
		e.preventDefault ? e.preventDefault() : event.returnValue = false;
		return false;
	}

	return isFirstN || isFirstD || reg.test( keychar );
}

function AddProductToCart(pid, campaignPrice, price, name, countToAdd)
{
    if ( $( "#pl-" + pid ).val() == window.StringEmpty )
        $( "#pl-" + pid ).val( "1" );

    var itemsCount = countToAdd;
    if ( countToAdd == null )
    {
        itemsCount = parseInt( $( "#pl-" + pid ).val() );    
    }

    PostAjaxRequest( "html", "/ShoppingCart/AddProductToCart", JSON.stringify( 
		{
			productId: pid,
			campaignPrice: campaignPrice,
			price: price,
			productName: name,
			count: itemsCount 
		}), RebuildMiniCart);
}

function DeleteCartItem ( productId )
{
    PostAjaxRequest( "html", "/ShoppingCart/RemoveCartItem", JSON.stringify( { productId: productId } ), RebuildMiniCart );
    PostAjaxRequest( "html", "/ShoppingCart/UpdateShoppingCartOrderRows", null, RebuildShoopingCart );
}

function RebuildMiniCart( data )
{
    $( "#mini-cart" ).replaceWith( data );
    PostAjaxRequest( "html", "/ShoppingCart/UpdateShoppingCartOrderRows", null, RebuildShoopingCart );
}

function RebuildShoopingCart (data)
{
    if ( $( "#shopping-cart-items" ).length > 0 )
        $( "#shopping-cart-items" ).replaceWith( data );
}

