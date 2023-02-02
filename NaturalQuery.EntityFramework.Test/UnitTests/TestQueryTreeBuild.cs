using Microsoft.VisualStudio.TestTools.UnitTesting;
using NaturalQuery.PostfixSyntax;
using NaturalQuery.QueryTreeSyntax;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaturalQuery.EntityFramework.Test
{
    [TestClass]
    public class TestQueryTreeBuild : AbstractTestQueryTree
    {
        [TestCleanup]
        public void Finalizer()
        {
            if (Given != null && Expected != null && Actual != null)
            {
                Debug.WriteLine("Given the following postfix expression:");
                Debug.WriteLine((object)Given);
                Debug.WriteLine(string.Empty);
                Debug.WriteLine("Expected tree:");
                Debug.WriteLine((object)Expected);
                Debug.WriteLine(string.Empty);
                Debug.WriteLine("Actual tree:");
                Debug.WriteLine((object)Actual);
            }
        }

        [TestMethod]
        [TestCategory("PostfixExpression To QueryTree")]
        public void Test_equality_successfully()
        {
            EntityQueryNode entity = new EntityQueryNode(StoreModelContext.Customers);
            FieldQueryNode field = new FieldQueryNode("ContactName");
            SelectQueryNode select = new SelectQueryNode(field, entity);

            Actual = new QueryTree(select);

            Assert.IsNotNull(Actual);
        }

        [TestMethod]
        [TestCategory("PostfixExpression To QueryTree")]
        public void Mostre_a_descricao_dos_clientes()
        {
            Expected = new QueryTree(
                QueryNode.Select(
                    QueryNode.Field("ContactName"),
                    QueryNode.Entity(StoreModelContext.Customers)
                )
            );

            Given = new PostfixExpression();
            Given.AddValue("Field", "ContactName");
            Given.AddValue("Entity", StoreModelContext.Customers);
            Given.AddOperator("Select", 2);


            Actual = Given.ToQueryTree();

            Assert.AreEqual(Expected, Actual);
        }


        [TestMethod]
        [TestCategory("PostfixExpression To QueryTree")]
        public void Mostre_a_descricao_dos_clientes_que_tenham_pedidos_ordenado_pelo_maior_total_do_pedido_descendente()
        {
            Expected = new QueryTree(
                QueryNode.Select(
                    QueryNode.Field("ContactName"),
                    QueryNode.OrderDescending(
                        QueryNode.Select(
                            QueryNode.Projection(
                                QueryNode.GroupField("ContactName"),
                                QueryNode.Aggregation(
                                    QueryNode.Value("Max"),
                                    QueryNode.Field("Orders.OrderTotal")
                                )
                            ),
                            QueryNode.Group(
                                QueryNode.Join(
                                    QueryNode.Entity(StoreModelContext.Customers),
                                    QueryNode.Field("CustomerID"),
                                    QueryNode.Entity(StoreModelContext.Orders),
                                    QueryNode.Field("CustomerID")
                                ),
                                QueryNode.Field("Customers.ContactName")
                            )
                        ),
                        QueryNode.Field("OrderTotal")
                    )
                )
            );

            Given = new PostfixExpression();
            Given.AddValue("Field", "ContactName");
            Given.AddValue("GroupField", "ContactName");
            Given.AddValue("Value", "Max");
            Given.AddValue("Field", "Orders.OrderTotal");
            Given.AddOperator("Aggregation", 2);
            Given.AddOperator("Projection", 2);
            Given.AddValue("Entity", StoreModelContext.Customers);
            Given.AddValue("Field", "CustomerID");
            Given.AddValue("Entity", StoreModelContext.Orders);
            Given.AddValue("Field", "CustomerID");
            Given.AddOperator("Join", 4);
            Given.AddValue("Field", "Customers.ContactName");
            Given.AddOperator("Group", 2);
            Given.AddOperator("Select", 2);
            Given.AddValue("Field", "OrderTotal");
            Given.AddOperator("OrderDescending", 2);
            Given.AddOperator("Select", 2);


            Actual = Given.ToQueryTree();

            Assert.AreEqual(Expected, Actual);
        }

        [TestMethod]
        [TestCategory("PostfixExpression To QueryTree")]
        public void Mostre_a_descricao_dos_clientes_com_o_nome_contendo_Joao()
        {
            Expected = new QueryTree(
                QueryNode.Select(
                    QueryNode.Field("ContactName"),
                    QueryNode.Where(
                        QueryNode.Entity(StoreModelContext.Customers),
                        QueryNode.Contains(
                            QueryNode.Field("ContactName"),
                            QueryNode.Value("João")
                        )
                    )
                )
            );

            Given = new PostfixExpression();
            Given.AddValue("Field", "ContactName");
            Given.AddValue("Entity", StoreModelContext.Customers);
            Given.AddValue("Field", "ContactName");
            Given.AddValue("Value", "João");
            Given.AddOperator("Contains", 2);
            Given.AddOperator("Where", 2);
            Given.AddOperator("Select", 2);


            Actual = Given.ToQueryTree();

            Assert.AreEqual(Expected, Actual);
        }


        [TestMethod]
        [TestCategory("PostfixExpression To QueryTree")]
        public void Mostre_o_nome_da_cidade_e_a_quantidade_de_clientes_agrupados_por_cidade()
        {
            Expected = new QueryTree(
                QueryNode.Select(
                    QueryNode.Projection(
                        QueryNode.GroupField("City"),
                        QueryNode.Aggregation(new ValueQueryNode("Count"), new EmptyQueryNode())
                    ),
                    QueryNode.Group(
                        QueryNode.Entity(StoreModelContext.Customers),
                        QueryNode.Field("City")
                    )
                )
            );

            Given = new PostfixExpression();
            Given.AddValue("GroupField", "City");
            Given.AddValue("Value", "Count");
            Given.AddValue("Empty");
            Given.AddOperator("Aggregation", 2);
            Given.AddOperator("Projection", 2);
            Given.AddValue("Entity", StoreModelContext.Customers);
            Given.AddValue("Field", "City");
            Given.AddOperator("Group", 2);
            Given.AddOperator("Select", 2);


            Actual = Given.ToQueryTree();

            Assert.AreEqual(Expected, Actual);
        }

        [TestMethod]
        [TestCategory("PostfixExpression To QueryTree")]
        public void Mostre_o_nome_e_a_cidade_dos_clientes_que_pedidos_que_tenham_produtos_da_categoria_A()
        {
            Expected = new QueryTree(
                QueryNode.Select(
                    QueryNode.Projection(
                        QueryNode.Field("Customers.ContactName"),
                        QueryNode.Field("Customers.City")
                    ),
                    QueryNode.Where(
                        QueryNode.Join(
                            QueryNode.Entity(StoreModelContext.Customers),
                            QueryNode.Field("CustomerID"),
                            QueryNode.Join(
                                QueryNode.Entity(StoreModelContext.Orders),
                                QueryNode.Field("OrderID"),
                                QueryNode.Join(
                                    QueryNode.Entity(StoreModelContext.OrderDetails),
                                    QueryNode.Field("ProductID"),
                                    QueryNode.Join(
                                        QueryNode.Entity(StoreModelContext.Products),
                                        QueryNode.Field("CategoryID"),
                                        QueryNode.Entity(StoreModelContext.Categories),
                                        QueryNode.Field("CategoryID")
                                    ),
                                    QueryNode.Field("Products.ProductID")
                                ),
                                QueryNode.Field("OrderDetails.OrderID")
                            ),
                            QueryNode.Field("Orders.CustomerID")
                        ),
                        QueryNode.IsEquals(
                            QueryNode.Field("Categories.CategoryName"),
                            QueryNode.Value("A")
                        )
                    )
                )
            );

            Given = new PostfixExpression();
            Given.AddValue("Field", "Customers.ContactName");
            Given.AddValue("Field", "Customers.City");
            Given.AddOperator("Projection", 2);
            Given.AddValue("Entity", StoreModelContext.Customers);
            Given.AddValue("Field", "CustomerID");
            Given.AddValue("Entity", StoreModelContext.Orders);
            Given.AddValue("Field", "OrderID");
            Given.AddValue("Entity", StoreModelContext.OrderDetails);
            Given.AddValue("Field", "ProductID");
            Given.AddValue("Entity", StoreModelContext.Products);
            Given.AddValue("Field", "CategoryID");
            Given.AddValue("Entity", StoreModelContext.Categories);
            Given.AddValue("Field", "CategoryID");
            Given.AddOperator("Join", 4);
            Given.AddValue("Field", "Products.ProductID");
            Given.AddOperator("Join", 4);
            Given.AddValue("Field", "OrderDetails.OrderID");
            Given.AddOperator("Join", 4);
            Given.AddValue("Field", "OrderDetails.CustomerID");
            Given.AddOperator("Join", 4);
            Given.AddValue("Field", "Categories.CategoryName");
            Given.AddValue("Value", "A");
            Given.AddOperator("IsEquals", 2);
            Given.AddOperator("Where", 2);
            Given.AddOperator("Select", 2);

            Actual = Given.ToQueryTree();

            Assert.AreEqual(Expected, Actual);
        }

        [TestMethod]
        [TestCategory("PostfixExpression To QueryTree")]
        public void Mostre_a_descricao_dos_clientes_que_tenham_pedidos_com_total_maior_que_2_ordenados_descendente_pela_data_do_pedido()
        {
            Expected = new QueryTree(
                QueryNode.Select(
                    QueryNode.Field("Customers.ContactName"),
                    QueryNode.OrderDescending(
                        QueryNode.Where(
                            QueryNode.Join(
                                QueryNode.Entity(StoreModelContext.Customers),
                                QueryNode.Field("CustomerID"),
                                QueryNode.Entity(StoreModelContext.Orders),
                                QueryNode.Field("CustomerID")
                            ),
                            QueryNode.GreaterThan(
                                QueryNode.Field("Orders.OrderTotal"),
                                QueryNode.Value(2)
                            )
                        ),
                        QueryNode.Field("Orders.OrderDate")
                    )
                )
            );

            Given = new PostfixExpression();
            Given.AddValue("Field", "Customers.ContactName");
            Given.AddValue("Entity", StoreModelContext.Customers);
            Given.AddValue("Field", "CustomerID");
            Given.AddValue("Entity", StoreModelContext.Orders);
            Given.AddValue("Field", "OrderID");
            Given.AddOperator("Join", 4);
            Given.AddValue("Field", "Orders.OrderTotal");
            Given.AddValue("Value", 2);
            Given.AddOperator("GreaterThan", 2);
            Given.AddOperator("Where", 2);
            Given.AddValue("Field", "Orders.OrderDate");
            Given.AddOperator("OrderDescending", 2);
            Given.AddOperator("Select", 2);


            Actual = Given.ToQueryTree();

            Assert.AreEqual(Expected, Actual);
        }


        [TestMethod]
        [TestCategory("PostfixExpression To QueryTree")]
        public void Mostre_a_descricao_dos_clientes_ordenado_descendente_por_ID()
        {
            Expected = new QueryTree(
                QueryNode.Select(
                    QueryNode.Field("ContactName"),
                    QueryNode.OrderDescending(
                        QueryNode.Entity(StoreModelContext.Customers),
                        QueryNode.Field("CustomerID")
                    )
                )
            );

            Given = new PostfixExpression();
            Given.AddValue("Field", "ContactName");
            Given.AddValue("Entity", StoreModelContext.Customers);
            Given.AddValue("Field", "CustomerID");
            Given.AddOperator("OrderDescending", 2);
            Given.AddOperator("Select", 2);


            Actual = Given.ToQueryTree();

            Assert.AreEqual(Expected, Actual);
        }
    }
}
