using System.Linq;
using System.Collections.Generic;
using Epi.Web.Common.BusinessObject;
using Epi.Web.Common.DTO;
using Epi.Web.Common.Message;

namespace Epi.Web.Common.ObjectMapping
{
    /// <summary>
    /// Maps DTOs (Data Transfer Objects) to BOs (Business Objects) and vice versa.
    /// </summary>
    public static class Mapper
    {

        /// <summary>
        /// Maps SurveyMetaData entity to SurveyInfoBO business object.
        /// </summary>
        /// <param name="entity">A SurveyMetaData entity to be transformed.</param>
        /// <returns>A SurveyInfoBO business object.</returns>
        public static SurveyInfoBO ToBusinessObject(SurveyInfoDTO pDTO)
        {
            return new SurveyInfoBO
            {
                SurveyId = pDTO.SurveyId,
                SurveyName = pDTO.SurveyName,
                SurveyNumber = pDTO.SurveyNumber,
                XML = pDTO.XML,
                IntroductionText = pDTO.IntroductionText,
                ExitText = pDTO.ExitText,
                OrganizationName = pDTO.OrganizationName,
                DepartmentName = pDTO.DepartmentName,
                ClosingDate = pDTO.ClosingDate,
                UserPublishKey=pDTO.UserPublishKey,
                SurveyType = pDTO.SurveyType,
                OrganizationKey = pDTO.OrganizationKey

            };
        }

        public static OrganizationBO ToBusinessObject(OrganizationDTO pDTO)
        {
            return new OrganizationBO
            {
                 IsEnabled = pDTO.IsEnabled,
                 Organization = pDTO.Organization,
                 OrganizationKey = pDTO.OrganizationKey,
                 // AdminId = pDTO.AdminId,
                  
            };
        }

        public static OrganizationDTO ToDataTransferObjects(OrganizationBO pBO)
        {

            return new OrganizationDTO
            {
              //  AdminId = pBO.AdminId,
                IsEnabled = pBO.IsEnabled,
                Organization = pBO.Organization,
                OrganizationKey = pBO.OrganizationKey

            };

        }

        public static List<SurveyInfoBO> ToBusinessObject(List<SurveyInfoDTO> pSurveyInfoList)
        {
            List<SurveyInfoBO> result = new List<SurveyInfoBO>();
            foreach (SurveyInfoDTO surveyInfo in pSurveyInfoList)
            {
                result.Add(ToBusinessObject(surveyInfo));
            };

            return result;
        }


        /// <summary>
        /// Maps SurveyInfoBO business object to SurveyInfoDTO entity.
        /// </summary>
        /// <param name="SurveyInfo">A SurveyInfoBO business object.</param>
        /// <returns>A SurveyInfoDTO.</returns>
        public static SurveyInfoDTO ToDataTransferObject(SurveyInfoBO pBO)
        {
            return new SurveyInfoDTO
            {
                SurveyId = pBO.SurveyId,
                SurveyName = pBO.SurveyName,
                SurveyNumber = pBO.SurveyNumber,
                XML = pBO.XML,
                IntroductionText = pBO.IntroductionText,
                ExitText = pBO.ExitText,
                OrganizationName = pBO.OrganizationName,
                DepartmentName = pBO.DepartmentName,
                SurveyType =pBO.SurveyType,
                ClosingDate = pBO.ClosingDate 
               

            };
        }
        public static List<SurveyInfoDTO> ToDataTransferObject(List<SurveyInfoBO> pSurveyInfoList)
        {
            List<SurveyInfoDTO> result = new List<SurveyInfoDTO>();
            foreach (SurveyInfoBO surveyInfo in pSurveyInfoList)
            {
                result.Add(ToDataTransferObject(surveyInfo));
            };

            return result;
        }

        /// <summary>
        /// Maps SurveyInfoBO business object to SurveyInfoDTO entity.
        /// </summary>
        /// <param name="SurveyInfo">A SurveyInfoBO business object.</param>
        /// <returns>A SurveyInfoDTO.</returns>
        public static SurveyAnswerDTO ToDataTransferObject(SurveyResponseBO pBO)
        {
            return new SurveyAnswerDTO
            {
                SurveyId = pBO.SurveyId,
                ResponseId = pBO.ResponseId,
                DateUpdated = pBO.DateUpdated,
                XML = pBO.XML,
                DateCompleted = pBO.DateCompleted,
                DateCreated = pBO.DateCreated, 
                Status = pBO.Status
            };
        }
        public static List<SurveyAnswerDTO> ToDataTransferObject(List<SurveyResponseBO> pSurveyResposneList)
        {
            List<SurveyAnswerDTO> result = new List<SurveyAnswerDTO>();
            foreach (SurveyResponseBO surveyResponse in pSurveyResposneList)
            {
                result.Add(ToDataTransferObject(surveyResponse));
            };

            return result;
        }

        /// <summary>
        /// Maps SurveyInfoBO business object to SurveyInfoDTO entity.
        /// </summary>
        /// <param name="SurveyInfo">A SurveyResponseDTO business object.</param>
        /// /// <returns>A SurveyResponseBO.</returns>
        public static SurveyResponseBO ToBusinessObject(SurveyAnswerDTO pBO)
        {
            return new SurveyResponseBO
            {
                SurveyId = pBO.SurveyId,
                ResponseId = pBO.ResponseId,
                DateUpdated = pBO.DateUpdated,
                XML = pBO.XML,
                DateCompleted = pBO.DateCompleted,
                DateCreated = pBO.DateCreated,
                Status = pBO.Status
                
            };
        }

        public static List<SurveyResponseBO> ToBusinessObject(List<SurveyAnswerDTO> pSurveyAnswerList)
        {
            List<SurveyResponseBO> result = new List<SurveyResponseBO>();
            foreach (SurveyAnswerDTO surveyAnswer in pSurveyAnswerList)
            {
                result.Add(ToBusinessObject(surveyAnswer));
            };

            return result;
        }

        /// <summary>
        /// Maps SurveyRequestResultBO business object to PublishInfoDTO.
        /// </summary>
        /// <param name="SurveyInfo">A SurveyRequestResultBO business object.</param>
        /// <returns>A PublishInfoDTO.</returns>
        public static PublishInfoDTO ToDataTransferObject(SurveyRequestResultBO pBO)
        {
            return new PublishInfoDTO
            {
                IsPulished = pBO.IsPulished,
                StatusText = pBO.StatusText,
                URL = pBO.URL

            };
        }

        public static UserAuthenticationRequestBO ToPassCodeBO(UserAuthenticationRequest UserAuthenticationObj)
        {
            return new UserAuthenticationRequestBO
            {
                ResponseId = UserAuthenticationObj.SurveyResponseId,
                PassCode = UserAuthenticationObj.PassCode

            };
        }

        public static UserAuthenticationResponse ToAuthenticationResponse(UserAuthenticationResponseBO AuthenticationRequestBO)
        {

            return new UserAuthenticationResponse
            {

                PassCode = AuthenticationRequestBO.PassCode,

            };
        
        
        }
        /// <summary>
        /// Transforms list of SurveyInfoBO BOs list of category DTOs.
        /// </summary>
        /// <param name="SurveyInfoBO">List of categories BOs.</param>
        /// <returns>List of SurveyInfoDTO DTOs.</returns>
        public static IList<SurveyInfoDTO> ToDataTransferObjects(IEnumerable<SurveyInfoBO> pBO)
        {
            if (pBO == null) return null;
            return pBO.Select(c => ToDataTransferObject(c)).ToList();
        }


        

        ///// <summary>
        ///// Transforms list of category BOs list of category DTOs.
        ///// </summary>
        ///// <param name="categories">List of categories BOs.</param>
        ///// <returns>List of category DTOs.</returns>
        //public static IList<CategoryDto> ToDataTransferObjects(IEnumerable<Category> categories)
        //{
        //    if (categories == null) return null;
        //    return categories.Select(c => ToDataTransferObject(c)).ToList();
        //}

        ///// <summary>
        ///// Transforms category BO to category DTO.
        ///// </summary>
        ///// <param name="category">Category BO.</param>
        ///// <returns>Category DTO.</returns>
        //public static CategoryDto ToDataTransferObject(Category category)
        //{
        //    if (category == null) return null;

        //    return new CategoryDto
        //    {
        //        CategoryId = category.CategoryId,
        //        Name = category.Name,
        //        Description = category.Description,
        //        Version = category.Version
        //    };
        //}

        ///// <summary>
        ///// Transforms list of Product BOs to list of Product DTOs.
        ///// </summary>
        ///// <param name="products">List of Product BOs.</param>
        ///// <returns>List of Product DTOs.</returns>
        //public static IList<ProductDto> ToDataTransferObjects(IEnumerable<Product> products)
        //{
        //    if (products == null) return null;
        //    return products.Select(p => ToDataTransferObject(p)).ToList();
        //}

        ///// <summary>
        ///// Transforms Product BO to Product DTO.
        ///// </summary>
        ///// <param name="product">Product BO.</param>
        ///// <returns>Product DTO.</returns>
        //public static ProductDto ToDataTransferObject(Product product)
        //{
        //    if (product == null) return null;

        //    return new ProductDto
        //    {
        //        ProductId = product.ProductId,
        //        ProductName = product.ProductName,
        //        Category = ToDataTransferObject(product.Category),
        //        UnitPrice = product.UnitPrice,
        //        UnitsInStock = product.UnitsInStock,
        //        Weight = product.Weight,
        //        Version = product.Version
        //    };
        //}

        ///// <summary>
        ///// Transforms list of SurveyInfo BOs to list of SurveyInfo DTOs.
        ///// </summary>
        ///// <param name="SurveyInfos">List of SurveyInfo BOs.</param>
        ///// <returns>List of SurveyInfo DTOs.</returns>
        //public static IList<SurveyInfoDto> ToDataTransferObjects(IEnumerable<SurveyInfo> SurveyInfos)
        //{
        //    if (SurveyInfos == null) return null;
        //    return SurveyInfos.Select(c => ToDataTransferObject(c)).ToList();
        //}

        ///// <summary>
        ///// Transforms SurveyInfo BO to SurveyInfo DTO.
        ///// </summary>
        ///// <param name="SurveyInfo">SurveyInfo BO.</param>
        ///// <returns>SurveyInfo DTO.</returns>
        //public static SurveyInfoDto ToDataTransferObject(SurveyInfo SurveyInfo)
        //{
        //    if (SurveyInfo == null) return null;

        //    return new SurveyInfoDto
        //    {
        //        SurveyInfoId = SurveyInfo.SurveyInfoId,
        //        Company = SurveyInfo.Company,
        //        Country = SurveyInfo.Country,
        //        City = SurveyInfo.City,
        //        Orders = ToDataTransferObjects(SurveyInfo.Orders),
        //        LastOrderDate = SurveyInfo.LastOrderDate,
        //        NumOrders = SurveyInfo.NumOrders,
        //        Version = SurveyInfo.Version
        //    };
        //}

        ///// <summary>
        ///// Transforms list of Order BOs to list of Order DTOs.
        ///// </summary>
        ///// <param name="orders">List of Order BOs.</param>
        ///// <returns>List of Order DTOs.</returns>
        //public static OrderDto[] ToDataTransferObjects(IEnumerable<Order> orders)
        //{
        //    if (orders == null) return null;
        //    return orders.Select(o => ToDataTransferObject(o)).ToArray();
        //}

        ///// <summary>
        ///// Transfers Order BO to Order DTO.
        ///// </summary>
        ///// <param name="order">Order BO.</param>
        ///// <returns>Order DTO.</returns>
        //public static OrderDto ToDataTransferObject(Order order)
        //{
        //    if (order == null) return null;

        //    return new OrderDto
        //    {
        //        OrderId = order.OrderId,
        //        Freight = order.Freight,
        //        OrderDate = order.OrderDate,
        //        SurveyInfo = ToDataTransferObject(order.SurveyInfo),
        //        OrderDetails = ToDataTransferObjects(order.OrderDetails),
        //        RequiredDate = order.RequiredDate,
        //        Version = order.Version
        //    };
        //}

        ///// <summary>
        ///// Transfers list of OrderDetail BOs to list of OrderDetail DTOs.
        ///// </summary>
        ///// <param name="orderDetails">List of OrderDetail BOs.</param>
        ///// <returns>List of OrderDetail DTOs.</returns>
        //public static OrderDetailDto[] ToDataTransferObjects(IEnumerable<OrderDetail> orderDetails)
        //{
        //    if (orderDetails == null) return null;
        //    return orderDetails.Select(o => ToDataTransferObject(o)).ToArray();
        //}

        ///// <summary>
        ///// Transfers OrderDetail BO to OrderDetail DTO.
        ///// </summary>
        ///// <param name="orderDetail">OrderDetail BO.</param>
        ///// <returns>OrderDetail DTO.</returns>
        //public static OrderDetailDto ToDataTransferObject(OrderDetail orderDetail)
        //{
        //    if (orderDetail == null) return null;

        //    return new OrderDetailDto
        //    {
        //        ProductName = orderDetail.ProductName,
        //        Discount = orderDetail.Discount,
        //        Quantity = orderDetail.Quantity,
        //        UnitPrice = orderDetail.UnitPrice,
        //        Version = orderDetail.Version
        //    };
        //}

        ///// <summary>
        ///// Transfers a Shopping Cart BO to Shopping Cart DTO.
        ///// </summary>
        ///// <param name="cart">Shopping Cart BO.</param>
        ///// <returns>Shopping Cart DTO.</returns>
        //public static ShoppingCartDto ToDataTransferObject(ShoppingCart cart)
        //{
        //    if (cart == null) return null;

        //    return new ShoppingCartDto
        //    {
        //        Shipping = cart.Shipping,
        //        SubTotal = cart.SubTotal,
        //        Total = cart.Total,
        //        ShippingMethod = cart.ShippingMethod.ToString(),
        //        CartItems = ToDataTransferObject(cart.Items)
        //    };
        //}

        ///// <summary>
        ///// Transfers list of Shopping Cart Item BOs to list of Shopping Cart Items DTOs.
        ///// </summary>
        ///// <param name="cartItems">List of Shopping Cart Items BOs.</param>
        ///// <returns>List of Shopping Cart Items DTOs.</returns>
        //private static ShoppingCartItemDto[] ToDataTransferObject(IList<ShoppingCartItem> cartItems)
        //{
        //    if (cartItems == null) return null;
        //    return cartItems.Select(i => ToDataTransferObject(i)).ToArray();
        //}

        ///// <summary>
        ///// Transfers Shopping Cart Item BO to Shopping Cart Item DTO.
        ///// </summary>
        ///// <param name="item">Shopping Cart Item BO.</param>
        ///// <returns>Shopping Cart Item DTO.</returns>
        //private static ShoppingCartItemDto ToDataTransferObject(ShoppingCartItem item)
        //{
        //    return new ShoppingCartItemDto
        //    {
        //        Id = item.Id,
        //        Name = item.Name,
        //        UnitPrice = item.UnitPrice,
        //        Quantity = item.Quantity
        //    };
        //}

        ///// <summary>
        ///// Transfers SurveyInfo DTO to SurveyInfo BO.
        ///// </summary>
        ///// <param name="c">SurveyInfo DTO.</param>
        ///// <returns>SurveyInfo BO.</returns>
        //public static SurveyInfo FromDataTransferObject(SurveyInfoDto c)
        //{
        //    if (c == null) return null;

        //    return new SurveyInfo
        //    {
        //        SurveyInfoId = c.SurveyInfoId,
        //        Company = c.Company,
        //        City = c.City,
        //        Country = c.Country,
        //        Version = c.Version
        //    };
        //}

        ///// <summary>
        ///// Transfers Shopping Cart Item DTO to Shopping Cart Item BO.
        ///// </summary>
        ///// <param name="item">Shopping Cart Item DTO.</param>
        ///// <returns>Shopping Cart Item BO.</returns>
        //public static ShoppingCartItem FromDataTransferObject(ShoppingCartItemDto item)
        //{
        //    return new ShoppingCartItem
        //    {
        //        Id = item.Id,
        //        Name = item.Name,
        //        Quantity = item.Quantity,
        //        UnitPrice = item.UnitPrice
        //    };
        //}
    }
}
