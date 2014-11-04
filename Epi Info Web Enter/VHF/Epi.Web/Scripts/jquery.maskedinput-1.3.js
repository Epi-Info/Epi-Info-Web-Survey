/*
	Masked Input plugin for jQuery
	Copyright (c) 2007-2011 Josh Bush (digitalbush.com)
	Licensed under the MIT license (http://digitalbush.com/projects/masked-input-plugin/#license) 
	Version: 1.3
*/
(function ($) {
    var pasteEventName = ($.browser.msie ? 'paste' : 'input') + ".mask";
    var iPhone = (window.orientation != undefined);

    $.mask = {
        //Predefined character definitions
        definitions: {
            '9': "[0-9]",
            'a': "[A-Za-z]",
            '*': "[A-Za-z0-9]"
        },
        dataName: "rawMaskFn"
    };

    $.fn.extend({
        //Helper Function for Caret positioning
        caret: function (begin, end) {
            if (this.length == 0) return;
            if (typeof begin == 'number') {
                end = (typeof end == 'number') ? end : begin;
                return this.each(function () {
                    if (this.setSelectionRange) {
                        this.setSelectionRange(begin, end);
                    } else if (this.createTextRange) {
                        var range = this.createTextRange();
                        range.collapse(true);
                        range.moveEnd('character', end);
                        range.moveStart('character', begin);
                        range.select();
                    }
                });
            } else {
                if (this[0].setSelectionRange) {
                    begin = this[0].selectionStart;
                    end = this[0].selectionEnd;
                } else if (document.selection && document.selection.createRange) {
                    var range = document.selection.createRange();
                    begin = 0 - range.duplicate().moveStart('character', -100000);
                    end = begin + range.text.length;
                }
                return { begin: begin, end: end };
            }
        },
        unmask: function () { return this.trigger("unmask"); },
        mask: function (mask, settings) {
            if (!mask && this.length > 0) {
                var input = $(this[0]);
                return input.data($.mask.dataName);
            }
            settings = $.extend({
                placeholder: "_",
                completed: null
            }, settings);

            var defs = $.mask.definitions;
            var tests = [];
            var partialPosition = mask.length;
            var firstNonMaskPos = null;
            var len = mask.length;

            $.each(mask.split(""), function (i, c) {
                if (c == '?') {
                    len--;
                    partialPosition = i;
                } else if (defs[c]) {
                    tests.push(new RegExp(defs[c]));
                    if (firstNonMaskPos == null)
                        firstNonMaskPos = tests.length - 1;
                } else {
                    tests.push(null);
                }
            });

            return this.trigger("unmask").each(function () {
                var input = $(this);
                var buffer = $.map(mask.split(""), function (c, i) { if (c != '?') return defs[c] ? settings.placeholder : c });
                var focusText = input.val();

                function seekNext(pos) {
                    while (++pos <= len && !tests[pos]);
                    return pos;
                };
                function seekPrev(pos) {
                    while (--pos >= 0 && !tests[pos]);
                    return pos;
                };

                function shiftL(begin, end) {
                    if (begin < 0)
                        return;
                    for (var i = begin, j = seekNext(end); i < len; i++) {
                        if (tests[i]) {
                            if (j < len && tests[i].test(buffer[j])) {
                                buffer[i] = buffer[j];
                                buffer[j] = settings.placeholder;
                            } else
                                break;
                            j = seekNext(j);
                        }
                    }
                    writeBuffer();
                    input.caret(Math.max(firstNonMaskPos, begin));
                };

                function shiftR(pos) {
                    for (var i = pos, c = settings.placeholder; i < len; i++) {
                        if (tests[i]) {
                            var j = seekNext(i);
                            var t = buffer[i];
                            buffer[i] = c;
                            if (j < len && tests[j].test(t))
                                c = t;
                            else
                                break;
                        }
                    }
                };

                function keydownEvent(e) {
                    var k = e.which;

                    //backspace, delete, and escape get special treatment
                    if (k == 8 || k == 46 || (iPhone && k == 127)) {
                        var pos = input.caret(),
							begin = pos.begin,
							end = pos.end;

                        if (end - begin == 0) {
                            begin = k != 46 ? seekPrev(begin) : (end = seekNext(begin - 1));
                            end = k == 46 ? seekNext(end) : end;
                        }
                        clearBuffer(begin, end);
                        shiftL(begin, end - 1);

                        return false;
                    } else if (k == 27) {//escape
                        input.val(focusText);
                        input.caret(0, checkVal());
                        return false;
                    }
                };

                function keypressEvent(e) {
                    var k = e.which,
						pos = input.caret();
                    if (e.ctrlKey || e.altKey || e.metaKey || k < 32) {//Ignore
                        return true;
                    } else if (k) {
                        if (pos.end - pos.begin != 0) {
                            clearBuffer(pos.begin, pos.end);
                            shiftL(pos.begin, pos.end - 1);
                        }

                        var p = seekNext(pos.begin - 1);
                        if (p < len) {
                            var c = String.fromCharCode(k);
                            if (tests[p].test(c)) {
                                shiftR(p);
                                buffer[p] = c;
                                writeBuffer();
                                var next = seekNext(p);
                                input.caret(next);
                                if (settings.completed && next >= len)
                                    settings.completed.call(input);
                            }
                        }
                        return false;
                    }
                };

                function clearBuffer(start, end) {
                    for (var i = start; i < end && i < len; i++) {
                        if (tests[i])
                            buffer[i] = settings.placeholder;
                    }
                };

                function writeBuffer() { return input.val(buffer.join('')).val(); };



                function checkVal(allow) {
                    //try to place characters where they belong
                    var test = input.val();
                    var lastMatch = -1;
                    for (var i = 0, pos = 0; i < len; i++) {
                        if (tests[i]) {
                            buffer[i] = settings.placeholder;
                            while (pos++ < test.length) {
                                var c = test.charAt(pos - 1);
                                if (tests[i].test(c)) {
                                    buffer[i] = c;
                                    lastMatch = i;
                                    break;
                                }
                            }
                            if (pos > test.length)
                                break;
                        } else if (buffer[i] == test.charAt(pos) && i != partialPosition) {
                            pos++;
                            lastMatch = i;
                        }
                    }
                    if (!allow && lastMatch + 1 < partialPosition) {
                        /******************************Customised the downloaded plugin******************************************************************************/
                        /*if entering first time the field is empty (Integers only). Clear the buffer.*/
                        if (test.length == 0) { 
                            input.val("");
                            clearBuffer(0, len);
                        }
                        /*if key pressed on the field and left without putting any digit, means test contains '_' as character (decimal numbers), clear buffer
                        This logis is intended for decimal numbers, if clicked on the field and then click outside test will be something like (_ _._ _ _) if pattern
                        is '##.###'. So we are clearing the buffer
                        */
                        else if (test.indexOf("_") != -1) {
                            input.val("");
                            clearBuffer(0, len);
                        }
                        /*
                         if test contains character '.' clear buffer (for Decimals only). If we put some number inside the decimal patterned (e.g ##.###) numeric
                         text box and then deleted the digits the '.' will still be there, but we still want to clear the buffer. 
                        */
                        else if ((test.indexOf(".") != -1) && (test.length == 1)) {
                            input.val("");
                            clearBuffer(0, len);
                        }
                        else if (test.length > 0) {
                            /*if only integer*/
                            if (test.indexOf(".") == -1) { //if it is integer
                                positionNumber(len, buffer);
                                input.val(buffer.join('')).val();
                            } //end if of if it is integer
                            else {//if decimal
                                //put the number before digit in an array and return it
                                var arrayBeforeDecimal = []; //array to put numbers before decimal
                                var arrayAfterDecimal = [];//array to put numbers after decimal 
                                var lengthd = 0;

                                lengthd = buffer.indexOf(".");

                                //populate the array with digits before decimal point
                                for (var k = 0; k < lengthd; k++) {
                                    arrayBeforeDecimal[k] = buffer[k];
                                }
                                //populate the array with digits after decimal point
                                for (var l = lengthd + 1; l < len; l++) {
                                    if (buffer[l] == settings.placeholder) {
                                        arrayAfterDecimal[l] = "0";
                                    }
                                    else {
                                        arrayAfterDecimal[l] = buffer[l];
                                    }
                                }



                                var ldigit = arrayBeforeDecimal.length;
                                //process the ddigits before decimal. e.g if somebody puts 1 and hit tab for ##.### then
                                //the number should be formatted like 01.000
                                positionNumber(ldigit, arrayBeforeDecimal);

                                var digitsBeforeDecimal = arrayBeforeDecimal.join('');
                                var digitsAfterDecimal = arrayAfterDecimal.join('');
                                var decimalNumber = digitsBeforeDecimal + "." + digitsAfterDecimal;

                                input.val(decimalNumber).val();

                            }
                            // input.val(buffer.join('')).val();

                        }
                        /*clearBuffer(0, len);*/
                        /*******************************end of Customised the downloaded plugin***************************************************************************/


                    } else if (allow || lastMatch + 1 >= partialPosition) {
                        writeBuffer();
                        if (!allow) input.val(input.val().substring(0, lastMatch + 1));
                    }
                    return (partialPosition ? i : firstNonMaskPos);
                };


                /*Customized function written for plugin*/
                /*
                 The following function will position the digits in an integer or the digits before decimal point in decimal number
                 For example, for integer pattern '####' we put 4 and click outside the box. The buffer will look something like
                 buffer[0] = 4
                 buffer[1] = _
                 buffer[2] = _
                 buffer[3] = _
                 In the above case, first we will need to organize the buffer.
                 
                 buffer[0] = _
                 buffer[1] = _
                 buffer[2] = _
                 buffer[3] = 4

                 Then replace the buffer space with '_'(s) with '0'

                 buffer[0] = 0
                 buffer[1] = 0
                 buffer[2] = 0
                 buffer[3] = 4
                 
                 In the similar way for decimals we are taking the digits before decimal and process in similar fashion.   
                */
                function positionNumber(pLength, pBuffer) {
                    var noOfDigit = 0;
                    var noOfNonDigit = 0;
                    var digitHolder = [];
                    /*count the number of digit and the numbers in a seperate array*/
                    for (var i = 0; i < pLength; i++) {
                        if (pBuffer[i] != settings.placeholder) {
                            noOfDigit++;
                            digitHolder[i] = pBuffer[i];
                        }
                    }
                    /*count the number of non-digits represented here as '_' */
                    for (var i = 0; i < pLength; i++) {
                        if (pBuffer[i] == settings.placeholder)
                            noOfNonDigit++;
                    }
                    /*populating buffer array, the beginning spaces for non-digits('_') with '0'*/
                    for (var i = 0; i < noOfNonDigit; i++) {
                        pBuffer[i] = "0";
                    }
                    /*populating the rest of buffer array spaces with digits*/
                    var j = 0;
                    for (var i = noOfNonDigit; i < pLength; i++) {
                        while (j < noOfDigit) {
                            pBuffer[i] = digitHolder[j];
                            j = j + 1;
                            break;
                        }
                    }
                }
                /* End of Customized function written for plugin*/

                input.data($.mask.dataName, function () {
                    return $.map(buffer, function (c, i) {
                        return tests[i] && c != settings.placeholder ? c : null;
                    }).join('');
                })

                if (!input.attr("readonly"))
                    input
					.one("unmask", function () {
					    input
							.unbind(".mask")
							.removeData($.mask.dataName);
					})
					.bind("focus.mask", function () {
					    focusText = input.val();
					    var pos = checkVal();
					    writeBuffer();
					    var moveCaret = function () {
					        if (pos == mask.length)
					            input.caret(0, pos);
					        else
					            input.caret(pos);
					    };
					    ($.browser.msie ? moveCaret : function () { setTimeout(moveCaret, 0) })();
					})
					.bind("blur.mask", function () {
					    checkVal();
					    if (input.val() != focusText)
					        input.change();
					})
					.bind("keydown.mask", keydownEvent)
					.bind("keypress.mask", keypressEvent)
					.bind(pasteEventName, function () {
					    setTimeout(function () { input.caret(checkVal(true)); }, 0);
					});

                checkVal(); //Perform initial check for existing values
            });
        }
    });
})(jQuery);
