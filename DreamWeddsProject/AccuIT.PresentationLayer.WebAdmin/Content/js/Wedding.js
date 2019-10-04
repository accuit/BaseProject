var showPopup = false;
var showUpdatePopup = "";
var showErrPopup = false;
var weddingId, eventID, venueID = "";
var filename = "";
var date = new Date();
var today = new Date(date.getFullYear(), date.getMonth(), date.getDate());

$(document).ready(function () {

    showPopup = $('#ShowPopup').val();
    showErrPopup = $('#ShowErrPopup').val();
    showUpdatePopup = $('#ShowUpdatePopup').val();
    weddingId = $('#hdnweddingID').val();
    eventID = $("#hdEventID").val(),
    venueID = $("#hdVenueID").val()
    $("#btnUpdateEvent").click(function (e) {
        var EventId = $(this).data('id');
        EditWeddingEvent(EventId);
    });

    function EditWeddingEvent(EventId) {
        var url = "../Wedding/GetWeddingEventByID?Id=" + EventId;
        $("#ModalTitle").html("Update Student Record");
        console.log("request starts");

        $.ajax({
            type: "GET",
            url: url,
            dataType: "json",
            success: function (data) {
                console.log(data);
                $("#weddingEventID").val(data.weddingEventID);
                $("#weddingID").val(data.WeddingID);
                $("#eventName").val(data.Name);
                $("#eventTitle").val(data.Title);
                $("#eventImageUrl").val(data.ImageUrl);
                $("#eventStartTime").val(data.strStartTime);
                $("#eventEndTime").val(data.strEndTime);
                $('#eventDate').val(data.strEventDate);
                //$('#myModal').modal('toggle');
            }
        })
    };

    $("#uploadEventImage").change(function (e) {
        filename = e.target.files[0].name;
        if (filename == undefined) {
            // $('#errormsg').hide();
            //$("#lblcsvfilename").css('display', 'inline-block');
            $('#progresseventimg').css('display', 'none');
            $("#lbeventlimage").html("Please select image.");
            $('#lbleventimage').css('color', 'red');

            return false;
        }
        else {
            // var filename = xfile.name;
            if (!(/\.(jpg|jpeg|png|gif|JPG|PNG|GIF)$/i).test(filename)) {
                $('#progresseventimg').css('display', 'block');
                $("#lbleventimage").html("File not supported.");
                $('#lbleventimage').css('color', 'red');

                return false;
            }
            else {
                $('#progresseventimg').css('display', 'block');
                $('#lbleventimage').css('color', 'black');
                var data = new FormData();
                $("#lbleventimage").val("uploading...");
                var files = $("#uploadEventImage").get(0).files;
                var bar = $('.progress-bar');

                if (files.length > 0) {
                    data.append("MyImages", files[0]);
                    data.append("imgtype", $(this).data('imgtype'));
                    data.append("imageof", $(this).data('imageof'));
                    data.append("myId", $(this).data('id'));
                    data.append("sequence", $(this).data('sequence'));
                    data.append("eventID", eventID);
                }

                $.ajax({
                    xhr: function () {
                        var xhr = new window.XMLHttpRequest();
                        $("#lbleventimage").text("Uploading Image...")
                        xhr.upload.addEventListener("progress", function (evt) {
                            if (evt.lengthComputable) {
                                var percentComplete = evt.loaded * 2.5 / evt.total;
                                percentComplete = parseInt(percentComplete * 100);
                                console.log(percentComplete);
                                bar.width(percentComplete);
                                if (percentComplete === 100) {

                                }
                            }
                        }, false);

                        return xhr;
                    },
                    // url: "/Wedding/UploadImageFile?imgtype=Event&myId=" + myId + "&sequence=" + sequence + "&eventID=" + eventID,
                    url: "/Wedding/UploadImageFile",
                    type: "POST",
                    processData: false,
                    contentType: false,
                    data: data,
                    success: function (response) {
                        $("#imgEventPreview").attr('src', response);
                        $("#eventimageUrl").val(response);
                        $('#progresseventimg').css('display', 'none');
                        bar.width(0);
                        $("#lbleventimage").html("Image Uploaded");
                        filename = undefined;
                    },
                    error: function (er) {
                        // alert(er);
                    }

                });
            }
        }
    });
    $("#uploadEventBackGroundImage").change(function (e) {
        filename = e.target.files[0].name;
        if (filename == undefined) {
            // $('#errormsg').hide();
            //$("#lblcsvfilename").css('display', 'inline-block');
            $('#progresseventBackGroundImage').css('display', 'none');
            $("#lbeventlBackGroundImage").html("Please select BackGroundImage.");
            $('#lbleventBackGroundImage').css('color', 'red');

            return false;
        }
        else {
            // var filename = xfile.name;
            if (!(/\.(jpg|jpeg|png|gif|JPG|PNG|GIF)$/i).test(filename)) {
                $('#progressEventBackGroundImage').css('display', 'block');
                $("#lbleventBackGroundImage").html("File not supported.");
                $('#lbleventBackGroundImage').css('color', 'red');

                return false;
            }
            else {
                $('#progressEventBackGroundImage').css('display', 'block');
                $('#lbleventBackGroundImage').css('color', 'black');
                var data = new FormData();
                $("#lbleventBackGroundImage").val("uploading...");
                var files = $("#uploadEventBackGroundImage").get(0).files;
                var bar = $('.progress-bar');

                if (files.length > 0) {
                    data.append("MyImages", files[0]);
                    data.append("imgtype", $(this).data('imgtype'));
                    data.append("BackGroundImageof", $(this).data('BackGroundImageof'));
                    data.append("myId", $(this).data('id'));
                    data.append("sequence", $(this).data('sequence'));
                    data.append("eventID", eventID);
                }

                $.ajax({
                    xhr: function () {
                        var xhr = new window.XMLHttpRequest();
                        $("#lbleventBackGroundImage").text("Uploading BackGroundImage...")
                        xhr.upload.addEventListener("progress", function (evt) {
                            if (evt.lengthComputable) {
                                var percentComplete = evt.loaded * 2.5 / evt.total;
                                percentComplete = parseInt(percentComplete * 100);
                                console.log(percentComplete);
                                bar.width(percentComplete);
                                if (percentComplete === 100) {

                                }
                            }
                        }, false);

                        return xhr;
                    },
                    // url: "/Wedding/UploadBackGroundImageFile?imgtype=Event&myId=" + myId + "&sequence=" + sequence + "&eventID=" + eventID,
                    url: "/Wedding/UploadImageFile",
                    type: "POST",
                    processData: false,
                    contentType: false,
                    data: data,
                    success: function (response) {
                        $("#BackGroundImageEventPreview").attr('src', response);
                        $("#eventBackGroundImageUrl").val(response);
                        $('#progressEventBackGroundImage').css('display', 'none');
                        bar.width(0);
                        $("#lbleventBackGroundImage").html("BackGroundImage Uploaded");
                        filename = undefined;
                    },
                    error: function (er) {
                        // alert(er);
                    }

                });
            }
        }
    });
    $("#uploadVenueImage").change(function (e) {
        filename = e.target.files[0].name;
        if (filename == undefined) {
            // $('#errormsg').hide();
            //$("#lblcsvfilename").css('display', 'inline-block');
            $('#progressVenueimg').css('display', 'none');
            $("#lblVenueimage").html("Please select image.");
            $('#lblVenueimage').css('color', 'red');

            return false;
        }
        else {
            // var filename = xfile.name;
            if (!(/\.(jpg|jpeg|png|gif|JPG|PNG|GIF)$/i).test(filename)) {
                $('#progressVenueimg').css('display', 'none');
                $("#lblVenueimage").html("File not supported.");
                $('#lblVenueimage').css('color', 'red');

                return false;
            }
            else {
                $('#progressVenueimg').css('display', 'block');
                $('#lblVenueimage').css('color', 'black');
                var data = new FormData();
                $("#lblVenueimage").val("uploading...");
                var files = $("#uploadVenueImage").get(0).files;
                var bar = $('.progress-bar');

                if (files.length > 0) {
                    data.append("MyImages", files[0]);
                    data.append("imgtype", $(this).data('imgtype'));
                    data.append("imageof", $(this).data('imageof'));
                    data.append("myId", $(this).data('id'));
                    data.append("sequence", $(this).data('sequence'));
                    data.append("eventID", eventID);
                }

                $.ajax({
                    xhr: function () {
                        var xhr = new window.XMLHttpRequest();
                        $("#lblVenueimage").text("Uploading Image...")
                        xhr.upload.addEventListener("progress", function (evt) {
                            if (evt.lengthComputable) {
                                var percentComplete = evt.loaded * 2.5 / evt.total;
                                percentComplete = parseInt(percentComplete * 100);
                                console.log(percentComplete);
                                bar.width(percentComplete);
                                if (percentComplete === 100) {

                                }
                            }
                        }, false);

                        return xhr;
                    },
                    url: "/Wedding/UploadImageFile",
                    type: "POST",
                    processData: false,
                    contentType: false,
                    data: data,
                    success: function (response) {
                        $("#imgVenuePreview").attr('src', response);
                        $("#venueimageUrl").val(response);
                        $('#progressVenueimg').css('display', 'none');
                        bar.width(0);
                        $("#lblVenueimage").html("Image Uploaded");
                        filename = undefined;
                    },
                    error: function (er) {
                        // alert(er);
                    }

                });
            }
        }
    });
    $("#uploadBrideImage").change(function (e) {
        filename = e.target.files[0].name;
        if (filename == undefined) {
            // $('#errormsg').hide();
            //$("#lblcsvfilename").css('display', 'inline-block');
            $('#progressBrideimg').css('display', 'none');
            $("#lblBrideimage").html("Please select image.");
            $('#lblBrideimage').css('color', 'red');

            return false;
        }
        else {
            // var filename = xfile.name;
            if (!(/\.(jpg|jpeg|png|gif|JPG|PNG|GIF)$/i).test(filename)) {
                $('#progressBrideimg').css('display', 'none');
                $("#lblBrideimage").html("File format not support.");
                $('#lblBrideimage').css('color', 'red');

                return false;
            }
            else {
                $('#progressBrideimg').css('display', 'block');
                $('#lblBrideimage').css('color', 'black');
                var data = new FormData();
                $("#lblBrideimage").val("uploading...");
                var files = $("#uploadBrideImage").get(0).files;
                var bar = $('.progress-bar');

                if (files.length > 0) {
                    data.append("MyImages", files[0]);
                    data.append("imgtype", $(this).data('imgtype'));
                    data.append("imageof", $(this).data('imageof'));
                    data.append("myId", $(this).data('id'));
                    data.append("sequence", $(this).data('sequence'));
                    data.append("eventID", weddingId);
                }

                $.ajax({
                    xhr: function () {
                        var xhr = new window.XMLHttpRequest();
                        $("#lblBrideimage").text("Uploading Image...")
                        xhr.upload.addEventListener("progress", function (evt) {
                            if (evt.lengthComputable) {
                                var percentComplete = evt.loaded * 2.5 / evt.total;
                                percentComplete = parseInt(percentComplete * 100);
                                console.log(percentComplete);
                                bar.width(percentComplete);
                                if (percentComplete === 100) {

                                }
                            }
                        }, false);

                        return xhr;
                    },
                    url: "/Wedding/UploadImageFile",
                    type: "POST",
                    processData: false,
                    contentType: false,
                    data: data,
                    success: function (response) {
                        $("#imgBridePreview").attr('src', response);
                        $("#BrideimageUrl").val(response);
                        $('#progressBrideimg').css('display', 'none');
                        bar.width(0);
                        $("#lblBrideimage").html("Image Uploaded");
                        filename = undefined;
                    },
                    error: function (er) {
                        // alert(er);
                    }

                });
            }
        }
    });
    $("#uploadGroomImage").change(function (e) {
        filename = e.target.files[0].name;
        if (filename == undefined) {
            // $('#errormsg').hide();
            //$("#lblcsvfilename").css('display', 'inline-block');
            $('#progressGroomimg').css('display', 'none');
            $("#lblGroomimage").html("Please select image.");
            $('#lblGroomimage').css('color', 'red');

            return false;
        }
        else {
            // var filename = xfile.name;
            if (!(/\.(jpg|jpeg|png|gif|JPG|PNG|GIF)$/i).test(filename)) {
                $('#progressGroomimg').css('display', 'none');
                $("#lblGroomimage").html("File format not support.");
                $('#lblGroomimage').css('color', 'red');

                return false;
            }
            else {
                $('#progressGroomimg').css('display', 'block');
                $('#lblGroomimage').css('color', 'black');
                var data = new FormData();
                $("#lblGroomimage").val("uploading...");
                var files = $("#uploadGroomImage").get(0).files;
                var bar = $('.progress-bar');

                if (files.length > 0) {
                    data.append("MyImages", files[0]);
                    data.append("imgtype", $(this).data('imgtype'));
                    data.append("imageof", $(this).data('imageof'));
                    data.append("myId", $(this).data('id'));
                    data.append("sequence", $(this).data('sequence'));
                    data.append("eventID", weddingId);
                }

                $.ajax({
                    xhr: function () {
                        var xhr = new window.XMLHttpRequest();
                        $("#lblGroomimage").text("Uploading Image...")
                        xhr.upload.addEventListener("progress", function (evt) {
                            if (evt.lengthComputable) {
                                var percentComplete = evt.loaded * 2.5 / evt.total;
                                percentComplete = parseInt(percentComplete * 100);
                                console.log(percentComplete);
                                bar.width(percentComplete);
                                if (percentComplete === 100) {

                                }
                            }
                        }, false);

                        return xhr;
                    },
                    url: "/Wedding/UploadImageFile",
                    type: "POST",
                    processData: false,
                    contentType: false,
                    data: data,
                    success: function (response) {
                        $("#imgGroomPreview").attr('src', response);
                        $("#GroomimageUrl").val(response);
                        $('#progressGroomimg').css('display', 'none');
                        bar.width(0);
                        $("#lblGroomimage").html("Image Uploaded");
                        filename = undefined;
                    },
                    error: function (er) {
                        // alert(er);
                    }

                });
            }
        }
    });
    $("#uploadWeddBackGImage").change(function (e) {
        filename = e.target.files[0].name;
        if (filename == undefined) {
            $('#progressWeddBackGimg').css('display', 'none');
            $("#lblWeddBackGimage").html("Please select image.");
            $('#lblWeddBackGimage').css('color', 'red');

            return false;
        }
        else {
            // var filename = xfile.name;
            if (!(/\.(jpg|jpeg|png|gif|JPG|PNG|GIF)$/i).test(filename)) {
                $('#progressWeddBackGimg').css('display', 'none');
                $("#lblWeddBackGimage").html("File format not support.");
                $('#lblWeddBackGimage').css('color', 'red');

                return false;
            }
            else {
                $('#progressWeddBackGimg').css('display', 'block');
                $('#lblWeddBackGimage').css('color', 'black');
                var data = new FormData();
                $("#lblWeddBackGimage").val("uploading...");
                var files = $("#uploadWeddBackGImage").get(0).files;
                var bar = $('.progress-bar');

                if (files.length > 0) {
                    data.append("MyImages", files[0]);
                    data.append("imgtype", $(this).data('imgtype'));
                    data.append("imageof", $(this).data('imageof'));
                    data.append("myId", $(this).data('id'));
                    data.append("sequence", $(this).data('sequence'));
                    data.append("eventID", weddingId);
                }

                $.ajax({
                    xhr: function () {
                        var xhr = new window.XMLHttpRequest();
                        $("#lblWeddBackGimage").text("Uploading Image...")
                        xhr.upload.addEventListener("progress", function (evt) {
                            if (evt.lengthComputable) {
                                var percentComplete = evt.loaded * 2.5 / evt.total;
                                percentComplete = parseInt(percentComplete * 100);
                                console.log(percentComplete);
                                bar.width(percentComplete);
                                if (percentComplete === 100) {

                                }
                            }
                        }, false);

                        return xhr;
                    },
                    url: "/Wedding/UploadImageFile",
                    type: "POST",
                    processData: false,
                    contentType: false,
                    data: data,
                    success: function (response) {
                        $("#imgWeddBackGPreview").attr('src', response);
                        $("#hdWeddBackGImageUrl").val(response);
                        $('#progressWeddBackGImg').css('display', 'none');
                        bar.width(0);
                        $("#lblWeddBackGimage").html("Image Uploaded");
                        filename = undefined;
                    },
                    error: function (er) {
                        // alert(er);
                    }

                });
            }
        }
    });
    $("#uploadModalTimeLineImage").change(function (e) {
        filename = e.target.files[0].name;
        if (filename == undefined) {
            $('#progressTimeLineimg').css('display', 'none');
            $("#lblModalTimeLineimage").html("Please select image.");
            $('#lblModalTimeLineimage').css('color', 'red');

            return false;
        }
        else {
            // var filename = xfile.name;
            if (!(/\.(jpg|jpeg|png|gif|JPG|PNG|GIF)$/i).test(filename)) {
                $('#progressTimeLineimg').css('display', 'none');
                $("#lblModalTimeLineimage").html("File format not support.");
                $('#lblModalTimeLineimage').css('color', 'red');

                return false;
            }
            else {
                $('#progressModalTimeLineimg').css('display', 'block');
                $('#lblModalTimeLineimage').css('color', 'black');
                var data = new FormData();
                $("#lblModalTimeLineimage").val("uploading...");
                var files = $("#uploadModalTimeLineImage").get(0).files;
                var bar = $('.progress-bar');

                if (files.length > 0) {
                    data.append("MyImages", files[0]);
                    data.append("imgtype", $(this).data('imgtype'));
                    data.append("imageof", $(this).data('imageof'));
                    data.append("myId", $(this).data('id'));
                    data.append("sequence", $(this).data('sequence'));
                    data.append("eventID", weddingId);
                }

                $.ajax({
                    xhr: function () {
                        var xhr = new window.XMLHttpRequest();
                        $("#lblModalTimeLineimage").text("Uploading Image...")
                        xhr.upload.addEventListener("progress", function (evt) {
                            if (evt.lengthComputable) {
                                var percentComplete = evt.loaded / evt.total;
                                percentComplete = parseInt(percentComplete * 100);
                                console.log(percentComplete);
                                bar.width(percentComplete);
                                if (percentComplete === 100) {

                                }
                            }
                        }, false);

                        return xhr;
                    },
                    url: "/Wedding/UploadImageFile",
                    type: "POST",
                    processData: false,
                    contentType: false,
                    data: data,
                    success: function (response) {
                        $("#imgModalTimeLinePreview").attr('src', response);
                        $("#hdModalTimeLineImageUrl").val(response);
                        $('#progressModalTimeLineImg').css('display', 'none');
                        bar.width(0);
                        $("#lblModalTimeLineimage").html("Image Uploaded");
                        filename = undefined;
                    },
                    error: function (er) {
                        // alert(er);
                    }

                });
            }
        }
    });
    
    $("#btnWeddCancel").click(function () {
        $('#formWedding input[type="text"]').val('');
    });

    //**************** BRIDE MODAL ADD AND UPDATE DATA ************************
    $("#btnAddBrideMaid").click(function () {
        ClearBrideModalForm();
        $('#btnBrideModal').trigger('click');
    });
    $('.updateBrideModal').click(function () {
        $('#btnBrideModal').trigger('click');
        var BrideID = $(this).data('id');
        GetBrideDetailsByID(BrideID);
        ClearBrideModalForm();
    });

    function ClearBrideModalForm() {
        
        $("#hdBrideWeddingID").val('');
        $("#hdBrideMaidID").val('');
        $("#modalBrideFName").val('');
        $("#modalBrideLName").val('');
        $("#hdModalBrideImageUrl").val('');
        $('#imgModalBridePreview').attr('src', '');
        $("#modalBrideAbout").val('');
        $("#modalBrideRelation").val('');
        $("#datepickerbr2modal").val('');
        $('#uploadModalBrideImage').attr('data-id', 0);
        $("#modalBridefbUrl").val('');
        $("#modalBridegoogleUrl").val('');
        $("#modalBrideinstagramUrl").val('');
        $("#datepickerbr2modal").datepicker({  enddate: today });
    };

    $('#ddlModalBrideRelation').change(function () {
        var ddlbriderelation = $('#ddlModalBrideRelation');
        if (ddlbriderelation != "") {

            var ddlSelectedMaster = $('#ddlModalBrideRelation option:selected').val();
            $('#ddlModalBrideRelation').valueOf(ddlSelectedMaster);

        }
        else {
            //$('.upload-file-btn').addClass('disabled');
            //$('#uploadcsv')[0].disabled = true;
        }
    });
    function GetBrideDetailsByID(BrideID) {
        var url = "../Wedding/GetBrideDetailsByID?Id=" + BrideID;
        $("#ModalTitle").html("Update Student Record");
        console.log("request starts");

        $.ajax({
            type: "GET",
            url: url,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                $('#hdBrideWeddingID').val(data.BrideAndMaidsBO.WeddingID);
                $('#uploadModalBrideImage').attr('data-id', data.BrideAndMaidsBO.BrideAndMaidID);
                $('#hdBrideMaidID').val(data.BrideAndMaidsBO.BrideAndMaidID);
                $('#modalBrideFName').val(data.BrideAndMaidsBO.FirstName);
                $('#modalBrideLName').val(data.BrideAndMaidsBO.LastName);
                $('#hdModalBrideImageUrl').val(data.BrideAndMaidsBO.Imageurl);
                $("#modalBridefbUrl").val(data.BrideAndMaidsBO.fbUrl);
                $("#modalBridegoogleUrl").val(data.BrideAndMaidsBO.googleUrl);
                $("#modalBrideinstagramUrl").val(data.BrideAndMaidsBO.instagramUrl);
                $('#imgModalBridePreview').attr('src', data.BrideAndMaidsBO.Imageurl);
                $('#modalBrideAbout').val(data.BrideAndMaidsBO.AboutBrideMaid);
                $('#modalBrideRelation').val(data.BrideAndMaidsBO.RelationWithBride);
                $('#datepickerbr2modal').val(data.BrideAndMaidsBO.strDateofBirth);
                //$("#datepickerbr2modal").date(data.BrideAndMaidsBO.strDateofBirth);
                $('#ddlModalBrideRelation option:selected').text(data.BrideAndMaidsBO.strRelationWithBride);
            }
        })
    };


    $("#ddlModalBrideRelation").change(function () {
        var selectedText = $(this).find("option:selected").text();
        var selectedValue = $(this).val();
        $('#uploadModalBrideImage').attr('data-imageof', selectedText);
    });

    $("#uploadModalBrideImage").change(function (e) {
        filename = e.target.files[0].name;
        if (filename == undefined) {
            $('#progressBrideimg').css('display', 'none');
            $("#lblModalBrideimage").html("Please select image.");
            $('#lblModalBrideimage').css('color', 'red');

            return false;
        }
        else {
            // var filename = xfile.name;
            if (!(/\.(jpg|jpeg|png|gif|JPG|PNG|GIF)$/i).test(filename)) {
                $('#progressBrideimg').css('display', 'none');
                $("#lblModalBrideimage").html("File format not support.");
                $('#lblModalBrideimage').css('color', 'red');

                return false;
            }
            else {
                $('#progressModalBrideimg').css('display', 'block');
                $('#lblModalBrideimage').css('color', 'black');
                var data = new FormData();
                $("#lblModalBrideimage").val("uploading...");
                var files = $("#uploadModalBrideImage").get(0).files;
                var bar = $('.progress-bar');

                if (files.length > 0) {
                    data.append("MyImages", files[0]);
                    data.append("imgtype", $(this).data('imgtype'));
                    data.append("imageof", $(this).data('imageof'));
                    data.append("myId", $(this).data('id'));
                    data.append("sequence", $(this).data('sequence'));
                    data.append("eventID", weddingId);
                }

                $.ajax({
                    xhr: function () {
                        var xhr = new window.XMLHttpRequest();
                        $("#lblModalBrideimage").text("Uploading Image...")
                        xhr.upload.addEventListener("progress", function (evt) {
                            if (evt.lengthComputable) {
                                var percentComplete = evt.loaded * 2.5 / evt.total;
                                percentComplete = parseInt(percentComplete * 100);
                                console.log(percentComplete);
                                bar.width(percentComplete);
                                if (percentComplete === 100) {

                                }
                            }
                        }, false);

                        return xhr;
                    },
                    url: "/Wedding/UploadImageFile",
                    type: "POST",
                    processData: false,
                    contentType: false,
                    data: data,
                    success: function (response) {
                        $("#imgModalBridePreview").attr('src', response);
                        $("#hdModalBrideImageUrl").val(response);
                        $('#progressModalBrideImg').css('display', 'none');
                        bar.width(0);
                        $("#lblModalBrideimage").html("Image Uploaded");
                        filename = undefined;
                    },
                    error: function (er) {
                        // alert(er);
                    }

                });
            }
        }
    });

    //**************** END BRIDE SECTION ************************


    //**************** GROOM MODAL ADD AND UPDATE DATA ************************
    $("#btnAddGroomMan").click(function () {
        ClearGroomModalForm();
        $('#btnGroomModal').trigger('click');
    });
    $('.updateGroomModal').click(function () {
        ClearGroomModalForm();
        $('#btnGroomModal').trigger('click');
        var GroomID = $(this).data('id');
        GetGroomDetailsByID(GroomID);
       
    });

    function ClearGroomModalForm() {
        $("#hdGroomWeddingID").val('');
        $("#hdGroomAndMenID").val('');
        $("#modalGroomFName").val('');
        $("#modalGroomLName").val('');
        $("#hdModalGroomImageUrl").val('');
        $('#imgModalGroomPreview').attr('src', '');
        $("#modalGroomAbout").val('');
        $("#modalGroomRelation").val('');
        $("#datepickergr2modal").val('');
        $('#uploadModalGroomImage').attr('data-id', 0);
        $("#modalGroomfbUrl").val('');
        $("#modalGroomgoogleUrl").val('');
        $("#modalGroominstagramUrl").val('');
        $("#GroomAndMenBO_modalDateofBirth").val('');
    };
    $('#ddlModalGroomRelation').change(function () {
        var ddlgroomrelation = $('#ddlModalGroomRelation');
        if (ddlgroomrelation != "") {
            ddlSelectedGroomRelation = $('#ddlModalGroomRelation option:selected').val();
            $('#ddlModalGroomRelation').valueOf(ddlSelectedGroomRelation);

        }
        else {
            //$('.upload-file-btn').addClass('disabled');
            //$('#uploadcsv')[0].disabled = true;
        }
    });
    function GetGroomDetailsByID(GroomID) {
        var url = "../Wedding/GetGroomDetailsByID?Id=" + GroomID;
        $("#ModalTitle").html("Update Groom Details");
        $.ajax({
            type: "GET",
            url: url,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                console.log(data);
                $('#hdGroomWeddingID').val(data.GroomAndMenBO.WeddingID);
                $('#hdGroomAndMenID').val(data.GroomAndMenBO.GroomAndMenID);
                $('#uploadModalGroomImage').attr('data-id', data.GroomAndMenBO.GroomAndMenID);
                $('#modalGroomFName').val(data.GroomAndMenBO.FirstName);
                $('#modalGroomLName').val(data.GroomAndMenBO.LastName);
                $('#hdModalGroomImageUrl').val(data.GroomAndMenBO.Imageurl);
                $('#imgModalGroomPreview').attr('src', data.GroomAndMenBO.Imageurl);
                $('#modalGroomAbout').val(data.GroomAndMenBO.AboutMen);
                $('#modalGroomRelation').val(data.GroomAndMenBO.RelationWithGroom);
                $('#datepickergr2modal').val(data.GroomAndMenBO.strDateofBirth);
                $('#GroomAndMenBO_modalDateofBirth').val(data.GroomAndMenBO.strDateofBirth);
                $('#ddlModalGroomRelation option:selected').text(data.GroomAndMenBO.strRelationWithGroom)
                $("#modalGroomfbUrl").val(data.GroomAndMenBO.fbUrl);
                $("#modalGroomgoogleUrl").val(data.GroomAndMenBO.googleUrl);
                $("#modalGroominstagramUrl").val(data.GroomAndMenBO.instagramUrl);
            }
        })
    };

    $("#ddlModalGroomRelation").change(function () {
        var selectedText = $(this).find("option:selected").text();
        var selectedValue = $(this).val();
        $('#uploadModalGroomImage').attr('data-imageof', selectedText);
    });

    $("#uploadModalGroomImage").change(function (e) {
        filename = e.target.files[0].name;
        if (filename == undefined) {
            $('#progressGroomimg').css('display', 'none');
            $("#lblModalGroomimage").html("Please select image.");
            $('#lblModalGroomimage').css('color', 'red');

            return false;
        }
        else {
            // var filename = xfile.name;
            if (!(/\.(jpg|jpeg|png|gif|JPG|PNG|GIF)$/i).test(filename)) {
                $('#progressGroomimg').css('display', 'none');
                $("#lblModalGroomimage").html("File format not support.");
                $('#lblModalGroomimage').css('color', 'red');

                return false;
            }
            else {
                $('#progressModalGroomimg').css('display', 'block');
                $('#lblModalGroomimage').css('color', 'black');
                var data = new FormData();
                $("#lblModalGroomimage").val("uploading...");
                var files = $("#uploadModalGroomImage").get(0).files;
                var bar = $('.progress-bar');

                if (files.length > 0) {
                    data.append("MyImages", files[0]);
                    data.append("imgtype", $(this).data('imgtype'));
                    data.append("imageof", $(this).data('imageof'));
                    data.append("myId", $(this).data('id'));
                    data.append("sequence", $(this).data('sequence'));
                    data.append("eventID", weddingId);
                }

                $.ajax({
                    xhr: function () {
                        var xhr = new window.XMLHttpRequest();
                        $("#lblModalGroomimage").text("Uploading Image...")
                        xhr.upload.addEventListener("progress", function (evt) {
                            if (evt.lengthComputable) {
                                var percentComplete = evt.loaded * 2.5 / evt.total;
                                percentComplete = parseInt(percentComplete * 100);
                                console.log(percentComplete);
                                bar.width(percentComplete);
                                if (percentComplete === 100) {

                                }
                            }
                        }, false);

                        return xhr;
                    },
                    url: "/Wedding/UploadImageFile",
                    type: "POST",
                    processData: false,
                    contentType: false,
                    data: data,
                    success: function (response) {
                        $("#imgModalGroomPreview").attr('src', response);
                        $("#hdModalGroomImageUrl").val(response);
                        $('#progressModalGroomImg').css('display', 'none');
                        bar.width(0);
                        $("#lblModalGroomimage").html("Image Uploaded");
                        filename = undefined;
                    },
                    error: function (er) {
                        // alert(er);
                    }

                });
            }
        }
    });

    //-------- **********************  TIMELINE MODAL ******************---------------------
    $("#btnAddTimeLine").click(function () {
        ClearTimeLineModalForm();
        $('#btnTimeLineModal').trigger('click');
    });
    $('.updateTimeLineModal').click(function () {
        $('#btnTimeLineModal').trigger('click');
        var TimeLineID = $(this).data('id');
        GetTimeLineDetailsByID(TimeLineID);
        ClearTimeLineModalForm();
    });

    function ClearTimeLineModalForm() {
        $(this).closest('frmTimeLineModal').find("input[type=text], textarea").val("");
        $("#frmTimeLineModal").trigger('reset');
    };

    function GetTimeLineDetailsByID(TimeLineID) {
        var url = "../Wedding/GetTimeLineDetailsByID?Id=" + TimeLineID;
        $("#ModalTitle").html("Update Student Record");

        $.ajax({
            type: "GET",
            url: url,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                console.log(data);
                $('#hdTimeLineWeddingID').val(data.TimeLineBO.WeddingID);
                $('#uploadModalTimeLineImage').attr('data-id', data.TimeLineBO.TimeLineID);
                $('#hdmodalTimeLineID').val(data.TimeLineBO.TimeLineID);
                $('#modalTimeLineTitle').val(data.TimeLineBO.Title);               
                $('#hdModalTimeLineImageUrl').val(data.TimeLineBO.ImageUrl);              
                $('#imgModalTimeLinePreview').attr('src', data.TimeLineBO.ImageUrl);
                $('#modalTimeLineStory').val(data.TimeLineBO.Story);
                $('#modalTimelineStorydate').val(data.TimeLineBO.strStoryDate);

            }
        })
    };

    $(function () {

        //rel="date"
        //Date picker
        $('#datepickerweddingdate').datepicker({
            autoclose: true,
            format: 'dd-M-yyyy'

        })
        $('#datepickerbride').datepicker({
            autoclose: true,
            format: 'dd-M-yyyy'
        })
        $('#datepickergroom').datepicker({
            autoclose: true,
            format: 'dd-M-yyyy'
        })
        //Date picker
        $('#datepickerbr2modal').datepicker({
            autoclose: true,
            format: 'dd-M-yyyy'
        })
        //Date picker
        $('#datepickergr2modal').datepicker({
            autoclose: true,
            format: 'dd-M-yyyy'
        })

        $('#datepickert').datepicker({
            autoclose: true,
            format: 'dd-M-yyyy'
        })
        //Date picker
        $('#eventDate').datepicker({
            autoclose: true,
            format: 'dd-M-yyyy'
        })
        
        $('#timelineStorydate').datepicker({
            autoclose: true,
            format: 'dd-M-yyyy'
        })
        $('#modalTimelineStorydate').datepicker({
            autoclose: true,
            format: 'dd-M-yyyy'
        })
        //Timepicker
        $('#eventStartTime').timepicker({
            showInputs: false
        })
        //Timepicker
        $('#eventEndTime').timepicker({
            showInputs: false
        })
    });
    if (showPopup) {
        //$('#modal-success').modal('toggle');
        $('#btnModal').trigger('click');
    }
});


