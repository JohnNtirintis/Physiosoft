function displayError(message, errorElementId) {
    const errorDiv = document.getElementById(errorElementId);
    if (errorDiv) {
        errorDiv.innerText = message;
    }
}

document.getElementById('PhysioID').addEventListener('change', function () {
    console.log("physio called");
    fetch('/Physios/GetPhysioLastName?id=' + this.value)
        .then(response => {
            if (response.ok) {
                return response.text();
            } else {
                throw new Error('Network response was NOT ok.');
            }
        })
        .then(data => {
            if (data) {
                document.getElementById('physioLastName').value = data;
            } else {
                throw new Error('No data returned');
            }
        })
        .catch(error => {
            displayError('Error fetching physio last name.', 'physioError');
        });
});

document.getElementById('PatientID').addEventListener('change', function () {
    fetch('/Patients/GetPatientLastName?id=' + this.value)
        .then(response => {
            if (response.ok) {
                return response.text(); 
            } else {
                throw new Error('Network response was NOT ok.');
            }
        })
        .then(data => {
            if (data) {
                document.getElementById('patientLastName').value = data;
            } else {
                throw new Error('No data returned');
            }
        })
        .catch(error => {
            displayError('Error fetching patient last name.', 'patientError');
        });
});

// Get names of preselected physio and patient
document.getElementById('PhysioID').dispatchEvent(new Event('change'));
document.getElementById('PatientID').dispatchEvent(new Event('change'));