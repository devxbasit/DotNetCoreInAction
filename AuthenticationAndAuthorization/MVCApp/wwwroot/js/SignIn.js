$(document).ready(function(){

    $('#signInForm').submit(function(e) {
        //e.preventDefault();
        let data = $('#signInForm').serialize();
        signIn(JSON.stringify(data));
    })
})


function signIn(data){

}

