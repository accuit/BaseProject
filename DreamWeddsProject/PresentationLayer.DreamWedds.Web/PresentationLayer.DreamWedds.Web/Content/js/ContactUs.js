(function ($) {
    $.fn.textAreaLimit = function (limit, element) {
        return this.each(function () {
            var $this = $(this);
            var displayCharactersLeft = function (charactersLeft) {
                if (element) {
                    $(element).html((charactersLeft <= 0) ? '0' : charactersLeft);
                }
            };

            $this.bind('focus keypress blur click', function () {
                var val = $this.val();
                var length = val.length;
                if (length > limit) {
                    $this.val($this.val().substring(0, limit));
                }
                displayCharactersLeft(limit - length);
            });

            displayCharactersLeft(limit - $this.val().length);
        });
    };
});

function countChar(val) {
    var len = val.value.length;
    if (len >= 500) {
        val.value = val.value.substring(0, 500);
    } else {
        $('#charNum').text(500 - len);
    }
};



$(document).ready(function () {
   
});
