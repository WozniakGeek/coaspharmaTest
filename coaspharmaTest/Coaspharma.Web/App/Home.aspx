<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="Coaspharma.Web.App.Home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="page-header">
        <h1>!Bienvenido <%= Session["Name"]%>!</h1>
    </div>
    <br />

    <div class="panel panel-primary" runat="server">
        <div class="panel-heading">
            <span class="" aria-hidden="true"></span>Consulta            
        </div>

        <%--<center>
            <asp:Button ID="btnShowPopup" runat="server" Text="Show Popup" OnClick="ShowPopup"
                CssClass="btn btn-info btn-lg" />
        </center>--%>
        <div class="panel-body">
            <div class="row">
                <%-- <div class="col-md-2 col-md-offset-5">
                    <asp:LinkButton ID="Descarga" runat="server" CssClass="btn btn-success btn-block"><span class="glyphicon glyphicon-search" aria-hidden="true"></span> Consulta</asp:LinkButton>
                </div>--%>

                <div class="col-md-2 col-md-offset-3">
                    <asp:LinkButton ID="Insertar" runat="server" OnClick="ShowPopup" CssClass="btn btn-warning btn-block"><span class="glyphicon glyphicon-user" aria-hidden="true"></span> Insertar</asp:LinkButton>
                </div>

                <div class="col-md-2 col-md-offset-3">
                    <asp:LinkButton ID="DescargaReporte" OnClick="Descargar" runat="server" CssClass="btn btn-success btn-block"><span class="glyphicon glyphicon-download" aria-hidden="true"></span> Descargar</asp:LinkButton>
                </div>
            </div>

        </div>


        <div class="panel-heading">
            <span aria-hidden="true"></span>Resultados de búsqueda
        </div>
        <div class="panel-body">
            <div class="table-responsive">
                <asp:HiddenField ID="HiddenField2" runat="server" />
                <table id="tabla" class="table table-striped  table-hover table-condensed cell-border">
                    <thead>

                        <tr>
                            <th>Codigo</th>
                            <th>Cedula</th>
                            <th>Nombres</th>
                            <th>Apellidos</th>
                            <th>Fecha Ingreso</th>
                            <th>Departamento</th>
                            <th>Municipio</th>
                            <%--<th>Foto</th>--%>
                            <%--<th>Ubicacion</th>--%>
                            <th>Acciones</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="tablaValores" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td><%#Eval("codcia")%></td>
                                    <td><%#Eval("cedula")%></td>
                                    <td><%#Eval("nombre")%></td>
                                    <td><%#Eval("apellido")%></td>
                                    <td><%#Eval("fecha_ingreso")%></td>
                                    <td><%#Eval("dep_codi")%></td>
                                    <td><%#Eval("mun_codi")%></td>
                                    <td>

                                        <asp:LinkButton ID="btn_editar" SkinID="Editar" runat="server" Text="Editar" CssClass="btn btn-outline-secondary" OnClick="ShowPopup" CommandArgument='<%#Eval("cedula")%>'><i class="glyphicon glyphicon-edit"></i></asp:LinkButton>
                                        <asp:LinkButton ID="btn_delete" SkinID="Eliminar" runat="server" Text="" CssClass="btn btn-outline-secondary" OnClick="ShowPopup" CommandArgument='<%#Eval("cedula")%>'><i class="glyphicon glyphicon-remove-sign"></i></asp:LinkButton>

                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
            </div>
            <div class="table-responsive">
                <asp:GridView ID="tabla_ppal" runat="server" HorizontalAlign="Center" Visible="false" Style="text-align: center;" AutoGenerateColumns="false" CssClass="table table-striped  table-hover table-condensed cell-border">

                    <Columns>
                        <asp:BoundField DataField="codcia" HeaderText="CODIGO" />
                        <asp:BoundField DataField="cedula" HeaderText="CEDULA" />
                        <asp:BoundField DataField="nombre" HeaderText="NOMBRES" />
                        <asp:BoundField DataField="apellido" HeaderText="APELLIDOS" />
                        <asp:BoundField DataField="fecha_ingreso" HeaderText="FECHA DE INGRESO" />
                        <asp:BoundField DataField="dep_codi" HeaderText="DEPARTAMENTO" />
                        <asp:BoundField DataField="mun_codi" HeaderText="MUNICIPIO" />
                        <%--<asp:BoundField DataField="foto" HeaderText="foto" />--%>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>

    <asp:HiddenField ID="accionForm" runat="server" />

    <!-- Mensaje de Error -->
    <%--<div class="modal fade" id="modalError" role="dialog">
        <div class="alert alert-danger alert-dismissible fade in" style="margin: 20%;">
            <a href="javascript:;" class="close" data-dismiss="modal" aria-label="close">&times;</a>
            <strong>Alerta!</strong><br />
            <br />
            <asp:Label ID="lbl_mensaje_error" runat="server" Text=""></asp:Label>
        </div>
    </div>--%>

    <!-- Modal Popup -->
    <div id="MyPopup" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        &times;</button>
                    <h4 class="modal-title"></h4>
                </div>
                <div class="modal-body" id="BodyModal" runat="server">
                    <asp:HiddenField ID="HiddenField1" runat="server" />
                    <div class="row">
                        <div class="col-md-12">

                            <div class="form-group col-md-6">
                                <label for="nombre">Cod:</label>
                                <asp:DropDownList ID="ddlcodModal" runat="server" class="form-control"></asp:DropDownList>
                            </div>
                            <div class="form-group col-md-6">
                                <label for="nombre">Cedula:</label>
                                <asp:TextBox ID="CedulaModal" runat="server" class="form-control"></asp:TextBox>
                            </div>
                            <div class="form-group col-md-6">
                                <label for="apellido">Nombre:</label>
                                <asp:TextBox ID="NombreModal" runat="server" class="form-control"></asp:TextBox>
                            </div>
                            <div class="form-group col-md-6">
                                <label for="apellido">Apellido:</label>
                                <asp:TextBox ID="ApellidoModal" runat="server" class="form-control"></asp:TextBox>
                            </div>
                            <div class="form-group col-md-6">
                                <label for="apellido">Departamento:</label>
                                <asp:DropDownList ID="ddlDepaModal" runat="server" class="form-control"></asp:DropDownList>
                            </div>
                            <div class="form-group ">
                                <label for="apellido">Municipio:</label>
                                <asp:DropDownList ID="ddlMuniModal" runat="server" class="form-control"></asp:DropDownList>
                            </div>
                            <div class="form-group col-md-6">
                                <label for="apellido">Subir Foto:</label>
                                <asp:FileUpload ID="UplodadPhoto" accept=".png" runat="server" class="form-control" />
                            </div>
                            <div class="form-group col-md-6">
                                <label for="apellido">Adjuntar:</label>
                                <asp:FileUpload ID="Uploader" runat="server" CssClass="form-control" />
                            </div>
                        </div>
                        <div class="form-group col-md-6">
                            <asp:Image ID="imageurl" Width="200" runat="server" />
                        </div>
                        <%--<div class="form-group col-md-6">

                            <img src="data:image/png;base64,<%#D %>" id="PictureTest" runat="server" width="500" height="600">
                        </div>--%>
                    </div>
                </div>
                <div class="modal-footer">
                    <div class="row">
                        <div class="col-md-6 text-center">
                            <asp:LinkButton ID="lbtnUpdate" SkinID="Update" runat="server" OnClick="UpdateModal"
                                CssClass="btn btn-success"><i class="fa fa-save"></i> Actualizar
                            </asp:LinkButton>
                            <asp:LinkButton ID="lbtnInsert" SkinID="Insert" Visible="false" runat="server" OnClick="UpdateModal"
                                CssClass="btn btn-success"><i class="fa fa-save"></i> Guardar
                            </asp:LinkButton>
                            <%--<asp:LinkButton ID="lbtPreview" SkinID="Insert" Visible="false" runat="server" OnClick="ShowPhoto"
                                CssClass="btn btn-success"><i class="glyphicon glyphicon-eye-open"></i>                   
                            </asp:LinkButton>--%>
                        </div>
                        <div class="col-md-6 text-center">
                            <button type="button" class="btn btn-danger" data-dismiss="modal">
                                Cerrar</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div id="ModalEliminar" class="modal fade" role="dialog">
        <div class="modal-dialog modal-sm">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close navbar-custom" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Ticket </h4>
                </div>
                <div class="modal-body">
                    <asp:HiddenField ID="hdf_itemId" runat="server" />
                    <div class="row">

                        <div class="col-md-12">
                            <p>
                                <asp:Label ID="lbl_error" runat="server" Text=""></asp:Label>
                            </p>
                        </div>

                    </div>
                </div>
                <div class="modal-footer">
                    <div class="row">
                        <div class="col-md-6 text-center">
                            <asp:LinkButton ID="lbtnDelete" runat="server" OnClick="DeleteModal"
                                CssClass="btn btn-success"><i class="fa fa-save"></i> Confirmar
                            </asp:LinkButton>
                        </div>
                        <div class="col-md-6 text-center">
                            <button type="button" class="btn btn-danger" data-dismiss="modal">
                                Cerrar</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="modalError" role="dialog">
        <div class="alert alert-danger alert-dismissible fade in" style="margin: 20%;">
            <a href="javascript:;" class="close" data-dismiss="modal" aria-label="close">&times;</a>
            <strong>Alerta!</strong><br />
            <br />
            <asp:Label ID="lbl_mensaje_error" runat="server" Text=""></asp:Label>
        </div>
    </div>

    <div class="modal fade" id="modalConfirm" role="dialog">
        <div class="alert alert-success alert-dismissible fade in" style="margin: 20%;">
            <a href="javascript:;" class="close" data-dismiss="modal" aria-label="close">&times;</a>
            <strong>Informacion!</strong><br />
            <br />
            <asp:Label ID="lbl_mensaje" runat="server" Text=""></asp:Label>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Scripts" runat="server">
    <script type="text/javascript">

       <%-- if ($("#<%=lbl_mensaje_error.ClientID%>").text() != "") {
            $('#modalError').modal('toggle');
        }--%>

        $(document).ready(function () {
            console.log("entro table");
            $('#tabla').DataTable({
                "order": [[3, "asc"]],
                "language": {
                    "lengthMenu": "Registros por página: _MENU_",
                    "zeroRecords": "No se encontraron registros.",
                    "info": "Página _PAGE_ de _PAGES_",
                    "infoEmpty": "No hay registros disponibles",
                    "infoFiltered": "(filtered from _MAX_ total records)",
                    "search": "Buscar:",
                    "paginate": {
                        "first": "Primero",
                        "last": "Último",
                        "next": "Siguiente",
                        "previous": "Anterior"
                    }
                }
            });
        });

        function openModal() {
            $("#MyPopup .modal-title").html();
            $('#MyPopup').modal('show');
           
        }

        function AlertModal(title) {
            $("#modalError .modal-title").html(title);
            $('#modalError').modal('show');           
        }

        function SuccessModal(title) {
            $("#modalConfirm .modal-title").html(title);
            $('#modalConfirm').modal('show');
        }

        function openModalD(title) {
            $("#ModalEliminar .modal-title").html(title);
            $('#ModalEliminar').modal('show');
        }

        function ShowPopup(title, body) {
            $("#MyPopup .modal-title").html(title);
            $("#MyPopup .modal-body").html(body);
            $("#MyPopup").modal("show");
        }


    </script>

   
<%--    <script>  
        function alert() {
            swal("Here's a message!")
        }
    </script>--%>
</asp:Content>
