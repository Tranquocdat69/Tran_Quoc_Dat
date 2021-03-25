    // ỉmport tất cả các biến chứa dữ liệu từ file ui2json.js
    import * as Data from "./data.js";

    // key của các cột khối lượng trong bảng
    let kl = [
        "SQ1",
        "SQ2",
        "SQ3",
        "SQ4",
        "BQ1",
        "BQ2",
        "BQ3",
        "BQ4",
        "TQ",
        "MQ"
    ];

    // gán biến để lấy dữ liệu từ file ui2json.js
    const array = Data.keyColumn;
    const data = Data.rawDataTable;
    const formattedData = [];

    // format lại dữ liệu từ file ui2json
    // lấy tất cả các object trong biến data
    for (let a = 0; a < data.length; a++) {
        // biến này chứa những giá trị trong các object của biến data 
        let obj = {};
        // lặp các key để dựa vào key đó lấy được dữ liệu trong object
        for (let b = 0; b < array.length; b++) {
            let valueColumn = data[a][array[b]];
            // không lấy giá trị từ cột mã
            if (array[b] !== "Co") {
                let checkValue = parseFloat(valueColumn);
                // kiểm tra để không bị NaN vì trong trường hợp cell không có giá trị sẽ bị NaN
                if (!isNaN(checkValue)) {
                    // kiểm tra key trong array có nằm trong  biến kl hay không , cột khối lượng nhân 10, cột giá chia 100
                    valueColumn = kl.includes(array[b]) ? valueColumn * 10 : valueColumn / 100;
                    // nếu giá trị không có chứa dấu chấm thì thực thi code
                    if (valueColumn.toString().indexOf(".") === -1) {
                        // format number thêm dấu phẩy ngăn cách giữa các số hàng nghìn
                        valueColumn = valueColumn.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
                    }
                }
            }
            // biến obj lưu dữ liệu giá trị được format tại key nằm trong biến array
            obj[array[b]] = valueColumn.toString();
        }
        // mỗi vòng lặp kết thúc đẩy dữ liệu từ biến obj vào mảng formattedData
        formattedData.push(obj);
    };

    // gán biến dataTable để đổ ra bảng 
    const dataTable = formattedData;

    // biến chứa các cột: giá, khối lượng, mã cột, mở cửa, cao nhất, thấp nhất để sau này tô màu 1 thể
    const dataPriceWeight = [
        ["BP3", "BQ3"],
        ["BP2", "BQ2"],
        ["BP1", "BQ1"],
        ["MP", "MQ", "MC", "Co"],
        ["SP1", "SQ1"],
        ["SP2", "SQ2"],
        ["SP3", "SQ3"],
        ["Op"],
        ["Hi"],
        ["Lo"]
    ];

    const cellBgGray = ["Re", "Ce", "Fl", "MP", "MQ", "MC", "TQ", "Op", "Hi", "Lo", "FB", "FS"];
    // lấy cả bảng có id là table
    const table = document.getElementById("table");

    // code đổ dữ liệu ra bảng, vòng lặp đầu tiên lấy dữ liệu các object trong biến dataTable
    for (let i = 0; i < dataTable.length; i++) {
        var row = document.createElement("tr");
        // vòng lặp thứ 2 lấy các key để dựa vào key ta lấy đc dữ liệu từ object
        for (let j = 0; j < array.length; j++) {
            var cell = document.createElement("td");
            const value = dataTable[i][array[j]];
            if (cellBgGray.includes(array[j])) {
                addClass(cell, "bg-gray");
            }
            // lấy giá trị từ 3 cột tc, trần, sàn để so sánh
            const value_Re = parseFloat(dataTable[i]["Re"]);
            const value_Ce = parseFloat(dataTable[i]["Ce"]);
            const value_Fl = parseFloat(dataTable[i]["Fl"]);
            cell.innerText = value;
            // đổ màu cho 3 cột: TC,Trần, Sàn
            if (array[j] === "Re" || array[j] === "Ce" || array[j] === "Fl") {
                addClassToMainColumn(cell, array[j]);
            }
            // giá trị khác rỗng mới thực thi tiếp
            if (value !== "") {
                // lặp các mảng trong mảng 2 chiều dataPriceWeight
                for (let v = 0; v < dataPriceWeight.length; v++) {
                    // kiểm tra dataPriceWeight có chứa các keys ở trong mảng array hay không
                    if (dataPriceWeight[v].includes(array[j])) {
                        let color = "";
                        // parse dữ liệu từ string sang float để so sánh
                        const valueParseFloat = parseFloat(dataTable[i][dataPriceWeight[v][0]]);
                        // so sánh giá trị hiện tại với các cột chính để gán cho biến color
                        if (valueParseFloat === value_Re) {
                            color = "yellow-color";
                        } else if (valueParseFloat === value_Ce) {
                            color = "purple-color";
                        } else if (valueParseFloat === value_Fl) {
                            color = "blue-color";
                        } else {
                            if (valueParseFloat > value_Re) {
                                color = "green-color";
                            }
                            if (valueParseFloat < value_Re) {
                                color = "red-color";
                            }
                        }
                        // lặp các phần tử có trong mảng con của mảng dataPriceWeight 
                        for (let t = 0; t < dataPriceWeight[v].length; t++) {
                            // tiến hành thêm class vào các cột có key nằm trong mảng con này
                            addClass(cell, color);
                        }

                    }

                }
            }
            row.appendChild(cell);
        }
        table.appendChild(row);
    }
    /**
     * thêm class color để tô màu cho các cell trong bảng
     * @param {HTMLTableCellElement} obj 
     * @param {String} string 
     * @returns {HTMLTableCellElement}
     */
    function addClass(obj, string) {
        if (string !== "") {
            return obj.classList.add(string);
        }
    }
    /**
     * tô màu vào 3 cột chính là TC, Trần, Sàn
     * @param {HTMLTableCellElement} obj 
     * @param {String} string 
     * @returns {HTMLTableCellElement}
     */
    function addClassToMainColumn(obj, string) {
        if (string === "Re") {
            return addClass(obj, "yellow-color")
        }
        if (string === "Ce") {
            return addClass(obj, "purple-color")
        }
        if (string === "Fl") {
            return addClass(obj, "blue-color")
        }
    }