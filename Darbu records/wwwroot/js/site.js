// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function priartinimas(nuotrauka) {
    nuotrauka.classList.toggle("priartinimas");
}



//Nuotrauku ikelimas -------------------------------------------------------------------
//si funkcija pasileidzia tik tada kai visi DOM elementai uzkrauti
document.addEventListener("DOMContentLoaded",function(){
    const dropZone = document.getElementById('drop-zone');
    photosinput = document.getElementById('photosInput');
    dropZone.addEventListener('dragover', (e) => {
        e.preventDefault();
        dropZone.classList.add('dragover');
    });

    dropZone.addEventListener('dragleave', () => {
        dropZone.classList.remove('dragover');
    });

    dropZone.addEventListener('click', () => {
       photosinput.click();
    });


    dropZone.addEventListener('drop', (e) => {
        e.preventDefault();
        dropZone.classList.remove('dragover');
        dropZone.innerHTML = '';
        const transfer = new DataTransfer();
        const file = e.dataTransfer.files;
        //perreinam per visa masyva ir atvaizduojame ji dropzone lauke
        for (var i = 0; i < file.length; i++) {
            if (file[i].type.includes('image')) {

                const reader = new FileReader();
                reader.onload = () => {
                    const img = new Image(120, 150);
                    img.src = reader.result;
                    dropZone.appendChild(img);

                }
                reader.readAsDataURL(file[i]);
                transfer.items.add(file[i]);
            } else {
                const reader = new FileReader();
                reader.onload = () => {
                    const otherFile = new Image(120, 150);
                    otherFile.src = "/assets/logo/download.jpg";
                    dropZone.appendChild(otherFile);
                }
                reader.readAsDataURL(file[i]);
                transfer.items.add(file[i]);
            }


        }
        //pridedam i input file
        photosinput.files = transfer.files;


    });

    const fileInput = document.getElementById('photosInput');

    fileInput.addEventListener('change', (e) => {
        dropZone.innerHTML = '';
        //sukuriam file kintamaji ir patalpiniame e objekto turini
        var file = e.target.files;
        // alert(file.length);

        //perreinam per visa masyva ir atvaizduojame ji dropzone lauke
        for (var i = 0; i < file.length; i++) {
            if (file[i].type.includes('image')) {

                const reader = new FileReader();
                reader.onload = () => {
                    const img = new Image(120, 150);
                    img.src = reader.result;
                    dropZone.appendChild(img);

                }
                reader.readAsDataURL(file[i]);
            } else {
                const reader = new FileReader();
                reader.onload = () => {
                    const otherFile = new Image(120, 150);
                    otherFile.src = "/assets/logo/download.jpg";
                    dropZone.appendChild(otherFile);
                }
                reader.readAsDataURL(file[i]);
            }


        }


    });

//// Sukurti įvykio klausytoją įklijavimui
//document.addEventListener('paste', function (event) {
//    const items = (event.clipboardData || event.originalEvent.clipboardData).items;

//    for (let item of items) {
//        if (item.kind === 'file' && item.type.startsWith('image/')) {
//            const file = item.getAsFile();
//            handleFile(file);
//        }
//    }
//});

   

});






//////////////////////////////////////////////////////////////////
//vieno failo ikelimas---------------------------------------
document.addEventListener("DOMContentLoaded",function() {
    const dropZone_file = document.getElementById('drop-zone-file');
    const fileInput = document.getElementById('fileInput');

    dropZone_file.addEventListener('dragover', (e) => {
        e.preventDefault();
        dropZone_file.classList.add('dragover');
    });

    dropZone_file.addEventListener('dragleave', () => {
        dropZone_file.classList.remove('dragover');
    });

    dropZone_file.addEventListener('click', () => {
        fileInput.click();
    });

    dropZone_file.addEventListener('drop', (e) => {
        e.preventDefault();
        dropZone_file.classList.remove('dragover');
        dropZone_file.innerHTML = '';

        const file = e.dataTransfer.files[0]; // Only handle the first file
        const transfer_file = new DataTransfer();

        if (file !=null) {
            const reader_file = new FileReader();
            reader_file.onload = () => {
                const img_file = new Image(120, 150);
                /*img_file.src = reader_file.result;*/
                img_file.src = "/assets/logo/download.jpg";
                dropZone_file.appendChild(img_file);
            }
            reader_file.readAsDataURL(file);
            transfer_file.items.add(file);
        } else {
            alert('Ikeltas ne img failas');
        }

        // Add the file to the input element
        fileInput.files = transfer_file.files;
    });

    fileInput.addEventListener('change', (e) => {
        dropZone_file.innerHTML = '';
        const file = e.target.files[0]; //cia gaunam url

        if (file!=null) {
            const reader_file = new FileReader();
            reader_file.onload = () => {
                const img_file = new Image(120, 150);
                img_file.src = "/assets/logo/download.jpg";
                dropZone_file.appendChild(img_file);
            }
            reader_file.readAsDataURL(file);
        } else {
            alert('Ikeltas ne img failas');
        }
    });
    //TEXTAREA AUTO SIZE///////////////////
    const TextAreaResize = document.getElementById("TextAreaResize");

    TextAreaResize.addEventListener('input', function () {

        this.style.height = 'auto';
        this.style.height = this.scrollHeight + 'px';



    });
});

///////////////////////////////////////////

//PAIESKA

function filterCategories() {
    let input = document.getElementById("categorySearch").value.toLowerCase();
    let cards = document.querySelectorAll(".category-card");

    cards.forEach(function (card) {
        let name = card.getAttribute("data-name");
        if (name.includes(input)) {
            card.style.display = "";
        } else {
            card.style.display = "none";
        }
    });
}


////////////////////////////
//DINAMINIS TEXTAREA PADIDINIMAS

document.addEventListener("DOMContentLoaded", function () {
    const textarea = document.getElementById("dinamycTextarea");
    if (!textarea) return; // saugiklis – jei elemento nėra, kodas nebus vykdomas

    // Auto-resize eventas
    textarea.addEventListener("input", function () {
        textAreaAdjust(this);
    });

    // Funkcija, kuri padidina textarea pagal turinį
    function textAreaAdjust(element) {
        element.style.height = "auto"; // atstatome aukštį
        element.style.height = element.scrollHeight + "px"; // nustatome pagal turinį
    }
});
///////////////////////////


////////////////////////////////////
//DINAMINIS KATEGORIJU ATVAIZDAVIMAS
document.addEventListener("DOMContentLoaded", function () {
    const groupSelect = document.getElementById("groupSelect");
    const categorySelect = document.getElementById("categorySelect");

    if (!groupSelect || !categorySelect) {
       
        return;
    }

    function filterCategories(groupId) {
        for (let option of categorySelect.options) {
            option.hidden = (option.dataset.group !== groupId);
        }
        let visible = Array.from(categorySelect.options).find(o => !o.hidden);
        if (visible) categorySelect.value = visible.value;
    }

    groupSelect.addEventListener("change", function () {
        filterCategories(this.value);
    });

    filterCategories(groupSelect.value);
});

/////////////////////////////////////////////////
////////
document.addEventListener("DOMContentLoaded", function () {
// Poll objektas
const poll = {
    title: "Koks tavo mėgstamiausias vaisius?",
    description: "Pasirink vieną variantą:",
    checkboxlist: ["Obuolys", "Bananas", "Apelsinas", "Vynuogės"]
};

// Užpildome poll lentele
document.getElementById("pollTitle").innerText = poll.title;
document.getElementById("pollDescription").innerText = poll.description;

const form = document.getElementById("pollForm");
poll.checkboxlist.forEach((item, index) => {
    const div = document.createElement("div");
    div.classList.add("form-check");
    div.innerHTML = `
            <input class="form-check-input" type="radio" name="pollOption" id="option${index}" value="${item}">
            <label class="form-check-label" for="option${index}">${item}</label>
        `;
    form.appendChild(div);
});

// Balsavimo funkcija
function submitPoll() {
    const selected = document.querySelector('input[name="pollOption"]:checked');
    if (!selected) {
        alert("Pasirink bent vieną variantą!");
    } else {
        alert("Pasirinkai: " + selected.value);
    }
    }




   





});
///////