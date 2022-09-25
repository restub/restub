using System.Runtime.Serialization;

namespace Restub.Tests.Pochta.Tariff
{
    /// <summary>
    /// Tariffication object types.
    /// https://tariff.pochta.ru/post-calculator-api.pdf (see Appendix 1)
    /// </summary>
    public enum TariffObjectType
    {
        LetterRegular = 2000,
        ПисьмоПростое = 2000,

        LetterRegistered = 2010,
        ПисьмоЗаказное = 2010,

        LetterWithDeclaredValue = 2020,
        ПисьмоСОбъявленнойЦенностью = 2020,

        LetterWithDeclaredValueAndCashOnDelivery = 2040,
        ПисьмоСОбъявленнойЦенностьюИНаложеннымПлатежом = 2040,

        Letter1ClassRegistered = 15010,
        Письмо1КлассаЗаказное = 15010,

        Letter1ClassWithDeclaredValue = 15020,
        Письмо1КлассаСОбъявленнойЦенностью = 15020,

        Letter1ClassWithDeclaredValueAndCashOnDelivery = 15040,
        Письмо1КлассаСОбъявленнойЦенностьюИНаложеннымПлатежом = 15040,

        LetterIntraCity1ClassRegular = 46000,
        ПисьмоВнутригородское1КлассаПростое = 46000,

        LetterIntraCity1ClassRegistered = 46010,
        ПисьмоВнутригородское1КлассаЗаказное = 46010,

        LetterRegular20 = 11000,
        ПисьмоПростое20 = 11000,

        LetterRegistered20 = 11010,
        ПисьмоЗаказное20 = 11010,

        LetterExpressRegistered = 32010,
        ПисьмоЭкспрессЗаказное = 32010,

        LetterCourierRegistered = 33010,
        ПисьмоКурьерскоеЗаказное = 33010,

        LetterTrackedPostcard = 36000,
        ПисьмоТрекОткрытка = 36000,

        LetterTracked = 37000,
        ПисьмоТрек = 37000,

        PostcardRegular = 6000,
        ПочтоваяКарточкаПростая = 6000,
        ОткрыткаПростая = 6000,

        PostcardRegistered = 6010,
        ПочтоваяКарточкаЗаказная = 6010,
        ОткрыткаЗаказная = 6010,

        Secogram = 8010,
        Секограмма = 8010,

        // ----------------

        WrapperRegular = 3000,
        БандерольПростая = 3000,

        WrapperRegistered = 3010,
        БандерольЗаказная = 3010,

        WrapperWithDeclaredValue = 3020,
        БандерольСОбъявленнойЦенностью = 3020,

        WrapperWithDeclaredValueAndCashOnDelivery = 3040,
        БандерольСОбъявленнойЦенностьюИНаложеннымПлатежом = 3040,

        Wrapper1ClassRegistered = 16010,
        Бандероль1КлассаЗаказное = 16010,

        Wrapper1ClassWithDeclaredValue = 16020,
        Бандероль1КлассаСОбъявленнойЦенностью = 16020,

        Wrapper1ClassWithDeclaredValueAndCashOnDelivery = 16040,
        Бандероль1КлассаСОбъявленнойЦенностьюИНаложеннымПлатежом = 16040,

        // ----------------

        ParcelStandard = 27030,
        ПосылкаСтандарт = 27030,

        ParcelStandardWithDeclaredValue = 27020,
        ПосылкаСтандартСОбъявленнойЦенностью = 27020,

        ParcelStandardWithDeclaredValueAndCashOnDelivery = 27040,
        ПосылкаСтандартСОбъявленнойЦенностьюИНаложеннымПлатежом = 27040,

        // ----------------

        ParcelExpress = 29030,
        ПосылкаЭкспресс = 29030,

        ParcelExpressWithDeclaredValue = 29020,
        ПосылкаЭкспрессСОбъявленнойЦенностью = 29020,

        ParcelExpressWithDeclaredValueAndCashOnDelivery = 29040,
        ПосылкаЭкспрессСОбъявленнойЦенностьюИНаложеннымПлатежом = 29040,

        // ----------------

        ParcelCourierEms = 28030,
        ПосылкаКурьерЕмс = 28030,

        ParcelCourierEmsWithDeclaredValue = 28020,
        ПосылкаКурьерЕмсСОбъявленнойЦенностью = 28020,

        ParcelCourierEmsWithDeclaredValueAndCashOnDelivery = 28040,
        ПосылкаКурьерЕмсСОбъявленнойЦенностьюИНаложеннымПлатежом = 28040,

        // ----------------

        ParcelNonstandard = 4030,
        ПосылкаНестандартная = 4030,

        ParcelNonstandardWithDeclaredValue = 4020,
        ПосылкаНестандартнаяСОбъявленнойЦенностью = 4020,

        ParcelNonstandardWithDeclaredValueAndCashOnDelivery = 4040,
        ПосылкаНестандартнаяСОбъявленнойЦенностьюИНаложеннымПлатежом = 4040,

        ParcelNonstandardWithDeclaredValueAndObligatoryPayment = 4060,
        ПосылкаНестандартнаяСОбъявленнойЦенностьюИОбязательнымПлатежом = 4060,

        // ----------------

        Parcel1Class = 47030,
        Посылка1Класса = 47030,

        Parcel1ClassWithDeclaredValue = 47020,
        Посылка1КлассаСОбъявленнойЦенностью = 47020,

        Parcel1ClassWithDeclaredValueAndCashOnDelivery = 47040,
        Посылка1КлассаСОбъявленнойЦенностьюИНаложеннымПлатежом = 47040,

        Parcel1ClassWithDeclaredValueAndObligatoryPayment = 47060,
        Посылка1КлассаСОбъявленнойЦенностьюИОбязательнымПлатежом = 47060,
    }
}
