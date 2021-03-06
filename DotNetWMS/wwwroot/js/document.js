$(function () {
    $('#generate').click(function () {
        var from = -1;
        var fromIndex = -1;
        var to = -1;
        var toIndex = -1;

        var userFromVal = $('#userFrom').val();
        var warehouseFromVal = $('#warehouseFrom').val();
        var externalFromVal = $('#externalFrom').val();

        var selectlistFromVals = [userFromVal, warehouseFromVal, externalFromVal]

        for (var i = 0; i < selectlistFromVals.length; i++) {
            if (selectlistFromVals[i] != "") {
                from = selectlistFromVals[i];
                fromIndex = i;
                break;
            }
        }

        var userToVal = $('#userTo').val();
        var warehouseToVal = $('#warehouseTo').val();
        var externalToVal = $('#externalTo').val();

        var selectlistToVals = [userToVal, warehouseToVal, externalToVal]

        for (var i = 0; i < selectlistToVals.length; i++) {
            if (selectlistToVals[i] != "") {
                to = selectlistToVals[i];
                toIndex = i;
                break;
            }
        }
        console.log(from)
        $('#divPartial').load('/Doc_Assignments/ConfigureDocument', {
            from: from,
            to: to,
            fromIndex: fromIndex,
            toIndex: toIndex
        });
    })

    $('select').change(function () {
        //from
        if ($(this).attr('id') == 'userFrom' && $('#userFrom').val() != 0) {
            $('#warehouseFrom').prop('disabled', true)
            $('#externalFrom').prop('disabled', true)
        }

        if ($(this).attr('id') == 'warehouseFrom' && $('#warehouseFrom').val() != 0) {
            $('#userFrom').prop('disabled', true)
            $('#externalFrom').prop('disabled', true)
        }

        if ($(this).attr('id') == 'externalFrom' && $('#externalFrom').val() != 0) {
            $('#userFrom').prop('disabled', true)
            $('#warehouseFrom').prop('disabled', true)
        }

        if ($(this).attr('id') == 'userFrom' && $('#userFrom').val() == 0) {
            $('#warehouseFrom').prop('disabled', false)
            $('#externalFrom').prop('disabled', false)
        }

        if ($(this).attr('id') == 'warehouseFrom' && $('#warehouseFrom').val() == 0) {
            $('#userFrom').prop('disabled', false)
            $('#externalFrom').prop('disabled', false)
        }

        if ($(this).attr('id') == 'externalFrom' && $('#externalFrom').val() == 0) {
            $('#userFrom').prop('disabled', false)
            $('#warehouseFrom').prop('disabled', false)
        }

        //to
        if ($(this).attr('id') == 'userTo' && $('#userTo').val() != 0) {
            $('#warehouseTo').prop('disabled', true)
            $('#externalTo').prop('disabled', true)
        }

        if ($(this).attr('id') == 'warehouseTo' && $('#warehouseTo').val() != 0) {
            $('#userTo').prop('disabled', true)
            $('#externalTo').prop('disabled', true)
        }

        if ($(this).attr('id') == 'externalTo' && $('#externalTo').val() != 0) {
            $('#userTo').prop('disabled', true)
            $('#warehouseTo').prop('disabled', true)
        }

        if ($(this).attr('id') == 'userTo' && $('#userTo').val() == 0) {
            $('#warehouseTo').prop('disabled', false)
            $('#externalTo').prop('disabled', false)
        }

        if ($(this).attr('id') == 'warehouseTo' && $('#warehouseTo').val() == 0) {
            $('#userTo').prop('disabled', false)
            $('#externalTo').prop('disabled', false)
        }

        if ($(this).attr('id') == 'externalTo' && $('#externalTo').val() == 0) {
            $('#userTo').prop('disabled', false)
            $('#warehouseTo').prop('disabled', false)
        }

    })
})