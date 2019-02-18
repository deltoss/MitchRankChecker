// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

$.extend(true, $.fn.dataTable.defaults, {
    colReorder: true,
    responsive: true,
    stateSave: true,
    searchDelay: 1000,
    dom: '<"html5buttons"B>lTfrgtip',
    lengthMenu: [
        [10, 25, 50, 75, 100],
        ['10', '25', '50', '75', '100']
    ],
    buttons: [
        {
            text: '<i class="fas fa-sync-alt"></i>',
            action: function (e, dt, node, config) {
                dt.ajax.reload();
            },
            titleAttr: function (dt) {
                return dt.i18n('buttons.refresh', 'Refresh');
            },
        },
        {
            extend: 'copy',
            text: '<i class="fa-fw fas fa-copy"></i>',
            titleAttr: function (dt) {
                return dt.i18n('buttons.copy', 'Copy');
            },
        },
        {
            extend: 'excel',
            text: '<i class="fa-fw fas fa-file-excel"></i>',
            exportOptions: {
                columns: ':visible:not(:last-child)'
            },
            titleAttr: function (dt) {
                return dt.i18n('buttons.excel', 'Excel');
            },
        },
        {
            extend: 'csv',
            text: '<i class="fa-fw fas fa-file-alt"></i>',
            exportOptions: {
                columns: ':visible:not(:last-child)'
            },
            titleAttr: function (dt) {
                return dt.i18n('buttons.csv', 'CSV');
            },
        },
        {
            extend: 'pdf',
            text: '<i class="fa-fw fas fa-file-pdf"></i>',
            orientation: 'landscape',
            exportOptions: {
                columns: ':visible:not(:last-child)'
            },
            titleAttr: function (dt) {
                return dt.i18n('buttons.pdf', 'PDF');
            },
        },
        {
            extend: 'print',
            text: '<i class="fa-fw fas fa-print"></i>',
            titleAttr: function (dt) {
                return dt.i18n('buttons.print', 'Print');
            },
        },
        {
            extend: 'colvis',
            text: '<i class="fa-fw fas fa-cog"></i>',
            titleAttr: function (dt) {
                return dt.i18n('buttons.colvis', 'Change Columns');
            },
            columns: ':not(.noVis)',
            collectionLayout: 'two-column',
            postfixButtons: [
                // This would reset the column visibility back
                // to the original specified settings
                // With stateSave turned off, this is identical
                // to the button 'colvisRestore'.
                // With it turned on, colVis restore only
                // 'cancels' the visibility changes since
                // the page was loaded.
                // This button would completely reset to
                // the original visibility settings as 
                // specified in the options
                {
                    class: 'buttons-colvisDefaultView',
                    text: function (dt) {
                        return dt.i18n('buttons.colvisDefaultView', 'Default view');
                    },
                    init: function (dt, button, conf) {
                        // If column settings were specified, then
                        // determine which columns were originally visible,
                        // and save it to a private property of the data 
                        // tables instance. Otherwise, just set all 
                        // columns to be visible
                        var originalColumns = dt.settings()[0].oInit.columns;
                        if (originalColumns) {
                            conf._visDefault = originalColumns.map(function (originalColumn, index, arr) {
                                if (typeof originalColumn.visible === 'undefined' || originalColumn.visible === true)
                                    return true;
                                return false;
                            });
                        }
                        else {
                            conf._visDefault = dt.columns().indexes().map(function (idx) {
                                return true;
                            }).toArray();
                        }
                    },
                    action: function (e, dt, button, conf) {
                        dt.columns().every(function (i) {
                            // Take into account that ColReorder might have disrupted our
                            // indexes
                            var idx = dt.colReorder && dt.colReorder.transpose ?
                                dt.colReorder.transpose(i, 'toOriginal') :
                                i;
                            var shouldShow = conf._visDefault[idx];
                            this.visible(shouldShow);
                        });
                    }
                },
            ]
        },
    ]
});

// So that pace js also intercepts all DataTables
// AJAX requests to show the pace js loader
$(document).on('processing.dt', function (e, settings, processing) {
    if (!Pace)
        return;
    if (processing) {
        Pace.start();
    } else {
        Pace.stop();
    }
});