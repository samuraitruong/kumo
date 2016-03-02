$(document).ready(function () {
    /* Apply fancybox to multiple items */
    $("a[isdlg='True']").click(function () {
        //alert('test');
        //return false;
    })
    $("a[isdlg='True']").fancybox({
        'transitionIn': 'elastic',
        'transitionOut': 'elastic',
        'speedIn': 600,
        'speedOut': 200,
        'overlayShow': false,
        onClosed : function () { alert('close') }
    });

});