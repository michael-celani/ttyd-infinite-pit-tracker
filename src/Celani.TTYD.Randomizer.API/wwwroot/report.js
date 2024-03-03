const inputElement = document.getElementById("input");
inputElement.addEventListener("change", handleFiles, false);

function handleFiles() {
  const fileList = this.files;

  if (fileList.length == 0) {
    return;
  }

  let fileReader = new FileReader();
  let dataView = new DataView();

  fileReader.onload = function (e) {
    console.log(fileReader.result);
  }

  fileReader.readAsText(fileList[0]);
}
