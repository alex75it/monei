
var monei = monei || (function(){

	// private properties and methods
	var _sessionToken = null;
	var _accountId = null;

	var Constants = {
		DROPDOWN_SIZE: 10,
		DEFAULT_CURRENCY_DECIMALS: 2 // number of decimals for currency representation
	};

	// public properties and methods
	return {

		debug: function(data){
			var type_ = typeof data;
			var message = "???";
			if (type_ === "String" || type_ === "number") {
				message = data + " (type: " + type_ + ")";
			}
			else if (typeof (data) === "object") {
				message = "(" + type_ + ") "
				for (p in data)
					message += "\r\n" + p + ": " + (typeof (data[p]) === 'function' ? '(function)' : data[p]);
			}
			else {
				message = "(type: " + type_ + ") " + data;
			}

			noty({ text: message, type: "information", layout: "topCenter", timeout: 5 * 1000 });
		},
		
		setSessionToken: function (sessionToken) {
			_sessionToken = sessionToken;
		},

		setAccountId: function (accountId) {
			_accountId = accountId;
		},

		// proerties
		properties: {
			currencyDecimals: Constants.DEFAULT_CURRENCY_DECIMALS
		},

		// initialize
		initialize: function (values) {
			properties.currencyDecimals = values.currencyDecimals || properties.currencyDecimals
		},

		setSelectpicker: function (select) {
			//$(select).selectpicker({ style: "btn", size: Constants.DROPDOWN_SIZE });
			// not work with ajax filled select
		},

		setFieldCalculationResult: function (jquerySelector) {
			var field = $(jquerySelector);
			var expression_result = eval(field.val());
			field.val(expression_result.toFixed(this.properties.currencyDecimals));
		},
		
		loadCategories: function (select, selectedCategory, onLoadComplete, onSelectChangeCallback) {
			
			var $categorySelect = $(select); // typeof(container) === 'string' ? $(container) : container;

			if (typeof (selectedCategory) === 'undefined')
				selectedCategory = '';

			if (typeof (onSelectChangeCallback) === 'function')
				$categorySelect.change(onSelectChangeCallback);

			$categorySelect.empty();

			var url = '/Registry/GetCategories';

			$.ajax({
				url: url,
				type: 'GET',
				success: function (data) {
					for (var i in data) {
						var category = data[i];
						$categorySelect.append($('<option value="' + category.Id + '">' + category.Name + '</option>'));
					}
				 
					if (selectedCategory)
						$categorySelect.val(selectedCategory);
					else 
						$categorySelect.find('option:first').select();                    

					typeof onLoadComplete === "function" && onLoadComplete();

					monei.setSelectpicker($categorySelect);
				}   
			});

		},

		loadSubcategories: function (subcategorySelect, categoryId, selectedSubcategory, onLoadComplete) {
			var $subcategorySelect = $(subcategorySelect);
			$subcategorySelect.empty();

			if (typeof (selectedSubcategory) === 'undefined')
				selectedSubcategory = '';

			// todo: make call to API
			var url = '/Registry/GetSubcategories';

			if (categoryId && categoryId !== '') {
				
				$.ajax({
					url: url,
					data: { categoryId: categoryId },
					type: 'GET',
					success: function (data) {
						for (var i in data) {
							var subcategory = data[i];
							$subcategorySelect.append($('<option value="' + subcategory.Id + '">' + subcategory.Name + '</option>'));
						}
						if (selectedSubcategory)
							$subcategorySelect.val(selectedSubcategory);
						else 
							$subcategorySelect.find('option:first').select();
						
						monei.setSelectpicker($subcategorySelect);

						typeof onLoadComplete === "function" && onLoadComplete();                        
					}
				});
			}

		}

	}
})();