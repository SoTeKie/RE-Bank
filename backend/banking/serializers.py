from rest_framework import serializers

from .models import BankAccount, Card, Transaction, BillingAddress, ShippingAddress


class BankAccountSerializer(serializers.HyperlinkedModelSerializer):
    class Meta:
        model = BankAccount
        fields = "__all__"


class CardSerializer(serializers.HyperlinkedModelSerializer):
    class Meta:
        model = Card
        fields = "__all__"


class TransactionSerializer(serializers.HyperlinkedModelSerializer):
    class Meta:
        model = Transaction
        fields = "__all__"


class BillingAddressSerializer(serializers.HyperlinkedModelSerializer):
    class Meta:
        model = BillingAddress
        fields = "__all__"


class ShippingAddressSerializer(serializers.HyperlinkedModelSerializer):
    class Meta:
        model = ShippingAddress
        fields = "__all__"
