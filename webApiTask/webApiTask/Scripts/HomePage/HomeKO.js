function ViewModel() {

    var self = this;
    self.toDoLists = ko.observableArray();
    var tagsForAutoComplete = [];


    //NEW OBJECTS

    //New Item
    var newItem = function (data) {
        var m = ko.mapping.fromJS(data, itemMapping);

        m.Text.subscribe(function (newValue) {
            //send ajax
            self.SendItemText(m.Id(), newValue);
        });
        m.IsCompleted.subscribe(function (newValue) {
            //send ajax
            self.SendStatus(m.Id(), newValue);
        });
        return m;
    }

    //Create new Tag
    var newTag = function (data) {
        var m = ko.mapping.fromJS(data, tagMapping);
        return m;
    }

    //Create new list
    var newList = function (data) {
        var m = ko.mapping.fromJS(data, listMapping);

        var tags = [];
        $.each(m.Tags(), function (index, element) {
            tags.push(element.Name());
        });

        m.bindTagsEditor = function (elements) {

            $.each(elements, function (index, elem) {
                if (elem.nodeName == "INPUT") {

                    $(elem).tagEditor({
                        initialTags: tags,
                        placeholder: "Enter tags ...",
                        autocomplete: {
                            delay: 0,
                            position: { collision: "flip" },
                            source: tagsForAutoComplete
                        },
                        beforeTagDelete: function (field, editor, tags, val) {
                            self.removeTag(val, m.Id());
                        },
                        beforeTagSave: function (field, editor, tags, tag, val) {
                            self.addTag(val, m.Id());
                        }
                    });
                }
            });
        }

        m.findTag = function () {
            $(".tag-editor-tag").each(function () {
                $(this).click(function () {
                    var tagName = $(this).html();

                    var token = appContext.token;
                    $.ajax({
                        type: "GET",
                        url: appContext.buildUrl("/api/lists/GetByName/" + tagName),
                        beforeSend: function (xhr) { xhr.setRequestHeader("Authorization", "Bearer " + token); },
                        dataType: "json",
                        success: function (data) {
                            self.toDoLists.removeAll();
                            $.each(data, function (index, element) {
                                var listModel = newList(element);
                                self.toDoLists.push(listModel);

                                $(".tagName").empty();
                                $(".tagName").append("#" + tagName + "<span onclick = 'vm.RefreshLists();' style='cursor:pointer; font-size=20px;'><i></i></span>");
                            });
                        }
                    });
                });
            });
        }

        m.sortedItems = ko.pureComputed(function () {
            var k = m.Items();
            var incompleted = [];
            var completed = [];
            for (var i = 0; i < k.length; i++) {
                if (k[i].IsCompleted())
                    completed.push(k[i]);
                else
                    incompleted.push(k[i]);
            }
            return incompleted.concat(completed);
        }, m);
        return m;
    }

    //mappings
    var itemMapping = {
        'ignore': ["Created", "Modified", "Priority", "NextNotifyTime", "ToDoList_Id"]
    }

    var tagMapping = {
        'ignore': ["Created", "Modified"]
    }

    var listMapping = {
        'ignore': ["Created", "Modified", "User", "User_Id"],

        "Items": {
            create: function (options) {
                var m = newItem(options.data);
                return m;
            }
        },
        'Name': {
            create: function (options) {

                var m = ko.mapping.fromJS(options.data);

                m.subscribe(function (newValue) {
                    self.SendListName(options.parent.Id(), newValue);
                });

                return m;
            }
        },
        "Tags": {
            create: function (options) {
                var m = newTag(options.data);
                return m;
            }
        }

    }



    //AJAX REQUESTS

    //Load ToDoLists
    self.loadLists = function () {
        $.ajax({
            type: "GET",
            contentType: "application/json",
            url: appContext.buildUrl("/api/lists"),
            beforeSend: function (xhr) { xhr.setRequestHeader("Authorization", "Bearer " + appContext.token); },
            dataType: "JSON",
            success: function (data) {

                if (data.message != null) {
                    return;
                }

                self.toDoLists.removeAll();

                $.each(data, function (index, element) {

                    var listModel = newList(element);

                    self.toDoLists.push(listModel);
                });
            }
        });

    }

    //Change list name
    self.SendListName = function (id, value) {
        $.ajax({
            type: "PUT",
            url: appContext.buildUrl("/api/lists/changeListName/" + id + "/" + value),
            beforeSend: function (xhr) { xhr.setRequestHeader("Authorization", "Bearer " + appContext.token); },
            dataType: "json"
        });
    }

    //Add list
    self.AddList = function () {
        var data =
           {
               'Name': "newList",
               'Items': [
                   {
                       'Text': "newItem",
                       'IsCompleted': false
                   }
               ]
           };

        $.ajax({
            type: "POST",
            url: appContext.buildUrl("/api/ToDoList"),
            beforeSend: function (xhr) { xhr.setRequestHeader("Authorization", "Bearer " + appContext.token); },
            dataType: "json",
            data: data,
            success: function (list) {
                var model = newList(list);
                self.toDoLists.push(model);
            }
        });
    }

    //Remove list
    self.removeList = function (list) {
        self.toDoLists.remove(list);
        var id = list.Id();
        $.ajax({
            type: "Delete",
            beforeSend: function (xhr) { xhr.setRequestHeader("Authorization", "Bearer " + appContext.token); },
            url: appContext.buildUrl("/api/lists/Delete/") + id,
            dataType: "json"
        });
    }



    //Change status of item(IsCompleted)
    self.SendStatus = function (id, value) {
        $.ajax({
            type: "PUT",
            url: appContext.buildUrl("/api/items/SetStatus/") + id + "/" + value,
            beforeSend: function (xhr) { xhr.setRequestHeader("Authorization", "Bearer " + appContext.token); },
            dataType: "json"
        });
    }
    //Change item text
    self.SendItemText = function (id, value) {
        $.ajax({
            type: "PUT",
            url: appContext.buildUrl("/api/items/SetText/") + id + "/" + value,
            beforeSend: function (xhr) { xhr.setRequestHeader("Authorization", "Bearer " + appContext.token); },
            dataType: "json"
        });
    }

    //Add item
    self.AddItem = function (data) {
        var listId = data.Id
        var length = data.Items().length;
        if (length <= 4) {

            $.ajax({
                type: "POST",
                url: appContext.buildUrl("/api/Item"),
                dataType: "json",
                beforeSend: function (xhr) { xhr.setRequestHeader("Authorization", "Bearer " + appContext.token); },
                data: { "ToDoList_Id": listId, "Text": "newItem", "IsCompleted": false },

                success: function (item) {
                    var m = newItem(item);
                    data.Items.unshift(m);
                }
            });
            return data;
        }
        return data;
    }

    //Remove item
    self.removeItem = function (data, item) {
        data.Items.remove(item);

        var id = item.Id();
        $.ajax({
            type: "Delete",
            url: appContext.buildUrl("/api/Item/") + id,
            beforeSend: function (xhr) { xhr.setRequestHeader("Authorization", "Bearer " + appContext.token); },
            dataType: "json"
        });
    }



    //Load tags for autocomplete
    self.GetAllTags = function () {
        $.ajax({
            type: "GET",
            beforeSend: function (xhr) { xhr.setRequestHeader("Authorization", "Bearer " + appContext.token); },
            url: appContext.buildUrl("/api/Tag"),
            dataType: "json",
            success: function (data) {
                $.each(data, function (index, element) {
                    tagsForAutoComplete.push(element.Name);
                });
            }
        });
    }

    //Re-load lists
    self.RefreshLists = function () {
        self.loadLists();
        $(".tagName").empty();
    }

    //Remove tag
    self.removeTag = function (value, listId) {

        $.ajax({
            type: "Delete",
            url: appContext.buildUrl("/api/tags/removeTag/") + value + "/" + listId,
            beforeSend: function (xhr) { xhr.setRequestHeader("Authorization", "Bearer " + appContext.token); },
            dataType: "json"
        });
    }

    //Add tag
    self.addTag = function (value, listId) {
        $.ajax({
            type: "POST",
            url: appContext.buildUrl("/api/tags/addTag/") + value + "/" + listId,
            dataType: "json",
            beforeSend: function (xhr) { xhr.setRequestHeader("Authorization", "Bearer " + appContext.token); }
        });
    }


    //bind Notification modal window
    self.bindNotification = function (item, e) {
        var itemId = item.Id();

        if (item.IsNotify()) {
            $.ajax({
                type: "PUT",
                contentType: "application/json",
                url: appContext.buildUrl("/api/items/DismissNotify/") + itemId,
                beforeSend: function (xhr) { xhr.setRequestHeader("Authorization", "Bearer " + appContext.token); },
                dataType: "JSON"
            });
            item.IsNotify(false);
            e.preventDefault();
            e.stopPropagation();
            return;
        }

        var element = $(".modal-content")[0];
        ko.cleanNode(element);
        item.picker = function () {
            $("#datetime24").combodate({ maxYear: 2020 });
        }

        //Send notification to server
        item.SubmitNotification = function () {
            var time = $("#datetime24").combodate("getValue", null);
            var data = JSON.stringify({ ItemId: itemId, Date: time });
            $.ajax({
                type: "PUT",
                contentType: "application/json",
                url: appContext.buildUrl("/api/items/SubmitNotify"),
                beforeSend: function (xhr) { xhr.setRequestHeader("Authorization", "Bearer " + appContext.token); },
                dataType: "JSON",
                data: data
            });
            item.IsNotify(true);
        }

        ko.applyBindings(item, element);
    }

    //methods for googleMaps

    self.GetSelectedList = function () {
        var result = null;
        $.each($(".list"), function (index, element) {

            if (element.classList.contains("selected")) {
                
                result = element;
                return false;
            }
        });
        return result;
    }

    self.GetListById = function (listId) {
       
        return list;
    }


    //Custom ko binding "inline"
    ko.bindingHandlers.inline = {
        init: function (element, valueAccessor) {
            var span = $(element);
            var input = $("<input />", { 'type': "text", 'style': "display:none" });
            span.after(input);

            ko.applyBindingsToNode(input.get(0), { value: valueAccessor() });
            ko.applyBindingsToNode(span.get(0), { text: valueAccessor() });

            span.click(function () {
                input.width(span.width());
                span.hide();
                input.show();
                input.focus();
            });

            input.blur(function () {
                span.show();
                input.hide();
            });

            input.keypress(function (e) {
                if (e.keyCode == 13) {
                    span.show();
                    input.hide();
                }
            });
        }
    }

}



var vm = new ViewModel();

$(function () {

    vm.loadLists();
    vm.GetAllTags();
    ko.applyBindings(vm, $(".notes-board")[0]);
});



