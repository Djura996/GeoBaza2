$(document).ready(function () {
   
        $.ajax({

        type: "GET",

        url: "Home/GetJsonLocation", 

       success: loadMaps

   });
 

       
});

function getLocationsByCategory(category) {

    $.ajax({

        type: "GET",

        url: "Home/GetJsonLocationByCategory",
        data: { category: category }, 

        success: loadMaps

    });


}



function loadMaps(data) {

    // uklonili smo prethodne layere mape da bi ucitali postojeci.
    $("#map").empty();
    var lokacije = data;
   
     
    var lokacijeStyle = new ol.style.Style({
        image: new ol.style.RegularShape({
            fill: new ol.style.Fill({ color: 'blue' }),
            stroke: new ol.style.Stroke({ color: 'black', width: 1 }),
            points: 4,
            radius: 10,
            angle: Math.PI / 4
        }),
        text: new ol.style.Text({
            font: 'bold 11px "Open Sans", "Arial Unicode MS", "sans-serif"',
            placement: 'point',
            fill: new ol.style.Fill({
                color: 'black'
            })
        })
    });

    var lokacijeSelectStyle = new ol.style.Style({
        image: new ol.style.RegularShape({
            fill: new ol.style.Fill({ color: 'red' }),
            stroke: new ol.style.Stroke({ color: 'black', width: 1 }),
            points: 4,
            radius: 10,
            angle: Math.PI / 4
        }),
        text: new ol.style.Text({
            font: 'bold 11px "Open Sans", "Arial Unicode MS", "sans-serif"',
            placement: 'point',
            fill: new ol.style.Fill({
                color: 'black'
            })
        })
    });

    var lokacijeSource = new ol.source.Vector({
        features: (new ol.format.GeoJSON({
            dataProjection: 'EPSG:4326',
            featureProjection: 'EPSG:4326'
        })).readFeatures(lokacije)
    });

    var lokacijeLayer = new ol.layer.Vector({
        id: 'lokacije',
        source: lokacijeSource,
        style: function (feature) {
            lokacijeStyle.getText().setText(feature.get('name'));
            return lokacijeStyle;
        }
    });

    var selectFeature = new ol.interaction.Select({
        condition: ol.events.condition.singleClick,
        toggleCondition: ol.events.condition.shiftKeyOnly,
        layers: function (layer) {
            return layer.get('id') == 'lokacije';
        },
        style: lokacijeSelectStyle
    });


    var map = new ol.Map({
        target: 'map',
        layers: [
            new ol.layer.Tile({
                source: new ol.source.OSM()
            }),
            lokacijeLayer
        ],
        view: new ol.View({
            projection: 'EPSG:4326',
            center: [19.83694, 45.25167],
            zoom: 15
        }),
        
    });

    // ovde je ponasanje kad kliknemo na postojeci feature da se prikaze forma i da se podaci upucaju u text polja.
    map.getInteractions().extend([selectFeature]);

    selectFeature.on('select', function (e) {
        
        $("#info").removeAttr("hidden");
        $("#features_properties_name").val(e.selected[0].values_.name);
        $("#features_properties_fclass").val(e.selected[0].values_.fclass);
        $("#features_properties_address").val(e.selected[0].values_.address);
        $("#features_properties_gid").val(e.selected[0].values_.gid);
        $("#features_geometry_coordinates_0_").val(e.selected[0].values_.geometry.flatCoordinates[0]);
        $("#features_geometry_coordinates_1_").val(e.selected[0].values_.geometry.flatCoordinates[1]);
        $("#features_geometry_type").val("Point");

    });

    // ovo je kad kliknemo na mapu bilo gde
    map.on("click", function (ev) {

        var latLong = ol.proj.transform(ev.coordinate, 'EPSG:4326', 'EPSG:4326');
       
        $("#info").removeAttr("hidden");
        $("#features_geometry_coordinates_0_").val(latLong[0]);
        $("#features_geometry_coordinates_1_").val(latLong[1]);
        $("#features_geometry_type").val("Point");
        $("#features_properties_name").val("");
        $("#features_properties_fclass").val("");
        $("#features_properties_address").val("");
        $("#features_properties_gid").val("");
    })

  
}

