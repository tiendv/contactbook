$(document).ready(function () {
    $('.header td').last().addClass('ui-corner-tr');

    $(".BtnLogin").hover(function () {
        $(this).attr("src", "/Styles/Images/MouseHover/button_login.png");
    }, function () {
        $(this).attr("src", "/Styles/Images/button_login.png");
    });

    $(".BtnSearch").hover(function () {
        $(this).attr("src", "/Styles/Images/MouseHover/button_search_with_text.png");
    }, function () {
        $(this).attr("src", "/Styles/Images/button_search_with_text.png");
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

    $(".BtnEdit").hover(function () {
        if ($(".BtnEdit").is(':disabled') == false) {            
            $(this).attr("src", "/Styles/buttons/button_edit_hover.png");
        }
    }, function () {
        if ($(".BtnEdit").is(':disabled') == false) {
            $(this).attr("src", "/Styles/buttons/button_edit.png");
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
        $(this).attr("src", "/Styles/Images/MouseHover/button_next_step.png");
    }, function () {
        $(this).attr("src", "/Styles/Images/button_next_step.png");
    });

    $(".StepPreviousButton").hover(function () {
        $(this).attr("src", "/Styles/Images/MouseHover/button_prev_step.png");
    }, function () {
        $(this).attr("src", "/Styles/Images/button_prev_step.png");
    });

    $(".ContinueButton").hover(function () {
        $(this).attr("src", "/Styles/Images/MouseHover/button_complete.png");
    }, function () {
        $(this).attr("src", "/Styles/Images/button_complete.png");
    });


    $(".DeleteItemButton").hover(function () {
        $(this).attr("src", "/Styles/Images/MouseHover/button_delete.png");
    }, function () {
        $(this).attr("src", "/Styles/Images/button_delete.png");
    });

    $(".EditItemButton").hover(function () {
        $(this).attr("src", "/Styles/Images/MouseHover/button_edit.png");
    }, function () {
        $(this).attr("src", "/Styles/Images/button_edit.png");
    });

    $(".SaveButton").hover(function () {
        $(this).attr("src", "/Styles/Images/MouseHover/button_save.png");
    }, function () {
        $(this).attr("src", "/Styles/Images/button_save.png");
    });

    $(".CancelButton").hover(function () {
        $(this).attr("src", "/Styles/Images/MouseHover/button_cancel.png");
    }, function () {
        $(this).attr("src", "/Styles/Images/button_cancel.png");
    });

    $(".YesButton").hover(function () {
        $(this).attr("src", "/Styles/Images/MouseHover/button_yes.png");
    }, function () {
        $(this).attr("src", "/Styles/Images/button_yes.png");
    });

    $(".NoButton").hover(function () {
        $(this).attr("src", "/Styles/Images/MouseHover/button_no.png");
    }, function () {
        $(this).attr("src", "/Styles/Images/button_no.png");
    });

    $(".button_close").hover(function () {
        $(this).attr("src", "/Styles/Images/MouseHover/popup_button_close.png");
    }, function () {
        $(this).attr("src", "/Styles/Images/popup_button_close.png");
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
    } else {
        $(".BtnDelete").attr("src", "/Styles/buttons/button_delete_disable.png");
        $(".BtnDelete").attr("disabled", "disabled");

        $(".BtnEdit").attr("src", "/Styles/buttons/button_edit_disable.png");
        $(".BtnEdit").attr("disabled", "disabled");
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
        } else {
            $(".BtnDelete").attr("src", "/Styles/buttons/button_delete_disable.png");
            $(".BtnDelete").attr("disabled", "disabled");

            $(".BtnEdit").attr("src", "/Styles/buttons/button_edit_disable.png");
            $(".BtnEdit").attr("disabled", "disabled");
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
        } else {
            $(".BtnDelete").attr("src", "/Styles/buttons/button_delete_disable.png");
            $(".BtnDelete").attr("disabled", "disabled");

            $(".BtnEdit").attr("src", "/Styles/buttons/button_edit_disable.png");
            $(".BtnEdit").attr("disabled", "disabled");
        }
    });   
});