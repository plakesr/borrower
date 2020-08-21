import "../scss/main.scss";

import "jquery";
import "bootstrap";
import "jquery-validation";
import "jquery-validation-unobtrusive";

import initDataTables from 'components/data-tables'
import initDropdowns from 'components/dropdown'
import initAlerts from 'components/alert-message'
import initRadio from 'components/radio'
import initDatePickers from 'components/date-pickers'
import initAutoNumeric from 'components/auto-numeric'
import initForms from 'components/forms'

import initSubmitPayment from './pages/submit-payment'

import './validators/validator'

document.addEventListener("DOMContentLoaded", function () {
    initDataTables();
    initDropdowns();
    initAlerts();
    initRadio();
    initDatePickers();
    initAutoNumeric();
    initForms();

    initSubmitPayment();
});
