const rows = document.querySelectorAll("#tbLP tbody tr");

const object = {}
object.time = new Date();
object.data = [];

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

const demo = document.querySelectorAll(".hide");
const removeElements = (elms) => elms.forEach(el => el.remove());
removeElements(demo);

for (let i = 0; i < rows.length; i++) {
    const tr = rows[i];
    let obj = {}
    for (let j = 0; j < tr.cells.length; j++) {
        const td = tr.cells[j];
        let value = td.innerText;
        let lengthString = -1;
        let multipleNumber = 1;
        const checkNumber = parseInt(value);
        if (!isNaN(checkNumber)) {
            if (value.indexOf(",") === -1) {
                multipleNumber = 1;
                lengthString = 0;
                if (value.indexOf(".") !== -1) {
                    let splitString = value.split(".");
                    lengthString = splitString[1].length;
                } else {
                    lengthString = value.length;
                }

                for (let k = 0; k < lengthString; k++) {
                    multipleNumber *= 10;
                }
                value = Math.round(value * multipleNumber);
            } else {
                value = value.toString().replace(/(,)/g, "");
            }
        }
        obj[arr[j]] = value.toString();
    }
    object.data.push(obj);
}

console.log(object.data);

// let oData = object.data;
// for (let d = 0; d < oData.length; d++) {
//     console.log("update table1 set Re=" + oData[d]['Re'] + ", Ce=" + oData[d]['Ce'] + ", Fl=" + oData[d]['Fl'] + " where Co='" + oData[d]['Co'] + "'");
// }