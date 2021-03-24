// lấy các dòng trong bảng có id là tbLp
const rows = document.querySelectorAll("#tbLP tbody tr");

// khởi tạo đối tượng để chứa dữ liệu có trong các dòng 
const object = {}
object.time = new Date();
object.data = [];

// dựa vào các keys này để gán dữ liệu từ các cột trong 1 dòng của bảng
const arr = [
    "Co",
    "Re",
    "Ce",
    "Fl",
    "BP3",
    "BQ3",
    "BP2",
    "BQ2",
    "BP1",
    "BQ1",
    "MP",
    "MQ",
    "MC",
    "SP1",
    "SQ1",
    "SP2",
    "SQ2",
    "SP3",
    "SQ3",
    "TQ",
    "Op",
    "Hi",
    "Lo",
    "FB",
    "FS"
];

// loại bỏ những cell có class là hide
const demo = document.querySelectorAll(".hide");
const removeElements = (elms) => elms.forEach(el => el.remove());
removeElements(demo);

// biến chứa keys của các cột khối lượng
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

// lặp các hàng trong table
for (let i = 0; i < rows.length; i++) {
    const tr = rows[i];
    // biến này chứa những giá trị trong từng cell
    let obj = {};
    // lặp các cell trong hàng đó
    for (let j = 0; j < tr.cells.length; j++) {
        const td = tr.cells[j];
        let value = td.innerText;
        // tìm dấu phảy và loại bỏ
        let checkNumber = value.replace(/,/g, "");
        // khởi tạo biến dùng cho việc nhân chia
        const multipleTen = 100;
        const divideTen = 10;
        // kiểm tra để không lấy dữ liệu từ cột mã
        if (arr[j] !== "Co") {
            checkNumber = parseFloat(checkNumber);
            // kiểm tra để không bị NaN vì trong trường hợp cell không có giá trị sẽ bị NaN
            if (!isNaN(checkNumber)) {
                // kiểm tra key trong array có nằm trong  biến kl hay không , cột khối lượng chia 10, cột giá nhân 100
                checkNumber = kl.includes(arr[j]) ? checkNumber / 10 : checkNumber * 100;
                // làm tròn giá trị đê không lấy quá nhiều số sau dấu chấm
                checkNumber = Math.round(checkNumber);
            } else {
                checkNumber = "";
            }
        }
        // biến obj lưu dữ liệu giá trị được format tại key nằm trong biến array
        obj[arr[j]] = checkNumber.toString();
    }
    // mỗi vòng lặp kết thúc đẩy dữ liệu từ biến obj vào mảng data
    object.data.push(obj);
}

console.log(object.data);

// let oData = object.data;
// for (let d = 0; d < oData.length; d++) {
//     console.log("update table1 set Re=" + oData[d]['Re'] + ", Ce=" + oData[d]['Ce'] + ", Fl=" + oData[d]['Fl'] + " where Co='" + oData[d]['Co'] + "'");
// }