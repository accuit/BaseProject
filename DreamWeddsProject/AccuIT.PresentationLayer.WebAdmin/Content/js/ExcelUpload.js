var isDuplicateFound = "N"
var filename = undefined;
var showPopup = false;
var ddlSelectedMaster = "";
var showUpdatePopup = "";
var showErrPopup = false;
var userID = "";
function DuplicateFound() {
    if (isDuplicateFound == "Y") {

        //$('body').append('<div id="overlay" class="pop-overlay"></div>');
        $("#overlay").show();
        DisableBackground(true);
        $('html, body').animate({ scrollTop: 0 }, 800);
        $('#modal-default').fadeIn(200);
        $('html, body').animate({ scrollTop: 0 }, 800);
    }


    $('html, body').animate({ scrollTop: 0 }, 800);

    if (isDuplicateFound == "N") {
        // $("#spellCheckPopUpSendBtn").hide();
        // $("#btnSendEshot").hide();
        return true;
    }
    else {

        return false;
    }
}

function openFileOption() {
    document.getElementById("uploadcsv").click();
}


$(document).ready(function () {
    isDuplicateFound = $('#DuplicateFound').val();
    showPopup = $('#ShowPopup').val();
    showErrPopup = $('#ShowErrPopup').val();
    showUpdatePopup = $('#ShowUpdatePopup').val();
    userID = $('hdUserId').val();
    //$('#modal-default').modal({
    //    backdrop: 'static',
    //    keyboard: false
    //});

    $('input[type="file"]').change(function (e) {
        filename = e.target.files[0].name;
        //alert('The file "' + filename + '" has been selected.');
        console.log('The file "' + filename + '" has been selected.')
    });

    $("#btnuploadcsv").click(function () {
        // var file = document.getElementById("uploadcsv");
        //var xfile = file.files[0];
        if ($('#DuplicateFound').val() != "Y") {
            if ($('#ddlMasterDataType').val() == "") {
                $('#errormsg').hide();
                $("#lblcsvfilename").css('display', 'inline-block');
                $("#lblcsvfilename").html("Please select data type.");
                document.getElementById('lblcsvfilename').style.color = "red";
                $('#ddlMasterDataType').addClass('has-error');
                $('#uploadcsv')[0].disabled = true;
                return false;
            }

            if (filename == undefined) {
                $('#errormsg').hide();
                $("#lblcsvfilename").css('display', 'inline-block');
                $("#lblcsvfilename").html("Please select a file.");
                $('#lblcsvfilename').style.color = "red";
                $('#ddlMasterDataType').addClass('has-error');
                //$('#uploadcsv')[0].disabled = true;
                return false;
            }
            else {
                // var filename = xfile.name;
                if (!(/\.(xlsx|xls|xlsm|csv)$/i).test(filename)) {
                    $("#lblcsvfilename").css('display', 'inline-block');
                    $("#lblcsvfilename").html("The file you have selected is not supported, please upload a CSV file. If this does not work please ensure your file is not empty.");
                    document.getElementById('lblcsvfilename').style.color = "red";
                    $('.browse-file').addClass('has-error').removeClass('file-selected');
                    // $('#uploadcsv')[0].disabled = true;
                    return false;
                }
                else {

                    var myxlfile = document.getElementById("uploadcsv");
                    document.getElementById('lblcsvfilename').style.color = "black";
                    var str = "Selected file:&nbsp;" + filename;
                    ddlSelectedMasterID = $("#ddlMasterDataType option:selected").index();
                    ddlSelectedMasterID = $("#ddlMasterDataType option:selected").val();
                    $("#lblcsvfilename").css('display', 'inline-block');
                    $("#lblcsvfilename").html(str);
                    $('.upload-file-top').removeClass('has-error').addClass('file-selected');
                    $('.browse-file').removeClass('has-error');
                    return true;
                }
            }
        }
    });

    $("#btndownloadcsv").click(function () {
        // var file = document.getElementById("uploadcsv");
        //var xfile = file.files[0];
        if ($('#DuplicateFound').val() != "Y") {

            ddlSelectedMaster = $("#ddlMasterDataType option:selected").index();
            ddlSelectedMaster = $("#ddlMasterDataType option:selected").val();
            $('#ddlSelectedMaster').val() = ddlSelectedMaster;
        }
    });

    
    $('.btn-download').click(function (e) {
        //e.stopProgagation();
        ddlSelectedMaster = $("#ddlMasterDataType option:selected").index();
        ddlSelectedMaster = $("#ddlMasterDataType option:selected").val();
        $('#ddlSelectedMaster').val() = ddlSelectedMaster;
    });

    $("#uploadUserImage").change(function (e) {
        filename = e.target.files[0].name;
        if (filename == undefined) {
            $('#progressBrideimg').css('display', 'none');
            $("#lblUserimage").html("Please select image.");
            $('#lblUserimage').css('color', 'red');

            return false;
        }
        else {
            // var filename = xfile.name;
            if (!(/\.(jpg|jpeg|png|gif|JPG|PNG|GIF)$/i).test(filename)) {
                $('#progressBrideimg').css('display', 'none');
                $("#lblUserimage").html("File format not support.");
                $('#lblUserimage').css('color', 'red');

                return false;
            }
            else {
                $('#progressUserImg').css('display', 'block');
                $('#lblUserimage').css('color', 'black');
                var data = new FormData();
                $("#lblUserimage").val("uploading...");
                var files = $("#uploadUserImage").get(0).files;
                var bar = $('.progress-bar');

                if (files.length > 0) {
                    data.append("MyImages", files[0]);
                    data.append("imgtype", $(this).data('imgtype'));
                    data.append("myId", $(this).data('id'));
                    data.append("sequence", $(this).data('sequence'));
                    data.append("eventID", userID);
                }

                $.ajax({
                    xhr: function () {
                        var xhr = new window.XMLHttpRequest();
                        $("#lblUserimage").text("Uploading Image...")
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
                        $("#imgUserPreview").attr('src', response);
                        $("#hdUserImageUrl").val(response);
                        $('#progressUserImg').css('display', 'none');
                        bar.width(0);
                        $("#lblUserimage").html("Image Uploaded");
                        filename = undefined;
                    },
                    error: function (er) {
                        // alert(er);
                    }

                });
            }
        }
    });

    //$("#ddlRole").on("change", function () {
    //    var roleid = $('#ddlRole option:selected').val();
    //    $('#ddlSelectedMasterText').valueOf(roleid);
    //    $.ajax(
    //    {
    //        url: '/Home/PartialManageRoleModules?roleID=' + $(this).attr("value"),
    //        type: 'GET',
    //        dataType: "json",
    //        contentType: 'application/json; charset=utf-8',
    //        success: function (data) {
    //            /* data is the pure html returned from action method, load it to your page */
    //            $('#partialPlaceHolder').html(data);
    //            /* little fade in effect */
    //            $('#partialPlaceHolder').fadeIn('fast');
    //           // $("#list2").html(data);
    //        },
    //        error: function () {
    //            //alert("error");
    //        }
    //    });
    //});



    if (showPopup) {
        $('#modal-default').modal('toggle');
        $('#modal-success').modal('toggle');
        //$('#btnModal').trigger('click');        
    }


});
