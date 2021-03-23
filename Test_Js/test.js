let array = [
    "AAA",
    "16.7",
    "17.85",
    "15.55",
    "16.7",
    "116,700",
    "16.75",
    // "40,200",
    // "16.8",
    // "7,100",
    // "16.8",
    // "200",
    // "0.1",
    // "16.85",
    // "35,600",
    // "16.9",
    // "45,000",
    "16.95",
    // "93,300",
    // "1,676,800",
    // "16.7",
    "17",
    // "16.6",
    // "45,500",
    "500"
];
let obj = {};
let lengthString = -1;
let multipleNumber = 1;
for (let i = 0; i < array.length; i++) {
    const checkNumber = parseInt(array[i]);
    if (!isNaN(checkNumber)) {
        if (array[i].indexOf(",") === -1) {
            multipleNumber = 1;
            lengthString = 0;
            if (array[i].indexOf(".") !== -1) {
                let splitString = array[i].split(".");
                lengthString = splitString[1].length;
            } else {
                lengthString = array[i].length;
            }
            for (let k = 0; k < lengthString; k++) {
                multipleNumber *= 10;
            }
            array[i] = Math.round(array[i] * multipleNumber);

        } else {
            array[i] = array[i].toString().replace(/(,)/g, "");
        }
    }

    obj[i] = array[i].toString();
}

console.log(obj);