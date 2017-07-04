function readFile(input) {
    if (input.files && input.files[0]) {

        if (input.files[0].size > 2500000)
        {
            alert('Image have very big size');
            return;
        }
       
        var reader = new FileReader();

        reader.onload = function (e) {

            var boundary = $('.cr-boundary');
            if (boundary != null) {
                var slider = $('.cr-slider-wrap');
                slider.remove();
                boundary.remove();
            }
            crop(e.target.result);
        }
        reader.readAsDataURL(input.files[0]);
    }
}
var crop = function (src) {

    var c = new Croppie(document.getElementById('crop_image'), {
        viewport: { width: 200, height: 200, type: 'square' },
        boundary: { width: 300, height: 300 },
        showZoom: false,
        enableExif: true
    });

    c.bind(src).then(function () {

    });

    $('#form').on('submit', function () {
        c.result('base64', 'viewport').then(function (resp) {
            var input = $('#profile');
            input.attr('value', resp);
            var pos = c.get();
            $('#zoom').val(pos.zoom);
            $('#points').val(pos.points);

        });
    });
}