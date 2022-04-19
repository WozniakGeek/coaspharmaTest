var directionsRenderer;
var autoComplete;
var googleMap;
var marker;
$(document).ready(function () {
    $("#btnAddLocation").click(function () {
        if ($("#txtLocation").val().trim() == "") {
            alert("Por favor ingrese la ubicación.");
            return
        }
        if (marker) {
            marker.setMap(null)
        }
        $("#ddlDestination").append('<option value="' + $("#txtLocation").val() + '">' + $("#txtLocation").val() + "</option>");
        $("#ddlSource").append('<option value="' + $("#txtLocation").val() + '">' + $("#txtLocation").val() + "</option>");
        $("#txtLocation").val("");
        $("#txtLocation").focus();
        $("#successMessage").show();
        setTimeout(function () {
            $("#successMessage").hide()
        }, 5000)
    });
    $("#chkDestinationIsSameAsSource").change(function () {
        $("#ddlDestination").attr("disabled", $("#chkDestinationIsSameAsSource").is(":checked"));
        if ($("#chkDestinationIsSameAsSource").is(":checked")) {
            $("#ddlDestination").val($("#ddlSource").val())
        }
    });
    $("#ddlSource").change(function () {
        if ($("#chkDestinationIsSameAsSource").is(":checked")) {
            $("#ddlDestination").val($("#ddlSource").val())
        }
    });
    $("#btnDisplayDirections").click(function () {
        if ($("#ddlSource").val() == "") {
            alert("Seleccione Punto De partida.");
            $("#ddlSource").focus();
            return
        }
        if ($("#ddlDestination").val() == "") {
            alert("Seleccione Destino.");
            $("#ddlDestination").focus();
            return
        }
        if ($("#ddlTravelMode").val() == "") {
            alert("Seleccione el modo de viaje.");
            $("#ddlTravelMode").focus();
            return
        }
        if (directionsRenderer) {
            directionsRenderer.setMap(null)
        }
        $("#panel").html("");
        var c = new google.maps.DirectionsService;
        directionsRenderer = new google.maps.DirectionsRenderer({
            map: googleMap,
            panel: document.getElementById("panel"),
            draggable: true
        });
        var b = [];
        $("#ddlSource > option").each(function () {
            if ($(this).val() != "" && $(this).val() != $("#ddlSource").val() && $(this).val() != $("#ddlDestination").val()) {
                var d = {
                    location: $(this).val(),
                    stopover: true
                };
                b.push(d)
            }
        });
        var a = {
            origin: $("#ddlSource").val(),
            destination: $("#ddlDestination").val(),
            travelMode: google.maps.TravelMode[$("#ddlTravelMode").val()],
            waypoints: b,
            optimizeWaypoints: $("#chkOptimizePath").is(":checked")
        };
        c.route(a, function (e, d) {
            if (d == google.maps.DirectionsStatus.OK) {
                directionsRenderer.setDirections(e)
            } else {
                alert("No pudimos encontrar ningún resultado para tu búsqueda.")
            }
        })
    })
});

function initializeMap() {
    autoComplete = new google.maps.places.Autocomplete(document.getElementById("txtLocation"));
    autoComplete.addListener("place_changed", function () {
        var c = autoComplete.getPlace();
        googleMap.setCenter(c.geometry.location);
        googleMap.setZoom(15);
        if (marker) {
            marker.setMap(null)
        }
        marker = new google.maps.Marker({
            position: c.geometry.location,
            map: googleMap,
            title: c.formatted_address
        })
    });
    document.getElementById("btnClearDirections").addEventListener("click", function () {
        if (confirm("¿Seguro que quieres borrar todas las ubicaciones?")) {
            if (directionsRenderer) {
                directionsRenderer.setMap(null)
            }
            $("#panel").html("");
            $("#frmLocation").get(0).reset();
            $("#ddlSource").html('<option value="">---Punto de partida---</option>');
            $("#ddlDestination").html('<option value="">---Destino---</option>');
            if (marker) {
                marker.setMap(null)
            }
        }
    });
    var b = new google.maps.LatLng(4.6871549, -74.1294176);
    var a = {
        center: b,
        zoom: 12
    };
    googleMap = new google.maps.Map(document.getElementById("googleMap"), a);
    google.maps.event.addListener(googleMap, "click", function (d) {
        if (marker) {
            marker.setMap(null)
        }
        var c = new google.maps.Geocoder();
        c.geocode({
            latLng: d.latLng
        }, function (g, f) {
            if (f == google.maps.GeocoderStatus.OK) {
                if (g[0]) {
                    var e = g[0].formatted_address;
                    marker = new google.maps.Marker({
                        position: d.latLng,
                        map: googleMap,
                        title: e
                    });
                    $("#txtLocation").val(e)
                }
            }
        })
    })
}
google.maps.event.addDomListener(window, "load", initializeMap);