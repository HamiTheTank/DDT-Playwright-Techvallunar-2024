using Microsoft.Playwright;

namespace UI_Test__Playwright_
{
    internal class MainPage : PageTest
    {

        //BROWSER
        private readonly IPage _page;
        //LOCATORS
        private readonly ILocator _douglasLogo;
        private readonly ILocator _shadowRoot;
        private readonly ILocator _acceptAllBtn;
        private readonly ILocator _parfumeBtn;
        private readonly ILocator _priceDropdown;
        private readonly ILocator _scrollbarsView;
        private readonly ILocator _produktartDropdown;
        private readonly ILocator _produktartSearchBar;
        private readonly ILocator _markeDropdown;
        private readonly ILocator _markeSearchBar;
        private readonly ILocator _furWenDropdown;
        private readonly ILocator _highlightsDropdown;
        private readonly ILocator _geschenkFurDropdown;
        private readonly ILocator _geschenkSearchBar;
        private readonly ILocator _parfumResultsLabel;

        public MainPage(IPage page)
        {            
            _page = page;
            _douglasLogo = _page.Locator(Constants.douglasLogo);
            _shadowRoot = _page.Locator(Constants.shadowRoot);
            _acceptAllBtn = _shadowRoot.GetByTestId(Constants.acceptAllBtn);
            _parfumeBtn = _page.Locator(Constants.parfumeBtn);
            _priceDropdown = _page.GetByTestId(Constants.priceDropdownId);
            _scrollbarsView = _page.Locator(Constants.scrollbarsView);
            _parfumResultsLabel = _page.Locator(Constants.parfumResultsLable);
            //dropdowns
            _produktartDropdown = _page.GetByTestId(Constants.produktartDropdownId);
            _produktartSearchBar = _page.GetByPlaceholder(Constants.produktartSearchBarPlaceholder);
            _markeDropdown = _page.GetByTestId(Constants.markeDropdownId);
            _markeSearchBar = _page.GetByPlaceholder(Constants.markeSearchBarPlaceholder);
            _furWenDropdown = _page.GetByTestId(Constants.furWenDropdownId);
            _highlightsDropdown = _page.GetByTestId(Constants.highlightsDropdownId);
            _geschenkFurDropdown = _page.GetByTestId(Constants.geschenkFurDropdownId);
            _geschenkSearchBar = _page.GetByPlaceholder(Constants.geschenkSearchBarPlaceholder);

        }

        public async Task GotoAsync()
        {
            await _page.GotoAsync(Constants.mainURL);
            await _page.WaitForLoadStateAsync();
        }

        public async Task HandleCookieWindow()
        {        
            await _acceptAllBtn.IsVisibleAsync();
            await _acceptAllBtn.ClickAsync();
        }

        public async Task ClickParfumesBtn()
        {
            await _parfumeBtn.IsVisibleAsync();
            await _parfumeBtn.ClickAsync();
            //await _page.Locator(Constants.douglasLogo).HoverAsync();
            await _douglasLogo.HoverAsync();
            await _priceDropdown.IsVisibleAsync();
        }

        private async Task applyFiltering()
        {
            await _scrollbarsView.IsVisibleAsync();
            var checkbox = _scrollbarsView.Locator("input");
            await Expect(checkbox).ToHaveCountAsync(1);
            await checkbox.FocusAsync();
            await checkbox.ClickAsync();
            await checkbox.IsCheckedAsync();
            await _scrollbarsView.PressAsync("Escape");
        }

        private async Task TypeFiltering(ILocator searchBar, string searchTerm)
        {
            await searchBar.IsVisibleAsync();
            await searchBar.ScrollIntoViewIfNeededAsync();
            await searchBar.ClickAsync();
            await searchBar.FocusAsync();
            await searchBar.TypeAsync(searchTerm);
        }

        private async Task SelectCheckbox(ILocator checkbox)
        {
            await Expect(checkbox).ToHaveCountAsync(1);
            await checkbox.FocusAsync();
            await checkbox.ClickAsync();
            await checkbox.IsCheckedAsync();
        }

        public async Task FilterProduktart(string produktart)
        {
            if (string.IsNullOrEmpty(produktart)) return;
            var labeltext = await _parfumResultsLabel.TextContentAsync();
            await _produktartDropdown.ClickAsync();
            await TypeFiltering(_produktartSearchBar, produktart);
            await applyFiltering();
            await Expect(_page.GetByText(labeltext)).ToHaveCountAsync(0);
        }

        public async Task FilterMarke(string marke)
        {
            if (string.IsNullOrEmpty(marke)) return;
            var labeltext = await _parfumResultsLabel.TextContentAsync();
            await _markeDropdown.ClickAsync();
            await TypeFiltering(_markeSearchBar, marke);
            await applyFiltering();
            await Expect(_page.GetByText(labeltext)).ToHaveCountAsync(0);
        }

        public async Task FilterGeschenkFur(string geshenkFur)
        {
            if (string.IsNullOrEmpty(geshenkFur)) return;
            var labeltext = await _parfumResultsLabel.TextContentAsync();
            await _geschenkFurDropdown.ClickAsync();
            await TypeFiltering(_geschenkSearchBar, geshenkFur);
            await applyFiltering();
            await Expect(_page.GetByText(labeltext)).ToHaveCountAsync(0);
        }

        public async Task FilterFurWen(string furWen)
        {
            if (string.IsNullOrEmpty(furWen)) return;
            var labeltext = await _parfumResultsLabel.TextContentAsync();
            await _furWenDropdown.ClickAsync();
            await SelectCheckbox(_page.Locator(string.Format(Constants.furWenXpath, furWen)));
            await _furWenDropdown.PressAsync("Escape");
            await Expect(_page.GetByText(labeltext)).ToHaveCountAsync(0);
        }

        public async Task FilterHighlights(string highlight)
        {
            if (string.IsNullOrEmpty(highlight)) return;
            var labeltext = await _parfumResultsLabel.TextContentAsync();
            await _highlightsDropdown.ClickAsync();
            await SelectCheckbox(_highlightsDropdown.Locator(string.Format(Constants.highlightsXpath, highlight)));
            await _highlightsDropdown.PressAsync("Escape");
            await Expect(_page.GetByText(labeltext)).ToHaveCountAsync(0);
        }

        public async Task<int> GetResultCount()
        {
            int count = 0;
            var labelText = await _page.Locator(Constants.parfumResultsLable).TextContentAsync();
            
            try
            {
                labelText = labelText?.Split("(").Last().Replace(".", string.Empty).Replace(")", string.Empty);
                count = int.Parse(labelText);
            }
            catch (Exception ex)
            { 
                Console.WriteLine("parsing label value to integer failed: " + ex.Message);
            }

            return count;
        }

        public async Task CloseAsync()
        {
            Thread.Sleep(3000);// just for bemonstration purposes, to not close immediately
            await _page.CloseAsync();
        }
    }
}
