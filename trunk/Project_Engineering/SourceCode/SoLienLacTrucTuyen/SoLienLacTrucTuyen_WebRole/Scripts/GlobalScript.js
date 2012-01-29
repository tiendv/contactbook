$(document).ready(function () {
    $('.header td').last().addClass('ui-corner-tr');

    $(".BtnLogin").hover(function () {
        $(this).attr("src", "/Styles/buttons/button_login_hover.png");
    }, function () {
        $(this).attr("src", "/Styles/buttons/button_login.png");
    });

    $(".BtnSearch").hover(function () {
        $(this).attr("src", "/Styles/buttons/button_search_hover.png");
    }, function () {
        $(this).attr("src", "/Styles/buttons/button_search.png");
    });

    $(".BtnAdd").hover(function () {
        if ($(".BtnAdd").is(':disabled') == false) {
            $(this).attr("src", "/Styles/buttons/button_add_hover.png");
        }
    }, function () {
        if ($(".BtnAdd").is(':disabled') == false) {
            $(this).attr("src", "/Styles/buttons/button_add.png");
        }
    });

    $(".BtnImport").hover(function () {
        $(this).attr("src", "/Styles/buttons/button_import_hover.png");
    }, function () {
        $(this).attr("src", "/Styles/buttons/button_import.png");
    });

    $(".BtnExport").hover(function () {
        if ($(".BtnExport").is(':disabled') == false) {
            $(this).attr("src", "/Styles/buttons/button_export_hover.png");
        }
    }, function () {
        if ($(".BtnExport").is(':disabled') == false) {
            $(this).attr("src", "/Styles/buttons/button_export.png");
        }
    });

    $(".UploadButton").hover(function () {
        if ($(".UploadButton").is(':disabled') == false) {
            $(this).attr("src", "/Styles/buttons/button_upload_hover.png");
        }
    }, function () {
        if ($(".UploadButton").is(':disabled') == false) {
            $(this).attr("src", "/Styles/buttons/button_upload.png");
        }
    });

    $(".BtnEditWithouDisable").hover(function () {
        if ($(".BtnEditWithouDisable").is(':disabled') == false) {
            $(this).attr("src", "/Styles/buttons/button_edit_hover.png");
        }
    }, function () {
        if ($(".BtnEditWithouDisable").is(':disabled') == false) {
            $(this).attr("src", "/Styles/buttons/button_edit.png");
        }
    });

    $(".BtnDanhGia").hover(function () {
        if ($(".BtnDanhGia").is(':disabled') == false) {
            $(this).attr("src", "/Styles/buttons/button_danhgia_hover.png");
        }
    }, function () {
        if ($(".BtnDanhGia").is(':disabled') == false) {
            $(this).attr("src", "/Styles/buttons/button_danhgia.png");
        }
    });

    $(".BtnEdit").hover(function () {
        if ($(".BtnEdit").is(':disabled') == false) {
            $(this).attr("src", "/Styles/buttons/button_edit_hover.png");
        }
    }, function () {
        if ($(".BtnEdit").is(':disabled') == false) {
            $(this).attr("src", "/Styles/buttons/button_edit.png");
        }
    });

    $(".BtnArrage").hover(function () {
        if ($(".BtnArrage").is(':disabled') == false) {
            $(this).attr("src", "/Styles/buttons/button_arrange_hover.png");
        }
    }, function () {
        if ($(".BtnArrage").is(':disabled') == false) {
            $(this).attr("src", "/Styles/buttons/button_arrange.png");
        }
    });

    $(".ButtonFeedback").hover(function () {
        if ($(".ButtonFeedback").is(':disabled') == false) {
            $(this).attr("src", "/Styles/buttons/button_feedback_hover.png");
        }
    }, function () {
        if ($(".ButtonFeedback").is(':disabled') == false) {
            $(this).attr("src", "/Styles/buttons/button_feedback.png");
        }
    });

    $(".BtnActivate").hover(function () {
        if ($(".BtnActivate").is(':disabled') == false) {
            $(this).attr("src", "/Styles/buttons/button_activate_hover.png");
        }
    }, function () {
        if ($(".BtnActivate").is(':disabled') == false) {
            $(this).attr("src", "/Styles/buttons/button_activate.png");
        }
    });

    $(".BtnDeactivate").hover(function () {
        if ($(".BtnDeactivate").is(':disabled') == false) {
            $(this).attr("src", "/Styles/buttons/button_deactivate_hover.png");
        }
    }, function () {
        if ($(".BtnDeactivate").is(':disabled') == false) {
            $(this).attr("src", "/Styles/buttons/button_deactivate.png");
        }
    });

    $(".BtnDelete").hover(function () {
        if ($(".BtnDelete").is(':disabled') == false) {
            $(this).attr("src", "/Styles/buttons/button_delete_hover.png");
        }
    }, function () {
        if ($(".BtnDelete").is(':disabled') == false) {
            $(this).attr("src", "/Styles/buttons/button_delete.png");
        }
    });

    $(".BtnConfirm").hover(function () {
        $(this).attr("src", "/Styles/buttons/button_confirm_hover.png");
    }, function () {
        $(this).attr("src", "/Styles/buttons/button_confirm.png");
    });

    $(".BtnClose").hover(function () {
        $(this).attr("src", "/Styles/buttons/button_close_hover.png");
    }, function () {
        $(this).attr("src", "/Styles/buttons/button_close.png");
    });

    $(".StepNextButton").hover(function () {
        $(this).attr("src", "/Styles/buttons/button_next_step_hover.png");
    }, function () {
        $(this).attr("src", "/Styles/buttons/button_next_step.png");
    });

    $(".StepPreviousButton").hover(function () {
        $(this).attr("src", "/Styles/buttons/button_prev_step_hover.png");
    }, function () {
        $(this).attr("src", "/Styles/buttons/button_prev_step.png");
    });

    $(".ContinueButton").hover(function () {
        $(this).attr("src", "/Styles/buttons/button_complete_hover.png");
    }, function () {
        $(this).attr("src", "/Styles/buttons/button_complete.png");
    });


    $(".DeleteItemButton").hover(function () {
        $(this).attr("src", "/Styles/buttons/button_delete_hover.png");
    }, function () {
        $(this).attr("src", "/Styles/buttons/button_delete.png");
    });

    $(".EditItemButton").hover(function () {
        $(this).attr("src", "/Styles/Images/MouseHover/button_edit.png");
    }, function () {
        $(this).attr("src", "/Styles/buttons/button_edit.png");
    });

    $(".SaveButton").hover(function () {
        $(this).attr("src", "/Styles/buttons/button_save_hover.png");
    }, function () {
        $(this).attr("src", "/Styles/buttons/button_save.png");
    });

    $(".CancelButton").hover(function () {
        $(this).attr("src", "/Styles/buttons/button_cancel_hover.png");
    }, function () {
        $(this).attr("src", "/Styles/buttons/button_cancel.png");
    });

    $(".YesButton").hover(function () {
        $(this).attr("src", "/Styles/buttons/button_yes_hover.png");
    }, function () {
        $(this).attr("src", "/Styles/buttons/button_yes.png");
    });

    $(".NoButton").hover(function () {
        $(this).attr("src", "/Styles/buttons/button_no_hover.png");
    }, function () {
        $(this).attr("src", "/Styles/buttons/button_no.png");
    });

    $(".button_close").hover(function () {
        $(this).attr("src", "/Styles/buttons/popup_button_close_hover.png");
    }, function () {
        $(this).attr("src", "/Styles/buttons/popup_button_close.png");
    });

    var bSelected = false;
    $(".select input[type='checkbox']").each(function () {
        if ($(this).is(':checked')) {
            $(this).parents('td').parents('tr').each(function () {
                $(this).find('td').addClass('hover');
            });

            bSelected = true;
        } else {
            $(this).parents('td').parents('tr').each(function () {
                $(this).find('td').removeClass('hover');
            });
        }
    });

    if (bSelected == true) {
        $(".BtnDelete").attr("src", "/Styles/buttons/button_delete.png");
        $(".BtnDelete:disabled").removeAttr('disabled');

        $(".BtnEdit").attr("src", "/Styles/buttons/button_edit.png");
        $(".BtnEdit:disabled").removeAttr('disabled');

        $(".BtnActivate").attr("src", "/Styles/buttons/button_activate.png");
        $(".BtnActivate:disabled").removeAttr('disabled');

        $(".BtnDeactivate").attr("src", "/Styles/buttons/button_deactivate.png");
        $(".BtnDeactivate:disabled").removeAttr('disabled');
    } else {
        $(".BtnDelete").attr("src", "/Styles/buttons/button_delete_disable.png");
        $(".BtnDelete").attr("disabled", "disabled");

        $(".BtnEdit").attr("src", "/Styles/buttons/button_edit_disable.png");
        $(".BtnEdit").attr("disabled", "disabled");

        $(".BtnActivate").attr("src", "/Styles/buttons/button_activate_disable.png");
        $(".BtnActivate").attr("disabled", "disabled");

        $(".BtnDeactivate").attr("src", "/Styles/buttons/button_deactivate_disable.png");
        $(".BtnDeactivate").attr("disabled", "disabled");
    }

    $('.selectAll').click(function () {
        $(".select input[type='checkbox']").attr('checked', $(".selectAll input[type='checkbox']").is(':checked'));

        var bSelected = false;
        $(".select input[type='checkbox']").each(function () {
            if ($(this).is(':checked')) {
                $(this).parents('td').parents('tr').each(function () {
                    $(this).find('td').addClass('hover');
                });

                bSelected = true;
            } else {
                $(this).parents('td').parents('tr').each(function () {
                    $(this).find('td').removeClass('hover');
                });
            }
        });

        if (bSelected == true) {
            $(".BtnDelete").attr("src", "/Styles/buttons/button_delete.png");
            $(".BtnDelete:disabled").removeAttr('disabled');

            $(".BtnEdit").attr("src", "/Styles/buttons/button_edit.png");
            $(".BtnEdit:disabled").removeAttr('disabled');

            $(".BtnActivate").attr("src", "/Styles/buttons/button_activate.png");
            $(".BtnActivate:disabled").removeAttr('disabled');

            $(".BtnDeactivate").attr("src", "/Styles/buttons/button_deactivate.png");
            $(".BtnDeactivate:disabled").removeAttr('disabled');
        } else {
            $(".BtnDelete").attr("src", "/Styles/buttons/button_delete_disable.png");
            $(".BtnDelete").attr("disabled", "disabled");

            $(".BtnEdit").attr("src", "/Styles/buttons/button_edit_disable.png");
            $(".BtnEdit").attr("disabled", "disabled");

            $(".BtnActivate").attr("src", "/Styles/buttons/button_activate_disable.png");
            $(".BtnActivate").attr("disabled", "disabled");

            $(".BtnDeactivate").attr("src", "/Styles/buttons/button_deactivate_disable.png");
            $(".BtnDeactivate").attr("disabled", "disabled");
        }
    });


    $(".select input[type='checkbox']").click(function () {
        var bSelected = false;
        $(".select input[type='checkbox']").each(function () {
            if ($(this).is(':checked')) {
                $(this).parents('td').parents('tr').each(function () {
                    $(this).find('td').addClass('hover');
                });

                bSelected = true;
            } else {
                $(this).parents('td').parents('tr').each(function () {
                    $(this).find('td').removeClass('hover');
                });

                $(".selectAll input[type='checkbox']").attr('checked', false);
            }
        });

        if (bSelected == true) {
            $(".BtnDelete").attr("src", "/Styles/buttons/button_delete.png");
            $(".BtnDelete:disabled").removeAttr('disabled');

            $(".BtnEdit").attr("src", "/Styles/buttons/button_edit.png");
            $(".BtnEdit:disabled").removeAttr('disabled');

            $(".BtnActivate").attr("src", "/Styles/buttons/button_activate.png");
            $(".BtnActivate:disabled").removeAttr('disabled');

            $(".BtnDeactivate").attr("src", "/Styles/buttons/button_deactivate.png");
            $(".BtnDeactivate:disabled").removeAttr('disabled');
        } else {
            $(".BtnDelete").attr("src", "/Styles/buttons/button_delete_disable.png");
            $(".BtnDelete").attr("disabled", "disabled");

            $(".BtnEdit").attr("src", "/Styles/buttons/button_edit_disable.png");
            $(".BtnEdit").attr("disabled", "disabled");

            $(".BtnActivate").attr("src", "/Styles/buttons/button_activate_disable.png");
            $(".BtnActivate").attr("disabled", "disabled");

            $(".BtnDeactivate").attr("src", "/Styles/buttons/button_deactivate_disable.png");
            $(".BtnDeactivate").attr("disabled", "disabled");
        }
    });

    $(".radio input[type='radio']").each(function () {
        if ($(this).is(':checked')) {
            $(this).parents('td').parents('tr').each(function () {
                $(this).find('td').addClass('hover');
            });
        } else {
            $(this).parents('td').parents('tr').each(function () {
                $(this).find('td').removeClass('hover');
            });
        }
    });
});