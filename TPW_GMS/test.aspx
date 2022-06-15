<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="test.aspx.cs" Inherits="TPW_GMS.test" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <div id="app">
       <div>
           <input type="file" multiple="" @change="onFileSelected" />
           <button @click="onUpload">Upload</button>
       </div>
   </div>
   <script>
       new Vue({
           el: "#app",
           data: {

           },
           methods: {
               onFileSelected(event) {
                    this.selectedFile=event.target.files
               },
               onUpload() {
                   const fd = new FormData();
                   $.each(this.selectedFile, (key, value) => {
                       fd.append('image', value, value.name)
                   })
                   fd.append('data','apple')

                   axios.post('api/UploadImageTest', fd)
                       .then(res => {
                           console.log(res)
                       });
               }
           }
       })
   </script>
</asp:Content>
