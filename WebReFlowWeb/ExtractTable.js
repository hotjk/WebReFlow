function ExtractTable() {
    var tables = document.querySelectorAll('table');
    var result = [];
    tables.forEach(function (table) {
        var rect = table.getBoundingClientRect();
        var ths = [];
        var tds = [];
        var rows = table.querySelectorAll('tr');
        rows.forEach(function (row) {
            var rowCells = row.querySelectorAll('th');
            rowCells.forEach(function (cell) {
                var cellRect = cell.getBoundingClientRect();
                ths.push({
                    x: parseInt(cellRect.left, 10),
                    y: parseInt(cellRect.top, 10),
                    w: parseInt(cellRect.width, 10),
                    h: parseInt(cellRect.height, 10)
                });
            });
            rowCells = row.querySelectorAll('td');
            rowCells.forEach(function (cell) {
                var cellRect = cell.getBoundingClientRect();
                tds.push({
                    x: parseInt(cellRect.left, 10),
                    y: parseInt(cellRect.top, 10),
                    w: parseInt(cellRect.width, 10),
                    h: parseInt(cellRect.height, 10)
                });
            });
        });
        result.push({
            x: parseInt(rect.left, 10),
            y: parseInt(rect.top, 10),
            w: parseInt(rect.width, 10),
            h: parseInt(rect.height, 10),
            ths: ths,
            tds: tds
        });
    });
    return result;
}