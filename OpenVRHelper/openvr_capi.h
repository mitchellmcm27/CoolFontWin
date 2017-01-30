



<!DOCTYPE html>
<html lang="en" class=" is-u2f-enabled">
  <head prefix="og: http://ogp.me/ns# fb: http://ogp.me/ns/fb# object: http://ogp.me/ns/object# article: http://ogp.me/ns/article# profile: http://ogp.me/ns/profile#">
    <meta charset='utf-8'>
    

    <link crossorigin="anonymous" href="https://assets-cdn.github.com/assets/frameworks-298818692f75de57d67115ca5a0c1f983d1d5ad302774216c297495f46f0a3da.css" integrity="sha256-KYgYaS913lfWcRXKWgwfmD0dWtMCd0IWwpdJX0bwo9o=" media="all" rel="stylesheet" />
    <link crossorigin="anonymous" href="https://assets-cdn.github.com/assets/github-e6eb4e5f69613961259d51b9c0e2ee31d3613e961bb480edc6d489a8739431a5.css" integrity="sha256-5utOX2lhOWElnVG5wOLuMdNhPpYbtIDtxtSJqHOUMaU=" media="all" rel="stylesheet" />
    
    
    
    

    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta http-equiv="Content-Language" content="en">
    <meta name="viewport" content="width=device-width">
    
    <title>openvr/openvr_capi.h at master · ValveSoftware/openvr</title>
    <link rel="search" type="application/opensearchdescription+xml" href="/opensearch.xml" title="GitHub">
    <link rel="fluid-icon" href="https://github.com/fluidicon.png" title="GitHub">
    <link rel="apple-touch-icon" href="/apple-touch-icon.png">
    <link rel="apple-touch-icon" sizes="57x57" href="/apple-touch-icon-57x57.png">
    <link rel="apple-touch-icon" sizes="60x60" href="/apple-touch-icon-60x60.png">
    <link rel="apple-touch-icon" sizes="72x72" href="/apple-touch-icon-72x72.png">
    <link rel="apple-touch-icon" sizes="76x76" href="/apple-touch-icon-76x76.png">
    <link rel="apple-touch-icon" sizes="114x114" href="/apple-touch-icon-114x114.png">
    <link rel="apple-touch-icon" sizes="120x120" href="/apple-touch-icon-120x120.png">
    <link rel="apple-touch-icon" sizes="144x144" href="/apple-touch-icon-144x144.png">
    <link rel="apple-touch-icon" sizes="152x152" href="/apple-touch-icon-152x152.png">
    <link rel="apple-touch-icon" sizes="180x180" href="/apple-touch-icon-180x180.png">
    <meta property="fb:app_id" content="1401488693436528">

      <meta content="https://avatars0.githubusercontent.com/u/3082775?v=3&amp;s=400" name="twitter:image:src" /><meta content="@github" name="twitter:site" /><meta content="summary" name="twitter:card" /><meta content="ValveSoftware/openvr" name="twitter:title" /><meta content="openvr - OpenVR SDK" name="twitter:description" />
      <meta content="https://avatars0.githubusercontent.com/u/3082775?v=3&amp;s=400" property="og:image" /><meta content="GitHub" property="og:site_name" /><meta content="object" property="og:type" /><meta content="ValveSoftware/openvr" property="og:title" /><meta content="https://github.com/ValveSoftware/openvr" property="og:url" /><meta content="openvr - OpenVR SDK" property="og:description" />
      <meta name="browser-stats-url" content="https://api.github.com/_private/browser/stats">
    <meta name="browser-errors-url" content="https://api.github.com/_private/browser/errors">
    <link rel="assets" href="https://assets-cdn.github.com/">
    <link rel="web-socket" href="wss://live.github.com/_sockets/VjI6MTI5NTI2MDMwOjMxMGYxZTdlMWVmYTRjY2ZmNjEzZTc4NTljNTM0YzBhMjY3YTYxZDk1NjliMDcwZjZhZmJiODkwYjU5ODdmMDk=--8ceb761151e21ded7016c0731b3fa4f8aab0ca3a">
    <meta name="pjax-timeout" content="1000">
    <link rel="sudo-modal" href="/sessions/sudo_modal">
    <meta name="request-id" content="FEDF:7361:DB005C:1633E18:588C284A" data-pjax-transient>

    <meta name="msapplication-TileImage" content="/windows-tile.png">
    <meta name="msapplication-TileColor" content="#ffffff">
    <meta name="selected-link" value="repo_source" data-pjax-transient>

    <meta name="google-site-verification" content="KT5gs8h0wvaagLKAVWq8bbeNwnZZK1r1XQysX3xurLU">
<meta name="google-site-verification" content="ZzhVyEFwb7w3e0-uOTltm8Jsck2F5StVihD0exw2fsA">
    <meta name="google-analytics" content="UA-3769691-2">

<meta content="collector.githubapp.com" name="octolytics-host" /><meta content="github" name="octolytics-app-id" /><meta content="FEDF:7361:DB005C:1633E18:588C284A" name="octolytics-dimension-request_id" /><meta content="14998294" name="octolytics-actor-id" /><meta content="mitchellmcm27" name="octolytics-actor-login" /><meta content="3e03d134bda4bc29823c5300ba1d197b0749358d0e3589a5996d5f3dabb6ffc7" name="octolytics-actor-hash" />
<meta content="/&lt;user-name&gt;/&lt;repo-name&gt;/blob/show" data-pjax-transient="true" name="analytics-location" />



  <meta class="js-ga-set" name="dimension1" content="Logged In">



        <meta name="hostname" content="github.com">
    <meta name="user-login" content="mitchellmcm27">

        <meta name="expected-hostname" content="github.com">
      <meta name="js-proxy-site-detection-payload" content="MmViNjJkMWE0ZDNhYWRmYTYxZmMzMGE0NDI5NWFkODU2MjJiMTRmMjk4ZDMyZTY0NGNmOTYwYjhjOWViYWExMHx7InJlbW90ZV9hZGRyZXNzIjoiMTA4LjY0LjIyMC4xNTAiLCJyZXF1ZXN0X2lkIjoiRkVERjo3MzYxOkRCMDA1QzoxNjMzRTE4OjU4OEMyODRBIiwidGltZXN0YW1wIjoxNDg1NTgwMzY3LCJob3N0IjoiZ2l0aHViLmNvbSJ9">


      <link rel="mask-icon" href="https://assets-cdn.github.com/pinned-octocat.svg" color="#000000">
      <link rel="icon" type="image/x-icon" href="https://assets-cdn.github.com/favicon.ico">

    <meta name="html-safe-nonce" content="6f1a3a3e6d20733282efa962e498c19537dc2622">

    <meta http-equiv="x-pjax-version" content="f0205f381ec31a38c9673c32ad285764">
    

      
  <meta name="description" content="openvr - OpenVR SDK">
  <meta name="go-import" content="github.com/ValveSoftware/openvr git https://github.com/ValveSoftware/openvr.git">

  <meta content="3082775" name="octolytics-dimension-user_id" /><meta content="ValveSoftware" name="octolytics-dimension-user_login" /><meta content="34289265" name="octolytics-dimension-repository_id" /><meta content="ValveSoftware/openvr" name="octolytics-dimension-repository_nwo" /><meta content="true" name="octolytics-dimension-repository_public" /><meta content="false" name="octolytics-dimension-repository_is_fork" /><meta content="34289265" name="octolytics-dimension-repository_network_root_id" /><meta content="ValveSoftware/openvr" name="octolytics-dimension-repository_network_root_nwo" />
  <link href="https://github.com/ValveSoftware/openvr/commits/master.atom" rel="alternate" title="Recent Commits to openvr:master" type="application/atom+xml">


      <link rel="canonical" href="https://github.com/ValveSoftware/openvr/blob/master/headers/openvr_capi.h" data-pjax-transient>
  </head>


  <body class="logged-in  env-production windows vis-public page-blob">
    <div id="js-pjax-loader-bar" class="pjax-loader-bar"><div class="progress"></div></div>
    <a href="#start-of-content" tabindex="1" class="accessibility-aid js-skip-to-content">Skip to content</a>

    
    
    



        <div class="header header-logged-in true" role="banner">
  <div class="container clearfix">

    <a class="header-logo-invertocat" href="https://github.com/" data-hotkey="g d" aria-label="Homepage" data-ga-click="Header, go to dashboard, icon:logo">
  <svg aria-hidden="true" class="octicon octicon-mark-github" height="28" version="1.1" viewBox="0 0 16 16" width="28"><path fill-rule="evenodd" d="M8 0C3.58 0 0 3.58 0 8c0 3.54 2.29 6.53 5.47 7.59.4.07.55-.17.55-.38 0-.19-.01-.82-.01-1.49-2.01.37-2.53-.49-2.69-.94-.09-.23-.48-.94-.82-1.13-.28-.15-.68-.52-.01-.53.63-.01 1.08.58 1.23.82.72 1.21 1.87.87 2.33.66.07-.52.28-.87.51-1.07-1.78-.2-3.64-.89-3.64-3.95 0-.87.31-1.59.82-2.15-.08-.2-.36-1.02.08-2.12 0 0 .67-.21 2.2.82.64-.18 1.32-.27 2-.27.68 0 1.36.09 2 .27 1.53-1.04 2.2-.82 2.2-.82.44 1.1.16 1.92.08 2.12.51.56.82 1.27.82 2.15 0 3.07-1.87 3.75-3.65 3.95.29.25.54.73.54 1.48 0 1.07-.01 1.93-.01 2.2 0 .21.15.46.55.38A8.013 8.013 0 0 0 16 8c0-4.42-3.58-8-8-8z"/></svg>
</a>


        <div class="header-search scoped-search site-scoped-search js-site-search" role="search">
  <!-- '"` --><!-- </textarea></xmp> --></option></form><form accept-charset="UTF-8" action="/ValveSoftware/openvr/search" class="js-site-search-form" data-scoped-search-url="/ValveSoftware/openvr/search" data-unscoped-search-url="/search" method="get"><div style="margin:0;padding:0;display:inline"><input name="utf8" type="hidden" value="&#x2713;" /></div>
    <label class="form-control header-search-wrapper js-chromeless-input-container">
      <div class="header-search-scope">This repository</div>
      <input type="text"
        class="form-control header-search-input js-site-search-focus js-site-search-field is-clearable"
        data-hotkey="s"
        name="q"
        placeholder="Search"
        aria-label="Search this repository"
        data-unscoped-placeholder="Search GitHub"
        data-scoped-placeholder="Search"
        autocapitalize="off">
    </label>
</form></div>


      <ul class="header-nav float-left" role="navigation">
        <li class="header-nav-item">
          <a href="/pulls" aria-label="Pull requests you created" class="js-selected-navigation-item header-nav-link" data-ga-click="Header, click, Nav menu - item:pulls context:user" data-hotkey="g p" data-selected-links="/pulls /pulls/assigned /pulls/mentioned /pulls">
            Pull requests
</a>        </li>
        <li class="header-nav-item">
          <a href="/issues" aria-label="Issues you created" class="js-selected-navigation-item header-nav-link" data-ga-click="Header, click, Nav menu - item:issues context:user" data-hotkey="g i" data-selected-links="/issues /issues/assigned /issues/mentioned /issues">
            Issues
</a>        </li>
          <li class="header-nav-item">
            <a class="header-nav-link" href="https://gist.github.com/" data-ga-click="Header, go to gist, text:gist">Gist</a>
          </li>
      </ul>

    
<ul class="header-nav user-nav float-right" id="user-links">
  <li class="header-nav-item">
    
    <a href="/notifications" aria-label="You have no unread notifications" class="header-nav-link notification-indicator tooltipped tooltipped-s js-socket-channel js-notification-indicator" data-channel="tenant:1:notification-changed:14998294" data-ga-click="Header, go to notifications, icon:read" data-hotkey="g n">
        <span class="mail-status "></span>
        <svg aria-hidden="true" class="octicon octicon-bell" height="16" version="1.1" viewBox="0 0 14 16" width="14"><path fill-rule="evenodd" d="M14 12v1H0v-1l.73-.58c.77-.77.81-2.55 1.19-4.42C2.69 3.23 6 2 6 2c0-.55.45-1 1-1s1 .45 1 1c0 0 3.39 1.23 4.16 5 .38 1.88.42 3.66 1.19 4.42l.66.58H14zm-7 4c1.11 0 2-.89 2-2H5c0 1.11.89 2 2 2z"/></svg>
</a>
  </li>

  <li class="header-nav-item dropdown js-menu-container">
    <a class="header-nav-link tooltipped tooltipped-s js-menu-target" href="/new"
       aria-label="Create new…"
       data-ga-click="Header, create new, icon:add">
      <svg aria-hidden="true" class="octicon octicon-plus float-left" height="16" version="1.1" viewBox="0 0 12 16" width="12"><path fill-rule="evenodd" d="M12 9H7v5H5V9H0V7h5V2h2v5h5z"/></svg>
      <span class="dropdown-caret"></span>
    </a>

    <div class="dropdown-menu-content js-menu-content">
      <ul class="dropdown-menu dropdown-menu-sw">
        
<a class="dropdown-item" href="/new" data-ga-click="Header, create new repository">
  New repository
</a>

  <a class="dropdown-item" href="/new/import" data-ga-click="Header, import a repository">
    Import repository
  </a>

<a class="dropdown-item" href="https://gist.github.com/" data-ga-click="Header, create new gist">
  New gist
</a>

  <a class="dropdown-item" href="/organizations/new" data-ga-click="Header, create new organization">
    New organization
  </a>



  <div class="dropdown-divider"></div>
  <div class="dropdown-header">
    <span title="ValveSoftware/openvr">This repository</span>
  </div>
    <a class="dropdown-item" href="/ValveSoftware/openvr/issues/new" data-ga-click="Header, create new issue">
      New issue
    </a>

      </ul>
    </div>
  </li>

  <li class="header-nav-item dropdown js-menu-container">
    <a class="header-nav-link name tooltipped tooltipped-sw js-menu-target" href="/mitchellmcm27"
       aria-label="View profile and more"
       data-ga-click="Header, show menu, icon:avatar">
      <img alt="@mitchellmcm27" class="avatar" height="20" src="https://avatars0.githubusercontent.com/u/14998294?v=3&amp;s=40" width="20" />
      <span class="dropdown-caret"></span>
    </a>

    <div class="dropdown-menu-content js-menu-content">
      <div class="dropdown-menu dropdown-menu-sw">
        <div class="dropdown-header header-nav-current-user css-truncate">
          Signed in as <strong class="css-truncate-target">mitchellmcm27</strong>
        </div>

        <div class="dropdown-divider"></div>

        <a class="dropdown-item" href="/mitchellmcm27" data-ga-click="Header, go to profile, text:your profile">
          Your profile
        </a>
        <a class="dropdown-item" href="/mitchellmcm27?tab=stars" data-ga-click="Header, go to starred repos, text:your stars">
          Your stars
        </a>
        <a class="dropdown-item" href="/explore" data-ga-click="Header, go to explore, text:explore">
          Explore
        </a>
          <a class="dropdown-item" href="/integrations" data-ga-click="Header, go to integrations, text:integrations">
            Integrations
          </a>
        <a class="dropdown-item" href="https://help.github.com" data-ga-click="Header, go to help, text:help">
          Help
        </a>

        <div class="dropdown-divider"></div>

        <a class="dropdown-item" href="/settings/profile" data-ga-click="Header, go to settings, icon:settings">
          Settings
        </a>

        <!-- '"` --><!-- </textarea></xmp> --></option></form><form accept-charset="UTF-8" action="/logout" class="logout-form" method="post"><div style="margin:0;padding:0;display:inline"><input name="utf8" type="hidden" value="&#x2713;" /><input name="authenticity_token" type="hidden" value="Xo1WDt814/t40/aVBiaArZg31m89W1EhsxIZBLEwCYqIxjCezt0C8wgJsTuX9cx0hUzUyEpPyudxaPBJln/PNw==" /></div>
          <button type="submit" class="dropdown-item dropdown-signout" data-ga-click="Header, sign out, icon:logout">
            Sign out
          </button>
</form>      </div>
    </div>
  </li>
</ul>


    
  </div>
</div>


      


    <div id="start-of-content" class="accessibility-aid"></div>

      <div id="js-flash-container">
</div>


    <div role="main">
        <div itemscope itemtype="http://schema.org/SoftwareSourceCode">
    <div id="js-repo-pjax-container" data-pjax-container>
      
<div class="pagehead repohead instapaper_ignore readability-menu experiment-repo-nav">
  <div class="container repohead-details-container">

    

<ul class="pagehead-actions">

  <li>
        <!-- '"` --><!-- </textarea></xmp> --></option></form><form accept-charset="UTF-8" action="/notifications/subscribe" class="js-social-container" data-autosubmit="true" data-remote="true" method="post"><div style="margin:0;padding:0;display:inline"><input name="utf8" type="hidden" value="&#x2713;" /><input name="authenticity_token" type="hidden" value="lwRd4tshmONumJu5lcssSywsPF9ur2l///V8fbjDlzYufSwqSlPDbztGokS/vofSEYHqUWSeSSKfJ77CQ6HAIw==" /></div>      <input class="form-control" id="repository_id" name="repository_id" type="hidden" value="34289265" />

        <div class="select-menu js-menu-container js-select-menu">
          <a href="/ValveSoftware/openvr/subscription"
            class="btn btn-sm btn-with-count select-menu-button js-menu-target" role="button" tabindex="0" aria-haspopup="true"
            data-ga-click="Repository, click Watch settings, action:blob#show">
            <span class="js-select-button">
              <svg aria-hidden="true" class="octicon octicon-eye" height="16" version="1.1" viewBox="0 0 16 16" width="16"><path fill-rule="evenodd" d="M8.06 2C3 2 0 8 0 8s3 6 8.06 6C13 14 16 8 16 8s-3-6-7.94-6zM8 12c-2.2 0-4-1.78-4-4 0-2.2 1.8-4 4-4 2.22 0 4 1.8 4 4 0 2.22-1.78 4-4 4zm2-4c0 1.11-.89 2-2 2-1.11 0-2-.89-2-2 0-1.11.89-2 2-2 1.11 0 2 .89 2 2z"/></svg>
              Watch
            </span>
          </a>
          <a class="social-count js-social-count"
            href="/ValveSoftware/openvr/watchers"
            aria-label="403 users are watching this repository">
            403
          </a>

        <div class="select-menu-modal-holder">
          <div class="select-menu-modal subscription-menu-modal js-menu-content" aria-hidden="true">
            <div class="select-menu-header js-navigation-enable" tabindex="-1">
              <svg aria-label="Close" class="octicon octicon-x js-menu-close" height="16" role="img" version="1.1" viewBox="0 0 12 16" width="12"><path fill-rule="evenodd" d="M7.48 8l3.75 3.75-1.48 1.48L6 9.48l-3.75 3.75-1.48-1.48L4.52 8 .77 4.25l1.48-1.48L6 6.52l3.75-3.75 1.48 1.48z"/></svg>
              <span class="select-menu-title">Notifications</span>
            </div>

              <div class="select-menu-list js-navigation-container" role="menu">

                <div class="select-menu-item js-navigation-item selected" role="menuitem" tabindex="0">
                  <svg aria-hidden="true" class="octicon octicon-check select-menu-item-icon" height="16" version="1.1" viewBox="0 0 12 16" width="12"><path fill-rule="evenodd" d="M12 5l-8 8-4-4 1.5-1.5L4 10l6.5-6.5z"/></svg>
                  <div class="select-menu-item-text">
                    <input checked="checked" id="do_included" name="do" type="radio" value="included" />
                    <span class="select-menu-item-heading">Not watching</span>
                    <span class="description">Be notified when participating or @mentioned.</span>
                    <span class="js-select-button-text hidden-select-button-text">
                      <svg aria-hidden="true" class="octicon octicon-eye" height="16" version="1.1" viewBox="0 0 16 16" width="16"><path fill-rule="evenodd" d="M8.06 2C3 2 0 8 0 8s3 6 8.06 6C13 14 16 8 16 8s-3-6-7.94-6zM8 12c-2.2 0-4-1.78-4-4 0-2.2 1.8-4 4-4 2.22 0 4 1.8 4 4 0 2.22-1.78 4-4 4zm2-4c0 1.11-.89 2-2 2-1.11 0-2-.89-2-2 0-1.11.89-2 2-2 1.11 0 2 .89 2 2z"/></svg>
                      Watch
                    </span>
                  </div>
                </div>

                <div class="select-menu-item js-navigation-item " role="menuitem" tabindex="0">
                  <svg aria-hidden="true" class="octicon octicon-check select-menu-item-icon" height="16" version="1.1" viewBox="0 0 12 16" width="12"><path fill-rule="evenodd" d="M12 5l-8 8-4-4 1.5-1.5L4 10l6.5-6.5z"/></svg>
                  <div class="select-menu-item-text">
                    <input id="do_subscribed" name="do" type="radio" value="subscribed" />
                    <span class="select-menu-item-heading">Watching</span>
                    <span class="description">Be notified of all conversations.</span>
                    <span class="js-select-button-text hidden-select-button-text">
                      <svg aria-hidden="true" class="octicon octicon-eye" height="16" version="1.1" viewBox="0 0 16 16" width="16"><path fill-rule="evenodd" d="M8.06 2C3 2 0 8 0 8s3 6 8.06 6C13 14 16 8 16 8s-3-6-7.94-6zM8 12c-2.2 0-4-1.78-4-4 0-2.2 1.8-4 4-4 2.22 0 4 1.8 4 4 0 2.22-1.78 4-4 4zm2-4c0 1.11-.89 2-2 2-1.11 0-2-.89-2-2 0-1.11.89-2 2-2 1.11 0 2 .89 2 2z"/></svg>
                      Unwatch
                    </span>
                  </div>
                </div>

                <div class="select-menu-item js-navigation-item " role="menuitem" tabindex="0">
                  <svg aria-hidden="true" class="octicon octicon-check select-menu-item-icon" height="16" version="1.1" viewBox="0 0 12 16" width="12"><path fill-rule="evenodd" d="M12 5l-8 8-4-4 1.5-1.5L4 10l6.5-6.5z"/></svg>
                  <div class="select-menu-item-text">
                    <input id="do_ignore" name="do" type="radio" value="ignore" />
                    <span class="select-menu-item-heading">Ignoring</span>
                    <span class="description">Never be notified.</span>
                    <span class="js-select-button-text hidden-select-button-text">
                      <svg aria-hidden="true" class="octicon octicon-mute" height="16" version="1.1" viewBox="0 0 16 16" width="16"><path fill-rule="evenodd" d="M8 2.81v10.38c0 .67-.81 1-1.28.53L3 10H1c-.55 0-1-.45-1-1V7c0-.55.45-1 1-1h2l3.72-3.72C7.19 1.81 8 2.14 8 2.81zm7.53 3.22l-1.06-1.06-1.97 1.97-1.97-1.97-1.06 1.06L11.44 8 9.47 9.97l1.06 1.06 1.97-1.97 1.97 1.97 1.06-1.06L13.56 8l1.97-1.97z"/></svg>
                      Stop ignoring
                    </span>
                  </div>
                </div>

              </div>

            </div>
          </div>
        </div>
</form>
  </li>

  <li>
      <div class="js-toggler-container js-social-container starring-container ">
    <!-- '"` --><!-- </textarea></xmp> --></option></form><form accept-charset="UTF-8" action="/ValveSoftware/openvr/unstar" class="starred" data-remote="true" method="post"><div style="margin:0;padding:0;display:inline"><input name="utf8" type="hidden" value="&#x2713;" /><input name="authenticity_token" type="hidden" value="84EOL5KZc7MBeoQbcw0d6sG0Er08exzjdHuiVG7410vsQR9+VLXSKwSR3hLTdUULL+iCnChx4+sFLbmXW7iSKw==" /></div>
      <button
        type="submit"
        class="btn btn-sm btn-with-count js-toggler-target"
        aria-label="Unstar this repository" title="Unstar ValveSoftware/openvr"
        data-ga-click="Repository, click unstar button, action:blob#show; text:Unstar">
        <svg aria-hidden="true" class="octicon octicon-star" height="16" version="1.1" viewBox="0 0 14 16" width="14"><path fill-rule="evenodd" d="M14 6l-4.9-.64L7 1 4.9 5.36 0 6l3.6 3.26L2.67 14 7 11.67 11.33 14l-.93-4.74z"/></svg>
        Unstar
      </button>
        <a class="social-count js-social-count" href="/ValveSoftware/openvr/stargazers"
           aria-label="2119 users starred this repository">
          2,119
        </a>
</form>
    <!-- '"` --><!-- </textarea></xmp> --></option></form><form accept-charset="UTF-8" action="/ValveSoftware/openvr/star" class="unstarred" data-remote="true" method="post"><div style="margin:0;padding:0;display:inline"><input name="utf8" type="hidden" value="&#x2713;" /><input name="authenticity_token" type="hidden" value="LEfN4Dy5Cd7mWF2QOAASELoyqiJGKWw1MBrp6zpVKzARL+urY6UZyI1DyTaGpohDoRvxQuibGKxtJ1+u6SiDcA==" /></div>
      <button
        type="submit"
        class="btn btn-sm btn-with-count js-toggler-target"
        aria-label="Star this repository" title="Star ValveSoftware/openvr"
        data-ga-click="Repository, click star button, action:blob#show; text:Star">
        <svg aria-hidden="true" class="octicon octicon-star" height="16" version="1.1" viewBox="0 0 14 16" width="14"><path fill-rule="evenodd" d="M14 6l-4.9-.64L7 1 4.9 5.36 0 6l3.6 3.26L2.67 14 7 11.67 11.33 14l-.93-4.74z"/></svg>
        Star
      </button>
        <a class="social-count js-social-count" href="/ValveSoftware/openvr/stargazers"
           aria-label="2119 users starred this repository">
          2,119
        </a>
</form>  </div>

  </li>

  <li>
          <!-- '"` --><!-- </textarea></xmp> --></option></form><form accept-charset="UTF-8" action="/ValveSoftware/openvr/fork" class="btn-with-count" method="post"><div style="margin:0;padding:0;display:inline"><input name="utf8" type="hidden" value="&#x2713;" /><input name="authenticity_token" type="hidden" value="idHJ5h+QdnPCiKxW8Q6k5RUMpLaIFy5aB+/w9OOYt963+m6q7rV7RrhJkPxKxLHcSGFPhLYR65IeLPjUX81gjw==" /></div>
            <button
                type="submit"
                class="btn btn-sm btn-with-count"
                data-ga-click="Repository, show fork modal, action:blob#show; text:Fork"
                title="Fork your own copy of ValveSoftware/openvr to your account"
                aria-label="Fork your own copy of ValveSoftware/openvr to your account">
              <svg aria-hidden="true" class="octicon octicon-repo-forked" height="16" version="1.1" viewBox="0 0 10 16" width="10"><path fill-rule="evenodd" d="M8 1a1.993 1.993 0 0 0-1 3.72V6L5 8 3 6V4.72A1.993 1.993 0 0 0 2 1a1.993 1.993 0 0 0-1 3.72V6.5l3 3v1.78A1.993 1.993 0 0 0 5 15a1.993 1.993 0 0 0 1-3.72V9.5l3-3V4.72A1.993 1.993 0 0 0 8 1zM2 4.2C1.34 4.2.8 3.65.8 3c0-.65.55-1.2 1.2-1.2.65 0 1.2.55 1.2 1.2 0 .65-.55 1.2-1.2 1.2zm3 10c-.66 0-1.2-.55-1.2-1.2 0-.65.55-1.2 1.2-1.2.65 0 1.2.55 1.2 1.2 0 .65-.55 1.2-1.2 1.2zm3-10c-.66 0-1.2-.55-1.2-1.2 0-.65.55-1.2 1.2-1.2.65 0 1.2.55 1.2 1.2 0 .65-.55 1.2-1.2 1.2z"/></svg>
              Fork
            </button>
</form>
    <a href="/ValveSoftware/openvr/network" class="social-count"
       aria-label="551 users forked this repository">
      551
    </a>
  </li>
</ul>

    <h1 class="public ">
  <svg aria-hidden="true" class="octicon octicon-repo" height="16" version="1.1" viewBox="0 0 12 16" width="12"><path fill-rule="evenodd" d="M4 9H3V8h1v1zm0-3H3v1h1V6zm0-2H3v1h1V4zm0-2H3v1h1V2zm8-1v12c0 .55-.45 1-1 1H6v2l-1.5-1.5L3 16v-2H1c-.55 0-1-.45-1-1V1c0-.55.45-1 1-1h10c.55 0 1 .45 1 1zm-1 10H1v2h2v-1h3v1h5v-2zm0-10H2v9h9V1z"/></svg>
  <span class="author" itemprop="author"><a href="/ValveSoftware" class="url fn" rel="author">ValveSoftware</a></span><!--
--><span class="path-divider">/</span><!--
--><strong itemprop="name"><a href="/ValveSoftware/openvr" data-pjax="#js-repo-pjax-container">openvr</a></strong>

</h1>

  </div>
  <div class="container">
    
<nav class="reponav js-repo-nav js-sidenav-container-pjax"
     itemscope
     itemtype="http://schema.org/BreadcrumbList"
     role="navigation"
     data-pjax="#js-repo-pjax-container">

  <span itemscope itemtype="http://schema.org/ListItem" itemprop="itemListElement">
    <a href="/ValveSoftware/openvr" class="js-selected-navigation-item selected reponav-item" data-hotkey="g c" data-selected-links="repo_source repo_downloads repo_commits repo_releases repo_tags repo_branches /ValveSoftware/openvr" itemprop="url">
      <svg aria-hidden="true" class="octicon octicon-code" height="16" version="1.1" viewBox="0 0 14 16" width="14"><path fill-rule="evenodd" d="M9.5 3L8 4.5 11.5 8 8 11.5 9.5 13 14 8 9.5 3zm-5 0L0 8l4.5 5L6 11.5 2.5 8 6 4.5 4.5 3z"/></svg>
      <span itemprop="name">Code</span>
      <meta itemprop="position" content="1">
</a>  </span>

    <span itemscope itemtype="http://schema.org/ListItem" itemprop="itemListElement">
      <a href="/ValveSoftware/openvr/issues" class="js-selected-navigation-item reponav-item" data-hotkey="g i" data-selected-links="repo_issues repo_labels repo_milestones /ValveSoftware/openvr/issues" itemprop="url">
        <svg aria-hidden="true" class="octicon octicon-issue-opened" height="16" version="1.1" viewBox="0 0 14 16" width="14"><path fill-rule="evenodd" d="M7 2.3c3.14 0 5.7 2.56 5.7 5.7s-2.56 5.7-5.7 5.7A5.71 5.71 0 0 1 1.3 8c0-3.14 2.56-5.7 5.7-5.7zM7 1C3.14 1 0 4.14 0 8s3.14 7 7 7 7-3.14 7-7-3.14-7-7-7zm1 3H6v5h2V4zm0 6H6v2h2v-2z"/></svg>
        <span itemprop="name">Issues</span>
        <span class="counter">187</span>
        <meta itemprop="position" content="2">
</a>    </span>

  <span itemscope itemtype="http://schema.org/ListItem" itemprop="itemListElement">
    <a href="/ValveSoftware/openvr/pulls" class="js-selected-navigation-item reponav-item" data-hotkey="g p" data-selected-links="repo_pulls /ValveSoftware/openvr/pulls" itemprop="url">
      <svg aria-hidden="true" class="octicon octicon-git-pull-request" height="16" version="1.1" viewBox="0 0 12 16" width="12"><path fill-rule="evenodd" d="M11 11.28V5c-.03-.78-.34-1.47-.94-2.06C9.46 2.35 8.78 2.03 8 2H7V0L4 3l3 3V4h1c.27.02.48.11.69.31.21.2.3.42.31.69v6.28A1.993 1.993 0 0 0 10 15a1.993 1.993 0 0 0 1-3.72zm-1 2.92c-.66 0-1.2-.55-1.2-1.2 0-.65.55-1.2 1.2-1.2.65 0 1.2.55 1.2 1.2 0 .65-.55 1.2-1.2 1.2zM4 3c0-1.11-.89-2-2-2a1.993 1.993 0 0 0-1 3.72v6.56A1.993 1.993 0 0 0 2 15a1.993 1.993 0 0 0 1-3.72V4.72c.59-.34 1-.98 1-1.72zm-.8 10c0 .66-.55 1.2-1.2 1.2-.65 0-1.2-.55-1.2-1.2 0-.65.55-1.2 1.2-1.2.65 0 1.2.55 1.2 1.2zM2 4.2C1.34 4.2.8 3.65.8 3c0-.65.55-1.2 1.2-1.2.65 0 1.2.55 1.2 1.2 0 .65-.55 1.2-1.2 1.2z"/></svg>
      <span itemprop="name">Pull requests</span>
      <span class="counter">6</span>
      <meta itemprop="position" content="3">
</a>  </span>

  <a href="/ValveSoftware/openvr/projects" class="js-selected-navigation-item reponav-item" data-selected-links="repo_projects new_repo_project repo_project /ValveSoftware/openvr/projects">
    <svg aria-hidden="true" class="octicon octicon-project" height="16" version="1.1" viewBox="0 0 15 16" width="15"><path fill-rule="evenodd" d="M10 12h3V2h-3v10zm-4-2h3V2H6v8zm-4 4h3V2H2v12zm-1 1h13V1H1v14zM14 0H1a1 1 0 0 0-1 1v14a1 1 0 0 0 1 1h13a1 1 0 0 0 1-1V1a1 1 0 0 0-1-1z"/></svg>
    Projects
    <span class="counter">0</span>
</a>
    <a href="/ValveSoftware/openvr/wiki" class="js-selected-navigation-item reponav-item" data-hotkey="g w" data-selected-links="repo_wiki /ValveSoftware/openvr/wiki">
      <svg aria-hidden="true" class="octicon octicon-book" height="16" version="1.1" viewBox="0 0 16 16" width="16"><path fill-rule="evenodd" d="M3 5h4v1H3V5zm0 3h4V7H3v1zm0 2h4V9H3v1zm11-5h-4v1h4V5zm0 2h-4v1h4V7zm0 2h-4v1h4V9zm2-6v9c0 .55-.45 1-1 1H9.5l-1 1-1-1H2c-.55 0-1-.45-1-1V3c0-.55.45-1 1-1h5.5l1 1 1-1H15c.55 0 1 .45 1 1zm-8 .5L7.5 3H2v9h6V3.5zm7-.5H9.5l-.5.5V12h6V3z"/></svg>
      Wiki
</a>

  <a href="/ValveSoftware/openvr/pulse" class="js-selected-navigation-item reponav-item" data-selected-links="pulse /ValveSoftware/openvr/pulse">
    <svg aria-hidden="true" class="octicon octicon-pulse" height="16" version="1.1" viewBox="0 0 14 16" width="14"><path fill-rule="evenodd" d="M11.5 8L8.8 5.4 6.6 8.5 5.5 1.6 2.38 8H0v2h3.6l.9-1.8.9 5.4L9 8.5l1.6 1.5H14V8z"/></svg>
    Pulse
</a>
  <a href="/ValveSoftware/openvr/graphs" class="js-selected-navigation-item reponav-item" data-selected-links="repo_graphs repo_contributors /ValveSoftware/openvr/graphs">
    <svg aria-hidden="true" class="octicon octicon-graph" height="16" version="1.1" viewBox="0 0 16 16" width="16"><path fill-rule="evenodd" d="M16 14v1H0V0h1v14h15zM5 13H3V8h2v5zm4 0H7V3h2v10zm4 0h-2V6h2v7z"/></svg>
    Graphs
</a>

</nav>

  </div>
</div>

<div class="container new-discussion-timeline experiment-repo-nav">
  <div class="repository-content">

    

<a href="/ValveSoftware/openvr/blob/b96746068239d5571a4d1ff77577517cc4ca1326/headers/openvr_capi.h" class="d-none js-permalink-shortcut" data-hotkey="y">Permalink</a>

<!-- blob contrib key: blob_contributors:v21:082722c46b2508b25454e72282eec51c -->

<div class="file-navigation js-zeroclipboard-container">
  
<div class="select-menu branch-select-menu js-menu-container js-select-menu float-left">
  <button class="btn btn-sm select-menu-button js-menu-target css-truncate" data-hotkey="w"
    
    type="button" aria-label="Switch branches or tags" tabindex="0" aria-haspopup="true">
    <i>Branch:</i>
    <span class="js-select-button css-truncate-target">master</span>
  </button>

  <div class="select-menu-modal-holder js-menu-content js-navigation-container" data-pjax aria-hidden="true">

    <div class="select-menu-modal">
      <div class="select-menu-header">
        <svg aria-label="Close" class="octicon octicon-x js-menu-close" height="16" role="img" version="1.1" viewBox="0 0 12 16" width="12"><path fill-rule="evenodd" d="M7.48 8l3.75 3.75-1.48 1.48L6 9.48l-3.75 3.75-1.48-1.48L4.52 8 .77 4.25l1.48-1.48L6 6.52l3.75-3.75 1.48 1.48z"/></svg>
        <span class="select-menu-title">Switch branches/tags</span>
      </div>

      <div class="select-menu-filters">
        <div class="select-menu-text-filter">
          <input type="text" aria-label="Filter branches/tags" id="context-commitish-filter-field" class="form-control js-filterable-field js-navigation-enable" placeholder="Filter branches/tags">
        </div>
        <div class="select-menu-tabs">
          <ul>
            <li class="select-menu-tab">
              <a href="#" data-tab-filter="branches" data-filter-placeholder="Filter branches/tags" class="js-select-menu-tab" role="tab">Branches</a>
            </li>
            <li class="select-menu-tab">
              <a href="#" data-tab-filter="tags" data-filter-placeholder="Find a tag…" class="js-select-menu-tab" role="tab">Tags</a>
            </li>
          </ul>
        </div>
      </div>

      <div class="select-menu-list select-menu-tab-bucket js-select-menu-tab-bucket" data-tab-filter="branches" role="menu">

        <div data-filterable-for="context-commitish-filter-field" data-filterable-type="substring">


            <a class="select-menu-item js-navigation-item js-navigation-open selected"
               href="/ValveSoftware/openvr/blob/master/headers/openvr_capi.h"
               data-name="master"
               data-skip-pjax="true"
               rel="nofollow">
              <svg aria-hidden="true" class="octicon octicon-check select-menu-item-icon" height="16" version="1.1" viewBox="0 0 12 16" width="12"><path fill-rule="evenodd" d="M12 5l-8 8-4-4 1.5-1.5L4 10l6.5-6.5z"/></svg>
              <span class="select-menu-item-text css-truncate-target js-select-menu-filter-text">
                master
              </span>
            </a>
        </div>

          <div class="select-menu-no-results">Nothing to show</div>
      </div>

      <div class="select-menu-list select-menu-tab-bucket js-select-menu-tab-bucket" data-tab-filter="tags">
        <div data-filterable-for="context-commitish-filter-field" data-filterable-type="substring">


            <a class="select-menu-item js-navigation-item js-navigation-open "
              href="/ValveSoftware/openvr/tree/v1.0.5/headers/openvr_capi.h"
              data-name="v1.0.5"
              data-skip-pjax="true"
              rel="nofollow">
              <svg aria-hidden="true" class="octicon octicon-check select-menu-item-icon" height="16" version="1.1" viewBox="0 0 12 16" width="12"><path fill-rule="evenodd" d="M12 5l-8 8-4-4 1.5-1.5L4 10l6.5-6.5z"/></svg>
              <span class="select-menu-item-text css-truncate-target" title="v1.0.5">
                v1.0.5
              </span>
            </a>
            <a class="select-menu-item js-navigation-item js-navigation-open "
              href="/ValveSoftware/openvr/tree/v1.0.4/headers/openvr_capi.h"
              data-name="v1.0.4"
              data-skip-pjax="true"
              rel="nofollow">
              <svg aria-hidden="true" class="octicon octicon-check select-menu-item-icon" height="16" version="1.1" viewBox="0 0 12 16" width="12"><path fill-rule="evenodd" d="M12 5l-8 8-4-4 1.5-1.5L4 10l6.5-6.5z"/></svg>
              <span class="select-menu-item-text css-truncate-target" title="v1.0.4">
                v1.0.4
              </span>
            </a>
            <a class="select-menu-item js-navigation-item js-navigation-open "
              href="/ValveSoftware/openvr/tree/v1.0.3/headers/openvr_capi.h"
              data-name="v1.0.3"
              data-skip-pjax="true"
              rel="nofollow">
              <svg aria-hidden="true" class="octicon octicon-check select-menu-item-icon" height="16" version="1.1" viewBox="0 0 12 16" width="12"><path fill-rule="evenodd" d="M12 5l-8 8-4-4 1.5-1.5L4 10l6.5-6.5z"/></svg>
              <span class="select-menu-item-text css-truncate-target" title="v1.0.3">
                v1.0.3
              </span>
            </a>
            <a class="select-menu-item js-navigation-item js-navigation-open "
              href="/ValveSoftware/openvr/tree/v1.0.2/headers/openvr_capi.h"
              data-name="v1.0.2"
              data-skip-pjax="true"
              rel="nofollow">
              <svg aria-hidden="true" class="octicon octicon-check select-menu-item-icon" height="16" version="1.1" viewBox="0 0 12 16" width="12"><path fill-rule="evenodd" d="M12 5l-8 8-4-4 1.5-1.5L4 10l6.5-6.5z"/></svg>
              <span class="select-menu-item-text css-truncate-target" title="v1.0.2">
                v1.0.2
              </span>
            </a>
            <a class="select-menu-item js-navigation-item js-navigation-open "
              href="/ValveSoftware/openvr/tree/v1.0.1/headers/openvr_capi.h"
              data-name="v1.0.1"
              data-skip-pjax="true"
              rel="nofollow">
              <svg aria-hidden="true" class="octicon octicon-check select-menu-item-icon" height="16" version="1.1" viewBox="0 0 12 16" width="12"><path fill-rule="evenodd" d="M12 5l-8 8-4-4 1.5-1.5L4 10l6.5-6.5z"/></svg>
              <span class="select-menu-item-text css-truncate-target" title="v1.0.1">
                v1.0.1
              </span>
            </a>
            <a class="select-menu-item js-navigation-item js-navigation-open "
              href="/ValveSoftware/openvr/tree/v1.0.0/headers/openvr_capi.h"
              data-name="v1.0.0"
              data-skip-pjax="true"
              rel="nofollow">
              <svg aria-hidden="true" class="octicon octicon-check select-menu-item-icon" height="16" version="1.1" viewBox="0 0 12 16" width="12"><path fill-rule="evenodd" d="M12 5l-8 8-4-4 1.5-1.5L4 10l6.5-6.5z"/></svg>
              <span class="select-menu-item-text css-truncate-target" title="v1.0.0">
                v1.0.0
              </span>
            </a>
            <a class="select-menu-item js-navigation-item js-navigation-open "
              href="/ValveSoftware/openvr/tree/v0.9.20/headers/openvr_capi.h"
              data-name="v0.9.20"
              data-skip-pjax="true"
              rel="nofollow">
              <svg aria-hidden="true" class="octicon octicon-check select-menu-item-icon" height="16" version="1.1" viewBox="0 0 12 16" width="12"><path fill-rule="evenodd" d="M12 5l-8 8-4-4 1.5-1.5L4 10l6.5-6.5z"/></svg>
              <span class="select-menu-item-text css-truncate-target" title="v0.9.20">
                v0.9.20
              </span>
            </a>
            <a class="select-menu-item js-navigation-item js-navigation-open "
              href="/ValveSoftware/openvr/tree/v0.9.19/headers/openvr_capi.h"
              data-name="v0.9.19"
              data-skip-pjax="true"
              rel="nofollow">
              <svg aria-hidden="true" class="octicon octicon-check select-menu-item-icon" height="16" version="1.1" viewBox="0 0 12 16" width="12"><path fill-rule="evenodd" d="M12 5l-8 8-4-4 1.5-1.5L4 10l6.5-6.5z"/></svg>
              <span class="select-menu-item-text css-truncate-target" title="v0.9.19">
                v0.9.19
              </span>
            </a>
            <a class="select-menu-item js-navigation-item js-navigation-open "
              href="/ValveSoftware/openvr/tree/v0.9.18/headers/openvr_capi.h"
              data-name="v0.9.18"
              data-skip-pjax="true"
              rel="nofollow">
              <svg aria-hidden="true" class="octicon octicon-check select-menu-item-icon" height="16" version="1.1" viewBox="0 0 12 16" width="12"><path fill-rule="evenodd" d="M12 5l-8 8-4-4 1.5-1.5L4 10l6.5-6.5z"/></svg>
              <span class="select-menu-item-text css-truncate-target" title="v0.9.18">
                v0.9.18
              </span>
            </a>
            <a class="select-menu-item js-navigation-item js-navigation-open "
              href="/ValveSoftware/openvr/tree/v0.9.17/headers/openvr_capi.h"
              data-name="v0.9.17"
              data-skip-pjax="true"
              rel="nofollow">
              <svg aria-hidden="true" class="octicon octicon-check select-menu-item-icon" height="16" version="1.1" viewBox="0 0 12 16" width="12"><path fill-rule="evenodd" d="M12 5l-8 8-4-4 1.5-1.5L4 10l6.5-6.5z"/></svg>
              <span class="select-menu-item-text css-truncate-target" title="v0.9.17">
                v0.9.17
              </span>
            </a>
            <a class="select-menu-item js-navigation-item js-navigation-open "
              href="/ValveSoftware/openvr/tree/v0.9.16/headers/openvr_capi.h"
              data-name="v0.9.16"
              data-skip-pjax="true"
              rel="nofollow">
              <svg aria-hidden="true" class="octicon octicon-check select-menu-item-icon" height="16" version="1.1" viewBox="0 0 12 16" width="12"><path fill-rule="evenodd" d="M12 5l-8 8-4-4 1.5-1.5L4 10l6.5-6.5z"/></svg>
              <span class="select-menu-item-text css-truncate-target" title="v0.9.16">
                v0.9.16
              </span>
            </a>
            <a class="select-menu-item js-navigation-item js-navigation-open "
              href="/ValveSoftware/openvr/tree/v0.9.15/headers/openvr_capi.h"
              data-name="v0.9.15"
              data-skip-pjax="true"
              rel="nofollow">
              <svg aria-hidden="true" class="octicon octicon-check select-menu-item-icon" height="16" version="1.1" viewBox="0 0 12 16" width="12"><path fill-rule="evenodd" d="M12 5l-8 8-4-4 1.5-1.5L4 10l6.5-6.5z"/></svg>
              <span class="select-menu-item-text css-truncate-target" title="v0.9.15">
                v0.9.15
              </span>
            </a>
            <a class="select-menu-item js-navigation-item js-navigation-open "
              href="/ValveSoftware/openvr/tree/v0.9.14/headers/openvr_capi.h"
              data-name="v0.9.14"
              data-skip-pjax="true"
              rel="nofollow">
              <svg aria-hidden="true" class="octicon octicon-check select-menu-item-icon" height="16" version="1.1" viewBox="0 0 12 16" width="12"><path fill-rule="evenodd" d="M12 5l-8 8-4-4 1.5-1.5L4 10l6.5-6.5z"/></svg>
              <span class="select-menu-item-text css-truncate-target" title="v0.9.14">
                v0.9.14
              </span>
            </a>
            <a class="select-menu-item js-navigation-item js-navigation-open "
              href="/ValveSoftware/openvr/tree/v0.9.13/headers/openvr_capi.h"
              data-name="v0.9.13"
              data-skip-pjax="true"
              rel="nofollow">
              <svg aria-hidden="true" class="octicon octicon-check select-menu-item-icon" height="16" version="1.1" viewBox="0 0 12 16" width="12"><path fill-rule="evenodd" d="M12 5l-8 8-4-4 1.5-1.5L4 10l6.5-6.5z"/></svg>
              <span class="select-menu-item-text css-truncate-target" title="v0.9.13">
                v0.9.13
              </span>
            </a>
            <a class="select-menu-item js-navigation-item js-navigation-open "
              href="/ValveSoftware/openvr/tree/v0.9.12/headers/openvr_capi.h"
              data-name="v0.9.12"
              data-skip-pjax="true"
              rel="nofollow">
              <svg aria-hidden="true" class="octicon octicon-check select-menu-item-icon" height="16" version="1.1" viewBox="0 0 12 16" width="12"><path fill-rule="evenodd" d="M12 5l-8 8-4-4 1.5-1.5L4 10l6.5-6.5z"/></svg>
              <span class="select-menu-item-text css-truncate-target" title="v0.9.12">
                v0.9.12
              </span>
            </a>
            <a class="select-menu-item js-navigation-item js-navigation-open "
              href="/ValveSoftware/openvr/tree/v0.9.11/headers/openvr_capi.h"
              data-name="v0.9.11"
              data-skip-pjax="true"
              rel="nofollow">
              <svg aria-hidden="true" class="octicon octicon-check select-menu-item-icon" height="16" version="1.1" viewBox="0 0 12 16" width="12"><path fill-rule="evenodd" d="M12 5l-8 8-4-4 1.5-1.5L4 10l6.5-6.5z"/></svg>
              <span class="select-menu-item-text css-truncate-target" title="v0.9.11">
                v0.9.11
              </span>
            </a>
            <a class="select-menu-item js-navigation-item js-navigation-open "
              href="/ValveSoftware/openvr/tree/v0.9.10/headers/openvr_capi.h"
              data-name="v0.9.10"
              data-skip-pjax="true"
              rel="nofollow">
              <svg aria-hidden="true" class="octicon octicon-check select-menu-item-icon" height="16" version="1.1" viewBox="0 0 12 16" width="12"><path fill-rule="evenodd" d="M12 5l-8 8-4-4 1.5-1.5L4 10l6.5-6.5z"/></svg>
              <span class="select-menu-item-text css-truncate-target" title="v0.9.10">
                v0.9.10
              </span>
            </a>
            <a class="select-menu-item js-navigation-item js-navigation-open "
              href="/ValveSoftware/openvr/tree/v0.9.9/headers/openvr_capi.h"
              data-name="v0.9.9"
              data-skip-pjax="true"
              rel="nofollow">
              <svg aria-hidden="true" class="octicon octicon-check select-menu-item-icon" height="16" version="1.1" viewBox="0 0 12 16" width="12"><path fill-rule="evenodd" d="M12 5l-8 8-4-4 1.5-1.5L4 10l6.5-6.5z"/></svg>
              <span class="select-menu-item-text css-truncate-target" title="v0.9.9">
                v0.9.9
              </span>
            </a>
            <a class="select-menu-item js-navigation-item js-navigation-open "
              href="/ValveSoftware/openvr/tree/v0.9.8/headers/openvr_capi.h"
              data-name="v0.9.8"
              data-skip-pjax="true"
              rel="nofollow">
              <svg aria-hidden="true" class="octicon octicon-check select-menu-item-icon" height="16" version="1.1" viewBox="0 0 12 16" width="12"><path fill-rule="evenodd" d="M12 5l-8 8-4-4 1.5-1.5L4 10l6.5-6.5z"/></svg>
              <span class="select-menu-item-text css-truncate-target" title="v0.9.8">
                v0.9.8
              </span>
            </a>
            <a class="select-menu-item js-navigation-item js-navigation-open "
              href="/ValveSoftware/openvr/tree/v0.9.7/headers/openvr_capi.h"
              data-name="v0.9.7"
              data-skip-pjax="true"
              rel="nofollow">
              <svg aria-hidden="true" class="octicon octicon-check select-menu-item-icon" height="16" version="1.1" viewBox="0 0 12 16" width="12"><path fill-rule="evenodd" d="M12 5l-8 8-4-4 1.5-1.5L4 10l6.5-6.5z"/></svg>
              <span class="select-menu-item-text css-truncate-target" title="v0.9.7">
                v0.9.7
              </span>
            </a>
            <a class="select-menu-item js-navigation-item js-navigation-open "
              href="/ValveSoftware/openvr/tree/v0.9.6/headers/openvr_capi.h"
              data-name="v0.9.6"
              data-skip-pjax="true"
              rel="nofollow">
              <svg aria-hidden="true" class="octicon octicon-check select-menu-item-icon" height="16" version="1.1" viewBox="0 0 12 16" width="12"><path fill-rule="evenodd" d="M12 5l-8 8-4-4 1.5-1.5L4 10l6.5-6.5z"/></svg>
              <span class="select-menu-item-text css-truncate-target" title="v0.9.6">
                v0.9.6
              </span>
            </a>
            <a class="select-menu-item js-navigation-item js-navigation-open "
              href="/ValveSoftware/openvr/tree/v0.9.5/headers/openvr_capi.h"
              data-name="v0.9.5"
              data-skip-pjax="true"
              rel="nofollow">
              <svg aria-hidden="true" class="octicon octicon-check select-menu-item-icon" height="16" version="1.1" viewBox="0 0 12 16" width="12"><path fill-rule="evenodd" d="M12 5l-8 8-4-4 1.5-1.5L4 10l6.5-6.5z"/></svg>
              <span class="select-menu-item-text css-truncate-target" title="v0.9.5">
                v0.9.5
              </span>
            </a>
            <a class="select-menu-item js-navigation-item js-navigation-open "
              href="/ValveSoftware/openvr/tree/v0.9.4/headers/openvr_capi.h"
              data-name="v0.9.4"
              data-skip-pjax="true"
              rel="nofollow">
              <svg aria-hidden="true" class="octicon octicon-check select-menu-item-icon" height="16" version="1.1" viewBox="0 0 12 16" width="12"><path fill-rule="evenodd" d="M12 5l-8 8-4-4 1.5-1.5L4 10l6.5-6.5z"/></svg>
              <span class="select-menu-item-text css-truncate-target" title="v0.9.4">
                v0.9.4
              </span>
            </a>
            <a class="select-menu-item js-navigation-item js-navigation-open "
              href="/ValveSoftware/openvr/tree/0.9.13/headers/openvr_capi.h"
              data-name="0.9.13"
              data-skip-pjax="true"
              rel="nofollow">
              <svg aria-hidden="true" class="octicon octicon-check select-menu-item-icon" height="16" version="1.1" viewBox="0 0 12 16" width="12"><path fill-rule="evenodd" d="M12 5l-8 8-4-4 1.5-1.5L4 10l6.5-6.5z"/></svg>
              <span class="select-menu-item-text css-truncate-target" title="0.9.13">
                0.9.13
              </span>
            </a>
            <a class="select-menu-item js-navigation-item js-navigation-open "
              href="/ValveSoftware/openvr/tree/0.9.3/headers/openvr_capi.h"
              data-name="0.9.3"
              data-skip-pjax="true"
              rel="nofollow">
              <svg aria-hidden="true" class="octicon octicon-check select-menu-item-icon" height="16" version="1.1" viewBox="0 0 12 16" width="12"><path fill-rule="evenodd" d="M12 5l-8 8-4-4 1.5-1.5L4 10l6.5-6.5z"/></svg>
              <span class="select-menu-item-text css-truncate-target" title="0.9.3">
                0.9.3
              </span>
            </a>
            <a class="select-menu-item js-navigation-item js-navigation-open "
              href="/ValveSoftware/openvr/tree/0.9.2/headers/openvr_capi.h"
              data-name="0.9.2"
              data-skip-pjax="true"
              rel="nofollow">
              <svg aria-hidden="true" class="octicon octicon-check select-menu-item-icon" height="16" version="1.1" viewBox="0 0 12 16" width="12"><path fill-rule="evenodd" d="M12 5l-8 8-4-4 1.5-1.5L4 10l6.5-6.5z"/></svg>
              <span class="select-menu-item-text css-truncate-target" title="0.9.2">
                0.9.2
              </span>
            </a>
            <a class="select-menu-item js-navigation-item js-navigation-open "
              href="/ValveSoftware/openvr/tree/0.9.1/headers/openvr_capi.h"
              data-name="0.9.1"
              data-skip-pjax="true"
              rel="nofollow">
              <svg aria-hidden="true" class="octicon octicon-check select-menu-item-icon" height="16" version="1.1" viewBox="0 0 12 16" width="12"><path fill-rule="evenodd" d="M12 5l-8 8-4-4 1.5-1.5L4 10l6.5-6.5z"/></svg>
              <span class="select-menu-item-text css-truncate-target" title="0.9.1">
                0.9.1
              </span>
            </a>
            <a class="select-menu-item js-navigation-item js-navigation-open "
              href="/ValveSoftware/openvr/tree/0.9.0/headers/openvr_capi.h"
              data-name="0.9.0"
              data-skip-pjax="true"
              rel="nofollow">
              <svg aria-hidden="true" class="octicon octicon-check select-menu-item-icon" height="16" version="1.1" viewBox="0 0 12 16" width="12"><path fill-rule="evenodd" d="M12 5l-8 8-4-4 1.5-1.5L4 10l6.5-6.5z"/></svg>
              <span class="select-menu-item-text css-truncate-target" title="0.9.0">
                0.9.0
              </span>
            </a>
        </div>

        <div class="select-menu-no-results">Nothing to show</div>
      </div>

    </div>
  </div>
</div>

  <div class="BtnGroup float-right">
    <a href="/ValveSoftware/openvr/find/master"
          class="js-pjax-capture-input btn btn-sm BtnGroup-item"
          data-pjax
          data-hotkey="t">
      Find file
    </a>
    <button aria-label="Copy file path to clipboard" class="js-zeroclipboard btn btn-sm BtnGroup-item tooltipped tooltipped-s" data-copied-hint="Copied!" type="button">Copy path</button>
  </div>
  <div class="breadcrumb js-zeroclipboard-target">
    <span class="repo-root js-repo-root"><span class="js-path-segment"><a href="/ValveSoftware/openvr"><span>openvr</span></a></span></span><span class="separator">/</span><span class="js-path-segment"><a href="/ValveSoftware/openvr/tree/master/headers"><span>headers</span></a></span><span class="separator">/</span><strong class="final-path">openvr_capi.h</strong>
  </div>
</div>


  <div class="commit-tease">
      <span class="float-right">
        <a class="commit-tease-sha" href="/ValveSoftware/openvr/commit/9d48ccf90d709382a137de9bc429aab5f7cea5de" data-pjax>
          9d48ccf
        </a>
        <relative-time datetime="2016-12-12T20:01:31Z">Dec 12, 2016</relative-time>
      </span>
      <div>
        <img alt="@jeremyselan" class="avatar" height="20" src="https://avatars2.githubusercontent.com/u/240544?v=3&amp;s=40" width="20" />
        <a href="/jeremyselan" class="user-mention" rel="contributor">jeremyselan</a>
          <a href="/ValveSoftware/openvr/commit/9d48ccf90d709382a137de9bc429aab5f7cea5de" class="message" data-pjax="true" title="OpenVR SDK 1.0.5

General: 
* Added final support for submitting Vulkan overlays and eye images. See this document for more information: https://github.com/ValveSoftware/openvr/wiki/Vulkan
* Added TextureType_Vulkan, VRVulkanTextureData_t
* Rename EGraphicsAPIConvention -&gt; ETextureType.
* New synchronous MessageOverlay API. Use IVROverlay::ShowMessageOverlay to display a message with up to four buttons.
* Added ETrackedDeviceClass, GenericTracker
* Added ETrackedPropertyError, PermissionDenied

IVRSystem (v15):
* GetProjectionMatrix signature change, removed EGraphicsAPIConvention eProjType

IVRCompositor (v19):
* Added GetVulkanInstanceExtensionsRequired, GetVulkanDeviceExtensionsRequired

IVROverlay (v14):
* Added VROverlayFlags_VisibleInDashboard
* GetOverlayTexture signature change (includes overlay texture bounds)
* Added GetOverlayFlags
* Added ShowMessageOverlay

IVRTrackedCamera (v13):
* Renamed GetCameraIntrinsics

[git-p4: depot-paths = &quot;//vr/steamvr/sdk_release/&quot;: change = 3739504]">OpenVR SDK 1.0.5</a>
      </div>

    <div class="commit-tease-contributors">
      <button type="button" class="btn-link muted-link contributors-toggle" data-facebox="#blob_contributors_box">
        <strong>2</strong>
         contributors
      </button>
          <a class="avatar-link tooltipped tooltipped-s" aria-label="JoeLudwig" href="/ValveSoftware/openvr/commits/9d48ccf90d709382a137de9bc429aab5f7cea5de/headers/openvr_capi.h?author=JoeLudwig"><img alt="@JoeLudwig" class="avatar" height="20" src="https://avatars2.githubusercontent.com/u/303656?v=3&amp;s=40" width="20" /> </a>
    <a class="avatar-link tooltipped tooltipped-s" aria-label="jeremyselan" href="/ValveSoftware/openvr/commits/9d48ccf90d709382a137de9bc429aab5f7cea5de/headers/openvr_capi.h?author=jeremyselan"><img alt="@jeremyselan" class="avatar" height="20" src="https://avatars2.githubusercontent.com/u/240544?v=3&amp;s=40" width="20" /> </a>


    </div>

    <div id="blob_contributors_box" style="display:none">
      <h2 class="facebox-header" data-facebox-id="facebox-header">Users who have contributed to this file</h2>
      <ul class="facebox-user-list" data-facebox-id="facebox-description">
          <li class="facebox-user-list-item">
            <img alt="@JoeLudwig" height="24" src="https://avatars0.githubusercontent.com/u/303656?v=3&amp;s=48" width="24" />
            <a href="/JoeLudwig">JoeLudwig</a>
          </li>
          <li class="facebox-user-list-item">
            <img alt="@jeremyselan" height="24" src="https://avatars0.githubusercontent.com/u/240544?v=3&amp;s=48" width="24" />
            <a href="/jeremyselan">jeremyselan</a>
          </li>
      </ul>
    </div>
  </div>


<div class="file">
  <div class="file-header">
  <div class="file-actions">

    <div class="BtnGroup">
      <a href="/ValveSoftware/openvr/raw/master/headers/openvr_capi.h" class="btn btn-sm BtnGroup-item" id="raw-url">Raw</a>
        <a href="/ValveSoftware/openvr/blame/master/headers/openvr_capi.h" class="btn btn-sm js-update-url-with-hash BtnGroup-item" data-hotkey="b">Blame</a>
      <a href="/ValveSoftware/openvr/commits/master/headers/openvr_capi.h" class="btn btn-sm BtnGroup-item" rel="nofollow">History</a>
    </div>

        <a class="btn-octicon tooltipped tooltipped-nw"
           href="github-windows://openRepo/https://github.com/ValveSoftware/openvr?branch=master&amp;filepath=headers%2Fopenvr_capi.h"
           aria-label="Open this file in GitHub Desktop"
           data-ga-click="Repository, open with desktop, type:windows">
            <svg aria-hidden="true" class="octicon octicon-device-desktop" height="16" version="1.1" viewBox="0 0 16 16" width="16"><path fill-rule="evenodd" d="M15 2H1c-.55 0-1 .45-1 1v9c0 .55.45 1 1 1h5.34c-.25.61-.86 1.39-2.34 2h8c-1.48-.61-2.09-1.39-2.34-2H15c.55 0 1-.45 1-1V3c0-.55-.45-1-1-1zm0 9H1V3h14v8z"/></svg>
        </a>

        <!-- '"` --><!-- </textarea></xmp> --></option></form><form accept-charset="UTF-8" action="/ValveSoftware/openvr/edit/master/headers/openvr_capi.h" class="inline-form js-update-url-with-hash" method="post"><div style="margin:0;padding:0;display:inline"><input name="utf8" type="hidden" value="&#x2713;" /><input name="authenticity_token" type="hidden" value="Bxt4XUsCMhtC2iKNed09kU/P9fphUKZDiLBg2Xdg14/iBTHIdLan5WP08k0yRtEujJ8K1eOVeb7Vf250bt2MoA==" /></div>
          <button class="btn-octicon tooltipped tooltipped-nw" type="submit"
            aria-label="Fork this project and edit the file" data-hotkey="e" data-disable-with>
            <svg aria-hidden="true" class="octicon octicon-pencil" height="16" version="1.1" viewBox="0 0 14 16" width="14"><path fill-rule="evenodd" d="M0 12v3h3l8-8-3-3-8 8zm3 2H1v-2h1v1h1v1zm10.3-9.3L12 6 9 3l1.3-1.3a.996.996 0 0 1 1.41 0l1.59 1.59c.39.39.39 1.02 0 1.41z"/></svg>
          </button>
</form>        <!-- '"` --><!-- </textarea></xmp> --></option></form><form accept-charset="UTF-8" action="/ValveSoftware/openvr/delete/master/headers/openvr_capi.h" class="inline-form" method="post"><div style="margin:0;padding:0;display:inline"><input name="utf8" type="hidden" value="&#x2713;" /><input name="authenticity_token" type="hidden" value="MH/pnUE69oeJhicm39UsVJC8eQmcBTK6Cvzb1fGI0qIA0SoG45jnXCz0DuS7mmxCMVrwS781QAfKVAPECd2lHQ==" /></div>
          <button class="btn-octicon btn-octicon-danger tooltipped tooltipped-nw" type="submit"
            aria-label="Fork this project and delete the file" data-disable-with>
            <svg aria-hidden="true" class="octicon octicon-trashcan" height="16" version="1.1" viewBox="0 0 12 16" width="12"><path fill-rule="evenodd" d="M11 2H9c0-.55-.45-1-1-1H5c-.55 0-1 .45-1 1H2c-.55 0-1 .45-1 1v1c0 .55.45 1 1 1v9c0 .55.45 1 1 1h7c.55 0 1-.45 1-1V5c.55 0 1-.45 1-1V3c0-.55-.45-1-1-1zm-1 12H3V5h1v8h1V5h1v8h1V5h1v8h1V5h1v9zm1-10H2V3h9v1z"/></svg>
          </button>
</form>  </div>

  <div class="file-info">
      1817 lines (1674 sloc)
      <span class="file-info-divider"></span>
    92.8 KB
  </div>
</div>

  

  <div itemprop="text" class="blob-wrapper data type-c">
      <table class="highlight tab-size js-file-line-container" data-tab-size="8">
      <tr>
        <td id="L1" class="blob-num js-line-number" data-line-number="1"></td>
        <td id="LC1" class="blob-code blob-code-inner js-file-line"><span class="pl-c"><span class="pl-c">//</span>======= Copyright (c) Valve Corporation, All rights reserved. ===============</span></td>
      </tr>
      <tr>
        <td id="L2" class="blob-num js-line-number" data-line-number="2"></td>
        <td id="LC2" class="blob-code blob-code-inner js-file-line"><span class="pl-c"><span class="pl-c">//</span></span></td>
      </tr>
      <tr>
        <td id="L3" class="blob-num js-line-number" data-line-number="3"></td>
        <td id="LC3" class="blob-code blob-code-inner js-file-line"><span class="pl-c"><span class="pl-c">//</span> Purpose: Header for flatted SteamAPI. Use this for binding to other languages.</span></td>
      </tr>
      <tr>
        <td id="L4" class="blob-num js-line-number" data-line-number="4"></td>
        <td id="LC4" class="blob-code blob-code-inner js-file-line"><span class="pl-c"><span class="pl-c">//</span> This file is auto-generated, do not edit it.</span></td>
      </tr>
      <tr>
        <td id="L5" class="blob-num js-line-number" data-line-number="5"></td>
        <td id="LC5" class="blob-code blob-code-inner js-file-line"><span class="pl-c"><span class="pl-c">//</span></span></td>
      </tr>
      <tr>
        <td id="L6" class="blob-num js-line-number" data-line-number="6"></td>
        <td id="LC6" class="blob-code blob-code-inner js-file-line"><span class="pl-c"><span class="pl-c">//</span>=============================================================================</span></td>
      </tr>
      <tr>
        <td id="L7" class="blob-num js-line-number" data-line-number="7"></td>
        <td id="LC7" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L8" class="blob-num js-line-number" data-line-number="8"></td>
        <td id="LC8" class="blob-code blob-code-inner js-file-line">#<span class="pl-k">ifndef</span> __OPENVR_API_FLAT_H__</td>
      </tr>
      <tr>
        <td id="L9" class="blob-num js-line-number" data-line-number="9"></td>
        <td id="LC9" class="blob-code blob-code-inner js-file-line">#<span class="pl-k">define</span> <span class="pl-en">__OPENVR_API_FLAT_H__</span></td>
      </tr>
      <tr>
        <td id="L10" class="blob-num js-line-number" data-line-number="10"></td>
        <td id="LC10" class="blob-code blob-code-inner js-file-line">#<span class="pl-k">if</span> defined( _WIN32 ) || defined( __clang__ )</td>
      </tr>
      <tr>
        <td id="L11" class="blob-num js-line-number" data-line-number="11"></td>
        <td id="LC11" class="blob-code blob-code-inner js-file-line">#<span class="pl-k">pragma</span> once</td>
      </tr>
      <tr>
        <td id="L12" class="blob-num js-line-number" data-line-number="12"></td>
        <td id="LC12" class="blob-code blob-code-inner js-file-line">#<span class="pl-k">endif</span></td>
      </tr>
      <tr>
        <td id="L13" class="blob-num js-line-number" data-line-number="13"></td>
        <td id="LC13" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L14" class="blob-num js-line-number" data-line-number="14"></td>
        <td id="LC14" class="blob-code blob-code-inner js-file-line">#<span class="pl-k">ifdef</span> __cplusplus</td>
      </tr>
      <tr>
        <td id="L15" class="blob-num js-line-number" data-line-number="15"></td>
        <td id="LC15" class="blob-code blob-code-inner js-file-line">#<span class="pl-k">define</span> <span class="pl-en">EXTERN_C</span> <span class="pl-k">extern</span> <span class="pl-s"><span class="pl-pds">&quot;</span>C<span class="pl-pds">&quot;</span></span></td>
      </tr>
      <tr>
        <td id="L16" class="blob-num js-line-number" data-line-number="16"></td>
        <td id="LC16" class="blob-code blob-code-inner js-file-line">#<span class="pl-k">else</span></td>
      </tr>
      <tr>
        <td id="L17" class="blob-num js-line-number" data-line-number="17"></td>
        <td id="LC17" class="blob-code blob-code-inner js-file-line">#<span class="pl-k">define</span> <span class="pl-en">EXTERN_C</span></td>
      </tr>
      <tr>
        <td id="L18" class="blob-num js-line-number" data-line-number="18"></td>
        <td id="LC18" class="blob-code blob-code-inner js-file-line">#<span class="pl-k">endif</span></td>
      </tr>
      <tr>
        <td id="L19" class="blob-num js-line-number" data-line-number="19"></td>
        <td id="LC19" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L20" class="blob-num js-line-number" data-line-number="20"></td>
        <td id="LC20" class="blob-code blob-code-inner js-file-line">#<span class="pl-k">define</span> <span class="pl-en">OPENVR_FNTABLE_CALLTYPE</span> __stdcall</td>
      </tr>
      <tr>
        <td id="L21" class="blob-num js-line-number" data-line-number="21"></td>
        <td id="LC21" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L22" class="blob-num js-line-number" data-line-number="22"></td>
        <td id="LC22" class="blob-code blob-code-inner js-file-line"><span class="pl-c"><span class="pl-c">//</span> OPENVR API export macro</span></td>
      </tr>
      <tr>
        <td id="L23" class="blob-num js-line-number" data-line-number="23"></td>
        <td id="LC23" class="blob-code blob-code-inner js-file-line">#<span class="pl-k">if</span> defined( _WIN32 ) &amp;&amp; !defined( _X360 )</td>
      </tr>
      <tr>
        <td id="L24" class="blob-num js-line-number" data-line-number="24"></td>
        <td id="LC24" class="blob-code blob-code-inner js-file-line">	#<span class="pl-k">if</span> defined( OPENVR_API_EXPORTS )</td>
      </tr>
      <tr>
        <td id="L25" class="blob-num js-line-number" data-line-number="25"></td>
        <td id="LC25" class="blob-code blob-code-inner js-file-line">	#<span class="pl-k">define</span> <span class="pl-en">S_API</span> EXTERN_C <span class="pl-en">__declspec</span>( dllexport )</td>
      </tr>
      <tr>
        <td id="L26" class="blob-num js-line-number" data-line-number="26"></td>
        <td id="LC26" class="blob-code blob-code-inner js-file-line">	#elif <span class="pl-en">defined</span>( OPENVR_API_NODLL )</td>
      </tr>
      <tr>
        <td id="L27" class="blob-num js-line-number" data-line-number="27"></td>
        <td id="LC27" class="blob-code blob-code-inner js-file-line">	#define S_API EXTERN_C</td>
      </tr>
      <tr>
        <td id="L28" class="blob-num js-line-number" data-line-number="28"></td>
        <td id="LC28" class="blob-code blob-code-inner js-file-line">	#<span class="pl-k">else</span></td>
      </tr>
      <tr>
        <td id="L29" class="blob-num js-line-number" data-line-number="29"></td>
        <td id="LC29" class="blob-code blob-code-inner js-file-line">	#<span class="pl-k">define</span> <span class="pl-en">S_API</span> <span class="pl-k">extern</span> <span class="pl-s"><span class="pl-pds">&quot;</span>C<span class="pl-pds">&quot;</span></span> __declspec( dllimport ) </td>
      </tr>
      <tr>
        <td id="L30" class="blob-num js-line-number" data-line-number="30"></td>
        <td id="LC30" class="blob-code blob-code-inner js-file-line">	#<span class="pl-k">endif</span> <span class="pl-c"><span class="pl-c">//</span> OPENVR_API_EXPORTS</span></td>
      </tr>
      <tr>
        <td id="L31" class="blob-num js-line-number" data-line-number="31"></td>
        <td id="LC31" class="blob-code blob-code-inner js-file-line">#<span class="pl-k">elif</span> defined( __GNUC__ )</td>
      </tr>
      <tr>
        <td id="L32" class="blob-num js-line-number" data-line-number="32"></td>
        <td id="LC32" class="blob-code blob-code-inner js-file-line">	#<span class="pl-k">if</span> defined( OPENVR_API_EXPORTS )</td>
      </tr>
      <tr>
        <td id="L33" class="blob-num js-line-number" data-line-number="33"></td>
        <td id="LC33" class="blob-code blob-code-inner js-file-line">	#<span class="pl-k">define</span> <span class="pl-en">S_API</span> EXTERN_C <span class="pl-en">__attribute__</span> ((visibility(<span class="pl-s"><span class="pl-pds">&quot;</span>default<span class="pl-pds">&quot;</span></span>)))</td>
      </tr>
      <tr>
        <td id="L34" class="blob-num js-line-number" data-line-number="34"></td>
        <td id="LC34" class="blob-code blob-code-inner js-file-line">	#<span class="pl-k">else</span></td>
      </tr>
      <tr>
        <td id="L35" class="blob-num js-line-number" data-line-number="35"></td>
        <td id="LC35" class="blob-code blob-code-inner js-file-line">	#<span class="pl-k">define</span> <span class="pl-en">S_API</span> EXTERN_C</td>
      </tr>
      <tr>
        <td id="L36" class="blob-num js-line-number" data-line-number="36"></td>
        <td id="LC36" class="blob-code blob-code-inner js-file-line">	#<span class="pl-k">endif</span> <span class="pl-c"><span class="pl-c">//</span> OPENVR_API_EXPORTS</span></td>
      </tr>
      <tr>
        <td id="L37" class="blob-num js-line-number" data-line-number="37"></td>
        <td id="LC37" class="blob-code blob-code-inner js-file-line">#<span class="pl-k">else</span> <span class="pl-c"><span class="pl-c">//</span> !WIN32</span></td>
      </tr>
      <tr>
        <td id="L38" class="blob-num js-line-number" data-line-number="38"></td>
        <td id="LC38" class="blob-code blob-code-inner js-file-line">	#<span class="pl-k">if</span> defined( OPENVR_API_EXPORTS )</td>
      </tr>
      <tr>
        <td id="L39" class="blob-num js-line-number" data-line-number="39"></td>
        <td id="LC39" class="blob-code blob-code-inner js-file-line">	#<span class="pl-k">define</span> <span class="pl-en">S_API</span> EXTERN_C</td>
      </tr>
      <tr>
        <td id="L40" class="blob-num js-line-number" data-line-number="40"></td>
        <td id="LC40" class="blob-code blob-code-inner js-file-line">	#<span class="pl-k">else</span></td>
      </tr>
      <tr>
        <td id="L41" class="blob-num js-line-number" data-line-number="41"></td>
        <td id="LC41" class="blob-code blob-code-inner js-file-line">	#<span class="pl-k">define</span> <span class="pl-en">S_API</span> EXTERN_C</td>
      </tr>
      <tr>
        <td id="L42" class="blob-num js-line-number" data-line-number="42"></td>
        <td id="LC42" class="blob-code blob-code-inner js-file-line">	#<span class="pl-k">endif</span> <span class="pl-c"><span class="pl-c">//</span> OPENVR_API_EXPORTS</span></td>
      </tr>
      <tr>
        <td id="L43" class="blob-num js-line-number" data-line-number="43"></td>
        <td id="LC43" class="blob-code blob-code-inner js-file-line">#<span class="pl-k">endif</span></td>
      </tr>
      <tr>
        <td id="L44" class="blob-num js-line-number" data-line-number="44"></td>
        <td id="LC44" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L45" class="blob-num js-line-number" data-line-number="45"></td>
        <td id="LC45" class="blob-code blob-code-inner js-file-line">#<span class="pl-k">include</span> <span class="pl-s"><span class="pl-pds">&lt;</span>stdint.h<span class="pl-pds">&gt;</span></span></td>
      </tr>
      <tr>
        <td id="L46" class="blob-num js-line-number" data-line-number="46"></td>
        <td id="LC46" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L47" class="blob-num js-line-number" data-line-number="47"></td>
        <td id="LC47" class="blob-code blob-code-inner js-file-line">#<span class="pl-k">if</span> defined( __WIN32 )</td>
      </tr>
      <tr>
        <td id="L48" class="blob-num js-line-number" data-line-number="48"></td>
        <td id="LC48" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">char</span> <span class="pl-k">bool</span>;</td>
      </tr>
      <tr>
        <td id="L49" class="blob-num js-line-number" data-line-number="49"></td>
        <td id="LC49" class="blob-code blob-code-inner js-file-line">#<span class="pl-k">else</span></td>
      </tr>
      <tr>
        <td id="L50" class="blob-num js-line-number" data-line-number="50"></td>
        <td id="LC50" class="blob-code blob-code-inner js-file-line">#<span class="pl-k">include</span> <span class="pl-s"><span class="pl-pds">&lt;</span>stdbool.h<span class="pl-pds">&gt;</span></span></td>
      </tr>
      <tr>
        <td id="L51" class="blob-num js-line-number" data-line-number="51"></td>
        <td id="LC51" class="blob-code blob-code-inner js-file-line">#<span class="pl-k">endif</span></td>
      </tr>
      <tr>
        <td id="L52" class="blob-num js-line-number" data-line-number="52"></td>
        <td id="LC52" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L53" class="blob-num js-line-number" data-line-number="53"></td>
        <td id="LC53" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L54" class="blob-num js-line-number" data-line-number="54"></td>
        <td id="LC54" class="blob-code blob-code-inner js-file-line"><span class="pl-c"><span class="pl-c">//</span> OpenVR Constants</span></td>
      </tr>
      <tr>
        <td id="L55" class="blob-num js-line-number" data-line-number="55"></td>
        <td id="LC55" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L56" class="blob-num js-line-number" data-line-number="56"></td>
        <td id="LC56" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">unsigned</span> <span class="pl-k">int</span> k_unMaxDriverDebugResponseSize = <span class="pl-c1">32768</span>;</td>
      </tr>
      <tr>
        <td id="L57" class="blob-num js-line-number" data-line-number="57"></td>
        <td id="LC57" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">unsigned</span> <span class="pl-k">int</span> k_unTrackedDeviceIndex_Hmd = <span class="pl-c1">0</span>;</td>
      </tr>
      <tr>
        <td id="L58" class="blob-num js-line-number" data-line-number="58"></td>
        <td id="LC58" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">unsigned</span> <span class="pl-k">int</span> k_unMaxTrackedDeviceCount = <span class="pl-c1">16</span>;</td>
      </tr>
      <tr>
        <td id="L59" class="blob-num js-line-number" data-line-number="59"></td>
        <td id="LC59" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">unsigned</span> <span class="pl-k">int</span> k_unTrackedDeviceIndexOther = <span class="pl-c1">4294967294</span>;</td>
      </tr>
      <tr>
        <td id="L60" class="blob-num js-line-number" data-line-number="60"></td>
        <td id="LC60" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">unsigned</span> <span class="pl-k">int</span> k_unTrackedDeviceIndexInvalid = <span class="pl-c1">4294967295</span>;</td>
      </tr>
      <tr>
        <td id="L61" class="blob-num js-line-number" data-line-number="61"></td>
        <td id="LC61" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">unsigned</span> <span class="pl-k">int</span> k_unMaxPropertyStringSize = <span class="pl-c1">32768</span>;</td>
      </tr>
      <tr>
        <td id="L62" class="blob-num js-line-number" data-line-number="62"></td>
        <td id="LC62" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">unsigned</span> <span class="pl-k">int</span> k_unControllerStateAxisCount = <span class="pl-c1">5</span>;</td>
      </tr>
      <tr>
        <td id="L63" class="blob-num js-line-number" data-line-number="63"></td>
        <td id="LC63" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">unsigned</span> <span class="pl-k">long</span> k_ulOverlayHandleInvalid = <span class="pl-c1">0</span>;</td>
      </tr>
      <tr>
        <td id="L64" class="blob-num js-line-number" data-line-number="64"></td>
        <td id="LC64" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">unsigned</span> <span class="pl-k">int</span> k_unScreenshotHandleInvalid = <span class="pl-c1">0</span>;</td>
      </tr>
      <tr>
        <td id="L65" class="blob-num js-line-number" data-line-number="65"></td>
        <td id="LC65" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * IVRSystem_Version = <span class="pl-s"><span class="pl-pds">&quot;</span>IVRSystem_015<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L66" class="blob-num js-line-number" data-line-number="66"></td>
        <td id="LC66" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * IVRExtendedDisplay_Version = <span class="pl-s"><span class="pl-pds">&quot;</span>IVRExtendedDisplay_001<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L67" class="blob-num js-line-number" data-line-number="67"></td>
        <td id="LC67" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * IVRTrackedCamera_Version = <span class="pl-s"><span class="pl-pds">&quot;</span>IVRTrackedCamera_003<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L68" class="blob-num js-line-number" data-line-number="68"></td>
        <td id="LC68" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">unsigned</span> <span class="pl-k">int</span> k_unMaxApplicationKeyLength = <span class="pl-c1">128</span>;</td>
      </tr>
      <tr>
        <td id="L69" class="blob-num js-line-number" data-line-number="69"></td>
        <td id="LC69" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_MimeType_HomeApp = <span class="pl-s"><span class="pl-pds">&quot;</span>vr/home<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L70" class="blob-num js-line-number" data-line-number="70"></td>
        <td id="LC70" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_MimeType_GameTheater = <span class="pl-s"><span class="pl-pds">&quot;</span>vr/game_theater<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L71" class="blob-num js-line-number" data-line-number="71"></td>
        <td id="LC71" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * IVRApplications_Version = <span class="pl-s"><span class="pl-pds">&quot;</span>IVRApplications_006<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L72" class="blob-num js-line-number" data-line-number="72"></td>
        <td id="LC72" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * IVRChaperone_Version = <span class="pl-s"><span class="pl-pds">&quot;</span>IVRChaperone_003<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L73" class="blob-num js-line-number" data-line-number="73"></td>
        <td id="LC73" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * IVRChaperoneSetup_Version = <span class="pl-s"><span class="pl-pds">&quot;</span>IVRChaperoneSetup_005<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L74" class="blob-num js-line-number" data-line-number="74"></td>
        <td id="LC74" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * IVRCompositor_Version = <span class="pl-s"><span class="pl-pds">&quot;</span>IVRCompositor_019<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L75" class="blob-num js-line-number" data-line-number="75"></td>
        <td id="LC75" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">unsigned</span> <span class="pl-k">int</span> k_unVROverlayMaxKeyLength = <span class="pl-c1">128</span>;</td>
      </tr>
      <tr>
        <td id="L76" class="blob-num js-line-number" data-line-number="76"></td>
        <td id="LC76" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">unsigned</span> <span class="pl-k">int</span> k_unVROverlayMaxNameLength = <span class="pl-c1">128</span>;</td>
      </tr>
      <tr>
        <td id="L77" class="blob-num js-line-number" data-line-number="77"></td>
        <td id="LC77" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">unsigned</span> <span class="pl-k">int</span> k_unMaxOverlayCount = <span class="pl-c1">64</span>;</td>
      </tr>
      <tr>
        <td id="L78" class="blob-num js-line-number" data-line-number="78"></td>
        <td id="LC78" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">unsigned</span> <span class="pl-k">int</span> k_unMaxOverlayIntersectionMaskPrimitivesCount = <span class="pl-c1">32</span>;</td>
      </tr>
      <tr>
        <td id="L79" class="blob-num js-line-number" data-line-number="79"></td>
        <td id="LC79" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * IVROverlay_Version = <span class="pl-s"><span class="pl-pds">&quot;</span>IVROverlay_014<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L80" class="blob-num js-line-number" data-line-number="80"></td>
        <td id="LC80" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_Controller_Component_GDC2015 = <span class="pl-s"><span class="pl-pds">&quot;</span>gdc2015<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L81" class="blob-num js-line-number" data-line-number="81"></td>
        <td id="LC81" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_Controller_Component_Base = <span class="pl-s"><span class="pl-pds">&quot;</span>base<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L82" class="blob-num js-line-number" data-line-number="82"></td>
        <td id="LC82" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_Controller_Component_Tip = <span class="pl-s"><span class="pl-pds">&quot;</span>tip<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L83" class="blob-num js-line-number" data-line-number="83"></td>
        <td id="LC83" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_Controller_Component_HandGrip = <span class="pl-s"><span class="pl-pds">&quot;</span>handgrip<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L84" class="blob-num js-line-number" data-line-number="84"></td>
        <td id="LC84" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_Controller_Component_Status = <span class="pl-s"><span class="pl-pds">&quot;</span>status<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L85" class="blob-num js-line-number" data-line-number="85"></td>
        <td id="LC85" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * IVRRenderModels_Version = <span class="pl-s"><span class="pl-pds">&quot;</span>IVRRenderModels_005<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L86" class="blob-num js-line-number" data-line-number="86"></td>
        <td id="LC86" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">unsigned</span> <span class="pl-k">int</span> k_unNotificationTextMaxSize = <span class="pl-c1">256</span>;</td>
      </tr>
      <tr>
        <td id="L87" class="blob-num js-line-number" data-line-number="87"></td>
        <td id="LC87" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * IVRNotifications_Version = <span class="pl-s"><span class="pl-pds">&quot;</span>IVRNotifications_002<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L88" class="blob-num js-line-number" data-line-number="88"></td>
        <td id="LC88" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">unsigned</span> <span class="pl-k">int</span> k_unMaxSettingsKeyLength = <span class="pl-c1">128</span>;</td>
      </tr>
      <tr>
        <td id="L89" class="blob-num js-line-number" data-line-number="89"></td>
        <td id="LC89" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * IVRSettings_Version = <span class="pl-s"><span class="pl-pds">&quot;</span>IVRSettings_002<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L90" class="blob-num js-line-number" data-line-number="90"></td>
        <td id="LC90" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_SteamVR_Section = <span class="pl-s"><span class="pl-pds">&quot;</span>steamvr<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L91" class="blob-num js-line-number" data-line-number="91"></td>
        <td id="LC91" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_SteamVR_RequireHmd_String = <span class="pl-s"><span class="pl-pds">&quot;</span>requireHmd<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L92" class="blob-num js-line-number" data-line-number="92"></td>
        <td id="LC92" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_SteamVR_ForcedDriverKey_String = <span class="pl-s"><span class="pl-pds">&quot;</span>forcedDriver<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L93" class="blob-num js-line-number" data-line-number="93"></td>
        <td id="LC93" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_SteamVR_ForcedHmdKey_String = <span class="pl-s"><span class="pl-pds">&quot;</span>forcedHmd<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L94" class="blob-num js-line-number" data-line-number="94"></td>
        <td id="LC94" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_SteamVR_DisplayDebug_Bool = <span class="pl-s"><span class="pl-pds">&quot;</span>displayDebug<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L95" class="blob-num js-line-number" data-line-number="95"></td>
        <td id="LC95" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_SteamVR_DebugProcessPipe_String = <span class="pl-s"><span class="pl-pds">&quot;</span>debugProcessPipe<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L96" class="blob-num js-line-number" data-line-number="96"></td>
        <td id="LC96" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_SteamVR_EnableDistortion_Bool = <span class="pl-s"><span class="pl-pds">&quot;</span>enableDistortion<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L97" class="blob-num js-line-number" data-line-number="97"></td>
        <td id="LC97" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_SteamVR_DisplayDebugX_Int32 = <span class="pl-s"><span class="pl-pds">&quot;</span>displayDebugX<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L98" class="blob-num js-line-number" data-line-number="98"></td>
        <td id="LC98" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_SteamVR_DisplayDebugY_Int32 = <span class="pl-s"><span class="pl-pds">&quot;</span>displayDebugY<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L99" class="blob-num js-line-number" data-line-number="99"></td>
        <td id="LC99" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_SteamVR_SendSystemButtonToAllApps_Bool = <span class="pl-s"><span class="pl-pds">&quot;</span>sendSystemButtonToAllApps<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L100" class="blob-num js-line-number" data-line-number="100"></td>
        <td id="LC100" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_SteamVR_LogLevel_Int32 = <span class="pl-s"><span class="pl-pds">&quot;</span>loglevel<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L101" class="blob-num js-line-number" data-line-number="101"></td>
        <td id="LC101" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_SteamVR_IPD_Float = <span class="pl-s"><span class="pl-pds">&quot;</span>ipd<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L102" class="blob-num js-line-number" data-line-number="102"></td>
        <td id="LC102" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_SteamVR_Background_String = <span class="pl-s"><span class="pl-pds">&quot;</span>background<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L103" class="blob-num js-line-number" data-line-number="103"></td>
        <td id="LC103" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_SteamVR_BackgroundUseDomeProjection_Bool = <span class="pl-s"><span class="pl-pds">&quot;</span>backgroundUseDomeProjection<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L104" class="blob-num js-line-number" data-line-number="104"></td>
        <td id="LC104" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_SteamVR_BackgroundCameraHeight_Float = <span class="pl-s"><span class="pl-pds">&quot;</span>backgroundCameraHeight<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L105" class="blob-num js-line-number" data-line-number="105"></td>
        <td id="LC105" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_SteamVR_BackgroundDomeRadius_Float = <span class="pl-s"><span class="pl-pds">&quot;</span>backgroundDomeRadius<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L106" class="blob-num js-line-number" data-line-number="106"></td>
        <td id="LC106" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_SteamVR_GridColor_String = <span class="pl-s"><span class="pl-pds">&quot;</span>gridColor<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L107" class="blob-num js-line-number" data-line-number="107"></td>
        <td id="LC107" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_SteamVR_PlayAreaColor_String = <span class="pl-s"><span class="pl-pds">&quot;</span>playAreaColor<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L108" class="blob-num js-line-number" data-line-number="108"></td>
        <td id="LC108" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_SteamVR_ShowStage_Bool = <span class="pl-s"><span class="pl-pds">&quot;</span>showStage<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L109" class="blob-num js-line-number" data-line-number="109"></td>
        <td id="LC109" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_SteamVR_ActivateMultipleDrivers_Bool = <span class="pl-s"><span class="pl-pds">&quot;</span>activateMultipleDrivers<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L110" class="blob-num js-line-number" data-line-number="110"></td>
        <td id="LC110" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_SteamVR_DirectMode_Bool = <span class="pl-s"><span class="pl-pds">&quot;</span>directMode<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L111" class="blob-num js-line-number" data-line-number="111"></td>
        <td id="LC111" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_SteamVR_DirectModeEdidVid_Int32 = <span class="pl-s"><span class="pl-pds">&quot;</span>directModeEdidVid<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L112" class="blob-num js-line-number" data-line-number="112"></td>
        <td id="LC112" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_SteamVR_DirectModeEdidPid_Int32 = <span class="pl-s"><span class="pl-pds">&quot;</span>directModeEdidPid<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L113" class="blob-num js-line-number" data-line-number="113"></td>
        <td id="LC113" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_SteamVR_UsingSpeakers_Bool = <span class="pl-s"><span class="pl-pds">&quot;</span>usingSpeakers<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L114" class="blob-num js-line-number" data-line-number="114"></td>
        <td id="LC114" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_SteamVR_SpeakersForwardYawOffsetDegrees_Float = <span class="pl-s"><span class="pl-pds">&quot;</span>speakersForwardYawOffsetDegrees<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L115" class="blob-num js-line-number" data-line-number="115"></td>
        <td id="LC115" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_SteamVR_BaseStationPowerManagement_Bool = <span class="pl-s"><span class="pl-pds">&quot;</span>basestationPowerManagement<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L116" class="blob-num js-line-number" data-line-number="116"></td>
        <td id="LC116" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_SteamVR_NeverKillProcesses_Bool = <span class="pl-s"><span class="pl-pds">&quot;</span>neverKillProcesses<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L117" class="blob-num js-line-number" data-line-number="117"></td>
        <td id="LC117" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_SteamVR_RenderTargetMultiplier_Float = <span class="pl-s"><span class="pl-pds">&quot;</span>renderTargetMultiplier<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L118" class="blob-num js-line-number" data-line-number="118"></td>
        <td id="LC118" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_SteamVR_AllowAsyncReprojection_Bool = <span class="pl-s"><span class="pl-pds">&quot;</span>allowAsyncReprojection<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L119" class="blob-num js-line-number" data-line-number="119"></td>
        <td id="LC119" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_SteamVR_AllowReprojection_Bool = <span class="pl-s"><span class="pl-pds">&quot;</span>allowInterleavedReprojection<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L120" class="blob-num js-line-number" data-line-number="120"></td>
        <td id="LC120" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_SteamVR_ForceReprojection_Bool = <span class="pl-s"><span class="pl-pds">&quot;</span>forceReprojection<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L121" class="blob-num js-line-number" data-line-number="121"></td>
        <td id="LC121" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_SteamVR_ForceFadeOnBadTracking_Bool = <span class="pl-s"><span class="pl-pds">&quot;</span>forceFadeOnBadTracking<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L122" class="blob-num js-line-number" data-line-number="122"></td>
        <td id="LC122" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_SteamVR_DefaultMirrorView_Int32 = <span class="pl-s"><span class="pl-pds">&quot;</span>defaultMirrorView<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L123" class="blob-num js-line-number" data-line-number="123"></td>
        <td id="LC123" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_SteamVR_ShowMirrorView_Bool = <span class="pl-s"><span class="pl-pds">&quot;</span>showMirrorView<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L124" class="blob-num js-line-number" data-line-number="124"></td>
        <td id="LC124" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_SteamVR_MirrorViewGeometry_String = <span class="pl-s"><span class="pl-pds">&quot;</span>mirrorViewGeometry<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L125" class="blob-num js-line-number" data-line-number="125"></td>
        <td id="LC125" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_SteamVR_StartMonitorFromAppLaunch = <span class="pl-s"><span class="pl-pds">&quot;</span>startMonitorFromAppLaunch<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L126" class="blob-num js-line-number" data-line-number="126"></td>
        <td id="LC126" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_SteamVR_StartCompositorFromAppLaunch_Bool = <span class="pl-s"><span class="pl-pds">&quot;</span>startCompositorFromAppLaunch<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L127" class="blob-num js-line-number" data-line-number="127"></td>
        <td id="LC127" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_SteamVR_StartDashboardFromAppLaunch_Bool = <span class="pl-s"><span class="pl-pds">&quot;</span>startDashboardFromAppLaunch<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L128" class="blob-num js-line-number" data-line-number="128"></td>
        <td id="LC128" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_SteamVR_StartOverlayAppsFromDashboard_Bool = <span class="pl-s"><span class="pl-pds">&quot;</span>startOverlayAppsFromDashboard<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L129" class="blob-num js-line-number" data-line-number="129"></td>
        <td id="LC129" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_SteamVR_EnableHomeApp = <span class="pl-s"><span class="pl-pds">&quot;</span>enableHomeApp<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L130" class="blob-num js-line-number" data-line-number="130"></td>
        <td id="LC130" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_SteamVR_SetInitialDefaultHomeApp = <span class="pl-s"><span class="pl-pds">&quot;</span>setInitialDefaultHomeApp<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L131" class="blob-num js-line-number" data-line-number="131"></td>
        <td id="LC131" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_SteamVR_CycleBackgroundImageTimeSec_Int32 = <span class="pl-s"><span class="pl-pds">&quot;</span>CycleBackgroundImageTimeSec<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L132" class="blob-num js-line-number" data-line-number="132"></td>
        <td id="LC132" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_SteamVR_RetailDemo_Bool = <span class="pl-s"><span class="pl-pds">&quot;</span>retailDemo<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L133" class="blob-num js-line-number" data-line-number="133"></td>
        <td id="LC133" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_SteamVR_IpdOffset_Float = <span class="pl-s"><span class="pl-pds">&quot;</span>ipdOffset<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L134" class="blob-num js-line-number" data-line-number="134"></td>
        <td id="LC134" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_Lighthouse_Section = <span class="pl-s"><span class="pl-pds">&quot;</span>driver_lighthouse<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L135" class="blob-num js-line-number" data-line-number="135"></td>
        <td id="LC135" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_Lighthouse_DisableIMU_Bool = <span class="pl-s"><span class="pl-pds">&quot;</span>disableimu<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L136" class="blob-num js-line-number" data-line-number="136"></td>
        <td id="LC136" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_Lighthouse_UseDisambiguation_String = <span class="pl-s"><span class="pl-pds">&quot;</span>usedisambiguation<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L137" class="blob-num js-line-number" data-line-number="137"></td>
        <td id="LC137" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_Lighthouse_DisambiguationDebug_Int32 = <span class="pl-s"><span class="pl-pds">&quot;</span>disambiguationdebug<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L138" class="blob-num js-line-number" data-line-number="138"></td>
        <td id="LC138" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_Lighthouse_PrimaryBasestation_Int32 = <span class="pl-s"><span class="pl-pds">&quot;</span>primarybasestation<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L139" class="blob-num js-line-number" data-line-number="139"></td>
        <td id="LC139" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_Lighthouse_DBHistory_Bool = <span class="pl-s"><span class="pl-pds">&quot;</span>dbhistory<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L140" class="blob-num js-line-number" data-line-number="140"></td>
        <td id="LC140" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_Null_Section = <span class="pl-s"><span class="pl-pds">&quot;</span>driver_null<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L141" class="blob-num js-line-number" data-line-number="141"></td>
        <td id="LC141" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_Null_EnableNullDriver_Bool = <span class="pl-s"><span class="pl-pds">&quot;</span>enable<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L142" class="blob-num js-line-number" data-line-number="142"></td>
        <td id="LC142" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_Null_SerialNumber_String = <span class="pl-s"><span class="pl-pds">&quot;</span>serialNumber<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L143" class="blob-num js-line-number" data-line-number="143"></td>
        <td id="LC143" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_Null_ModelNumber_String = <span class="pl-s"><span class="pl-pds">&quot;</span>modelNumber<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L144" class="blob-num js-line-number" data-line-number="144"></td>
        <td id="LC144" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_Null_WindowX_Int32 = <span class="pl-s"><span class="pl-pds">&quot;</span>windowX<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L145" class="blob-num js-line-number" data-line-number="145"></td>
        <td id="LC145" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_Null_WindowY_Int32 = <span class="pl-s"><span class="pl-pds">&quot;</span>windowY<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L146" class="blob-num js-line-number" data-line-number="146"></td>
        <td id="LC146" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_Null_WindowWidth_Int32 = <span class="pl-s"><span class="pl-pds">&quot;</span>windowWidth<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L147" class="blob-num js-line-number" data-line-number="147"></td>
        <td id="LC147" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_Null_WindowHeight_Int32 = <span class="pl-s"><span class="pl-pds">&quot;</span>windowHeight<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L148" class="blob-num js-line-number" data-line-number="148"></td>
        <td id="LC148" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_Null_RenderWidth_Int32 = <span class="pl-s"><span class="pl-pds">&quot;</span>renderWidth<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L149" class="blob-num js-line-number" data-line-number="149"></td>
        <td id="LC149" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_Null_RenderHeight_Int32 = <span class="pl-s"><span class="pl-pds">&quot;</span>renderHeight<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L150" class="blob-num js-line-number" data-line-number="150"></td>
        <td id="LC150" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_Null_SecondsFromVsyncToPhotons_Float = <span class="pl-s"><span class="pl-pds">&quot;</span>secondsFromVsyncToPhotons<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L151" class="blob-num js-line-number" data-line-number="151"></td>
        <td id="LC151" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_Null_DisplayFrequency_Float = <span class="pl-s"><span class="pl-pds">&quot;</span>displayFrequency<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L152" class="blob-num js-line-number" data-line-number="152"></td>
        <td id="LC152" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_UserInterface_Section = <span class="pl-s"><span class="pl-pds">&quot;</span>userinterface<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L153" class="blob-num js-line-number" data-line-number="153"></td>
        <td id="LC153" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_UserInterface_StatusAlwaysOnTop_Bool = <span class="pl-s"><span class="pl-pds">&quot;</span>StatusAlwaysOnTop<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L154" class="blob-num js-line-number" data-line-number="154"></td>
        <td id="LC154" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_UserInterface_MinimizeToTray_Bool = <span class="pl-s"><span class="pl-pds">&quot;</span>MinimizeToTray<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L155" class="blob-num js-line-number" data-line-number="155"></td>
        <td id="LC155" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_UserInterface_Screenshots_Bool = <span class="pl-s"><span class="pl-pds">&quot;</span>screenshots<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L156" class="blob-num js-line-number" data-line-number="156"></td>
        <td id="LC156" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_UserInterface_ScreenshotType_Int = <span class="pl-s"><span class="pl-pds">&quot;</span>screenshotType<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L157" class="blob-num js-line-number" data-line-number="157"></td>
        <td id="LC157" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_Notifications_Section = <span class="pl-s"><span class="pl-pds">&quot;</span>notifications<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L158" class="blob-num js-line-number" data-line-number="158"></td>
        <td id="LC158" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_Notifications_DoNotDisturb_Bool = <span class="pl-s"><span class="pl-pds">&quot;</span>DoNotDisturb<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L159" class="blob-num js-line-number" data-line-number="159"></td>
        <td id="LC159" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_Keyboard_Section = <span class="pl-s"><span class="pl-pds">&quot;</span>keyboard<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L160" class="blob-num js-line-number" data-line-number="160"></td>
        <td id="LC160" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_Keyboard_TutorialCompletions = <span class="pl-s"><span class="pl-pds">&quot;</span>TutorialCompletions<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L161" class="blob-num js-line-number" data-line-number="161"></td>
        <td id="LC161" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_Keyboard_ScaleX = <span class="pl-s"><span class="pl-pds">&quot;</span>ScaleX<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L162" class="blob-num js-line-number" data-line-number="162"></td>
        <td id="LC162" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_Keyboard_ScaleY = <span class="pl-s"><span class="pl-pds">&quot;</span>ScaleY<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L163" class="blob-num js-line-number" data-line-number="163"></td>
        <td id="LC163" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_Keyboard_OffsetLeftX = <span class="pl-s"><span class="pl-pds">&quot;</span>OffsetLeftX<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L164" class="blob-num js-line-number" data-line-number="164"></td>
        <td id="LC164" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_Keyboard_OffsetRightX = <span class="pl-s"><span class="pl-pds">&quot;</span>OffsetRightX<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L165" class="blob-num js-line-number" data-line-number="165"></td>
        <td id="LC165" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_Keyboard_OffsetY = <span class="pl-s"><span class="pl-pds">&quot;</span>OffsetY<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L166" class="blob-num js-line-number" data-line-number="166"></td>
        <td id="LC166" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_Keyboard_Smoothing = <span class="pl-s"><span class="pl-pds">&quot;</span>Smoothing<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L167" class="blob-num js-line-number" data-line-number="167"></td>
        <td id="LC167" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_Perf_Section = <span class="pl-s"><span class="pl-pds">&quot;</span>perfcheck<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L168" class="blob-num js-line-number" data-line-number="168"></td>
        <td id="LC168" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_Perf_HeuristicActive_Bool = <span class="pl-s"><span class="pl-pds">&quot;</span>heuristicActive<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L169" class="blob-num js-line-number" data-line-number="169"></td>
        <td id="LC169" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_Perf_NotifyInHMD_Bool = <span class="pl-s"><span class="pl-pds">&quot;</span>warnInHMD<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L170" class="blob-num js-line-number" data-line-number="170"></td>
        <td id="LC170" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_Perf_NotifyOnlyOnce_Bool = <span class="pl-s"><span class="pl-pds">&quot;</span>warnOnlyOnce<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L171" class="blob-num js-line-number" data-line-number="171"></td>
        <td id="LC171" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_Perf_AllowTimingStore_Bool = <span class="pl-s"><span class="pl-pds">&quot;</span>allowTimingStore<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L172" class="blob-num js-line-number" data-line-number="172"></td>
        <td id="LC172" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_Perf_SaveTimingsOnExit_Bool = <span class="pl-s"><span class="pl-pds">&quot;</span>saveTimingsOnExit<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L173" class="blob-num js-line-number" data-line-number="173"></td>
        <td id="LC173" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_Perf_TestData_Float = <span class="pl-s"><span class="pl-pds">&quot;</span>perfTestData<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L174" class="blob-num js-line-number" data-line-number="174"></td>
        <td id="LC174" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_CollisionBounds_Section = <span class="pl-s"><span class="pl-pds">&quot;</span>collisionBounds<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L175" class="blob-num js-line-number" data-line-number="175"></td>
        <td id="LC175" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_CollisionBounds_Style_Int32 = <span class="pl-s"><span class="pl-pds">&quot;</span>CollisionBoundsStyle<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L176" class="blob-num js-line-number" data-line-number="176"></td>
        <td id="LC176" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_CollisionBounds_GroundPerimeterOn_Bool = <span class="pl-s"><span class="pl-pds">&quot;</span>CollisionBoundsGroundPerimeterOn<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L177" class="blob-num js-line-number" data-line-number="177"></td>
        <td id="LC177" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_CollisionBounds_CenterMarkerOn_Bool = <span class="pl-s"><span class="pl-pds">&quot;</span>CollisionBoundsCenterMarkerOn<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L178" class="blob-num js-line-number" data-line-number="178"></td>
        <td id="LC178" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_CollisionBounds_PlaySpaceOn_Bool = <span class="pl-s"><span class="pl-pds">&quot;</span>CollisionBoundsPlaySpaceOn<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L179" class="blob-num js-line-number" data-line-number="179"></td>
        <td id="LC179" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_CollisionBounds_FadeDistance_Float = <span class="pl-s"><span class="pl-pds">&quot;</span>CollisionBoundsFadeDistance<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L180" class="blob-num js-line-number" data-line-number="180"></td>
        <td id="LC180" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_CollisionBounds_ColorGammaR_Int32 = <span class="pl-s"><span class="pl-pds">&quot;</span>CollisionBoundsColorGammaR<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L181" class="blob-num js-line-number" data-line-number="181"></td>
        <td id="LC181" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_CollisionBounds_ColorGammaG_Int32 = <span class="pl-s"><span class="pl-pds">&quot;</span>CollisionBoundsColorGammaG<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L182" class="blob-num js-line-number" data-line-number="182"></td>
        <td id="LC182" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_CollisionBounds_ColorGammaB_Int32 = <span class="pl-s"><span class="pl-pds">&quot;</span>CollisionBoundsColorGammaB<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L183" class="blob-num js-line-number" data-line-number="183"></td>
        <td id="LC183" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_CollisionBounds_ColorGammaA_Int32 = <span class="pl-s"><span class="pl-pds">&quot;</span>CollisionBoundsColorGammaA<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L184" class="blob-num js-line-number" data-line-number="184"></td>
        <td id="LC184" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_Camera_Section = <span class="pl-s"><span class="pl-pds">&quot;</span>camera<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L185" class="blob-num js-line-number" data-line-number="185"></td>
        <td id="LC185" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_Camera_EnableCamera_Bool = <span class="pl-s"><span class="pl-pds">&quot;</span>enableCamera<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L186" class="blob-num js-line-number" data-line-number="186"></td>
        <td id="LC186" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_Camera_EnableCameraInDashboard_Bool = <span class="pl-s"><span class="pl-pds">&quot;</span>enableCameraInDashboard<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L187" class="blob-num js-line-number" data-line-number="187"></td>
        <td id="LC187" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_Camera_EnableCameraForCollisionBounds_Bool = <span class="pl-s"><span class="pl-pds">&quot;</span>enableCameraForCollisionBounds<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L188" class="blob-num js-line-number" data-line-number="188"></td>
        <td id="LC188" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_Camera_EnableCameraForRoomView_Bool = <span class="pl-s"><span class="pl-pds">&quot;</span>enableCameraForRoomView<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L189" class="blob-num js-line-number" data-line-number="189"></td>
        <td id="LC189" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_Camera_BoundsColorGammaR_Int32 = <span class="pl-s"><span class="pl-pds">&quot;</span>cameraBoundsColorGammaR<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L190" class="blob-num js-line-number" data-line-number="190"></td>
        <td id="LC190" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_Camera_BoundsColorGammaG_Int32 = <span class="pl-s"><span class="pl-pds">&quot;</span>cameraBoundsColorGammaG<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L191" class="blob-num js-line-number" data-line-number="191"></td>
        <td id="LC191" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_Camera_BoundsColorGammaB_Int32 = <span class="pl-s"><span class="pl-pds">&quot;</span>cameraBoundsColorGammaB<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L192" class="blob-num js-line-number" data-line-number="192"></td>
        <td id="LC192" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_Camera_BoundsColorGammaA_Int32 = <span class="pl-s"><span class="pl-pds">&quot;</span>cameraBoundsColorGammaA<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L193" class="blob-num js-line-number" data-line-number="193"></td>
        <td id="LC193" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_Camera_BoundsStrength_Int32 = <span class="pl-s"><span class="pl-pds">&quot;</span>cameraBoundsStrength<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L194" class="blob-num js-line-number" data-line-number="194"></td>
        <td id="LC194" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_audio_Section = <span class="pl-s"><span class="pl-pds">&quot;</span>audio<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L195" class="blob-num js-line-number" data-line-number="195"></td>
        <td id="LC195" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_audio_OnPlaybackDevice_String = <span class="pl-s"><span class="pl-pds">&quot;</span>onPlaybackDevice<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L196" class="blob-num js-line-number" data-line-number="196"></td>
        <td id="LC196" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_audio_OnRecordDevice_String = <span class="pl-s"><span class="pl-pds">&quot;</span>onRecordDevice<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L197" class="blob-num js-line-number" data-line-number="197"></td>
        <td id="LC197" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_audio_OnPlaybackMirrorDevice_String = <span class="pl-s"><span class="pl-pds">&quot;</span>onPlaybackMirrorDevice<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L198" class="blob-num js-line-number" data-line-number="198"></td>
        <td id="LC198" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_audio_OffPlaybackDevice_String = <span class="pl-s"><span class="pl-pds">&quot;</span>offPlaybackDevice<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L199" class="blob-num js-line-number" data-line-number="199"></td>
        <td id="LC199" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_audio_OffRecordDevice_String = <span class="pl-s"><span class="pl-pds">&quot;</span>offRecordDevice<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L200" class="blob-num js-line-number" data-line-number="200"></td>
        <td id="LC200" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_audio_VIVEHDMIGain = <span class="pl-s"><span class="pl-pds">&quot;</span>viveHDMIGain<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L201" class="blob-num js-line-number" data-line-number="201"></td>
        <td id="LC201" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_Power_Section = <span class="pl-s"><span class="pl-pds">&quot;</span>power<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L202" class="blob-num js-line-number" data-line-number="202"></td>
        <td id="LC202" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_Power_PowerOffOnExit_Bool = <span class="pl-s"><span class="pl-pds">&quot;</span>powerOffOnExit<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L203" class="blob-num js-line-number" data-line-number="203"></td>
        <td id="LC203" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_Power_TurnOffScreensTimeout_Float = <span class="pl-s"><span class="pl-pds">&quot;</span>turnOffScreensTimeout<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L204" class="blob-num js-line-number" data-line-number="204"></td>
        <td id="LC204" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_Power_TurnOffControllersTimeout_Float = <span class="pl-s"><span class="pl-pds">&quot;</span>turnOffControllersTimeout<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L205" class="blob-num js-line-number" data-line-number="205"></td>
        <td id="LC205" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_Power_ReturnToWatchdogTimeout_Float = <span class="pl-s"><span class="pl-pds">&quot;</span>returnToWatchdogTimeout<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L206" class="blob-num js-line-number" data-line-number="206"></td>
        <td id="LC206" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_Power_AutoLaunchSteamVROnButtonPress = <span class="pl-s"><span class="pl-pds">&quot;</span>autoLaunchSteamVROnButtonPress<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L207" class="blob-num js-line-number" data-line-number="207"></td>
        <td id="LC207" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_Dashboard_Section = <span class="pl-s"><span class="pl-pds">&quot;</span>dashboard<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L208" class="blob-num js-line-number" data-line-number="208"></td>
        <td id="LC208" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_Dashboard_EnableDashboard_Bool = <span class="pl-s"><span class="pl-pds">&quot;</span>enableDashboard<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L209" class="blob-num js-line-number" data-line-number="209"></td>
        <td id="LC209" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_Dashboard_ArcadeMode_Bool = <span class="pl-s"><span class="pl-pds">&quot;</span>arcadeMode<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L210" class="blob-num js-line-number" data-line-number="210"></td>
        <td id="LC210" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * k_pch_modelskin_Section = <span class="pl-s"><span class="pl-pds">&quot;</span>modelskins<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L211" class="blob-num js-line-number" data-line-number="211"></td>
        <td id="LC211" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * IVRScreenshots_Version = <span class="pl-s"><span class="pl-pds">&quot;</span>IVRScreenshots_001<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L212" class="blob-num js-line-number" data-line-number="212"></td>
        <td id="LC212" class="blob-code blob-code-inner js-file-line"><span class="pl-k">static</span> <span class="pl-k">const</span> <span class="pl-k">char</span> * IVRResources_Version = <span class="pl-s"><span class="pl-pds">&quot;</span>IVRResources_001<span class="pl-pds">&quot;</span></span>;</td>
      </tr>
      <tr>
        <td id="L213" class="blob-num js-line-number" data-line-number="213"></td>
        <td id="LC213" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L214" class="blob-num js-line-number" data-line-number="214"></td>
        <td id="LC214" class="blob-code blob-code-inner js-file-line"><span class="pl-c"><span class="pl-c">//</span> OpenVR Enums</span></td>
      </tr>
      <tr>
        <td id="L215" class="blob-num js-line-number" data-line-number="215"></td>
        <td id="LC215" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L216" class="blob-num js-line-number" data-line-number="216"></td>
        <td id="LC216" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">enum</span> EVREye</td>
      </tr>
      <tr>
        <td id="L217" class="blob-num js-line-number" data-line-number="217"></td>
        <td id="LC217" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L218" class="blob-num js-line-number" data-line-number="218"></td>
        <td id="LC218" class="blob-code blob-code-inner js-file-line">	EVREye_Eye_Left = <span class="pl-c1">0</span>,</td>
      </tr>
      <tr>
        <td id="L219" class="blob-num js-line-number" data-line-number="219"></td>
        <td id="LC219" class="blob-code blob-code-inner js-file-line">	EVREye_Eye_Right = <span class="pl-c1">1</span>,</td>
      </tr>
      <tr>
        <td id="L220" class="blob-num js-line-number" data-line-number="220"></td>
        <td id="LC220" class="blob-code blob-code-inner js-file-line">} EVREye;</td>
      </tr>
      <tr>
        <td id="L221" class="blob-num js-line-number" data-line-number="221"></td>
        <td id="LC221" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L222" class="blob-num js-line-number" data-line-number="222"></td>
        <td id="LC222" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">enum</span> ETextureType</td>
      </tr>
      <tr>
        <td id="L223" class="blob-num js-line-number" data-line-number="223"></td>
        <td id="LC223" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L224" class="blob-num js-line-number" data-line-number="224"></td>
        <td id="LC224" class="blob-code blob-code-inner js-file-line">	ETextureType_TextureType_DirectX = <span class="pl-c1">0</span>,</td>
      </tr>
      <tr>
        <td id="L225" class="blob-num js-line-number" data-line-number="225"></td>
        <td id="LC225" class="blob-code blob-code-inner js-file-line">	ETextureType_TextureType_OpenGL = <span class="pl-c1">1</span>,</td>
      </tr>
      <tr>
        <td id="L226" class="blob-num js-line-number" data-line-number="226"></td>
        <td id="LC226" class="blob-code blob-code-inner js-file-line">	ETextureType_TextureType_Vulkan = <span class="pl-c1">2</span>,</td>
      </tr>
      <tr>
        <td id="L227" class="blob-num js-line-number" data-line-number="227"></td>
        <td id="LC227" class="blob-code blob-code-inner js-file-line">} ETextureType;</td>
      </tr>
      <tr>
        <td id="L228" class="blob-num js-line-number" data-line-number="228"></td>
        <td id="LC228" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L229" class="blob-num js-line-number" data-line-number="229"></td>
        <td id="LC229" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">enum</span> EColorSpace</td>
      </tr>
      <tr>
        <td id="L230" class="blob-num js-line-number" data-line-number="230"></td>
        <td id="LC230" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L231" class="blob-num js-line-number" data-line-number="231"></td>
        <td id="LC231" class="blob-code blob-code-inner js-file-line">	EColorSpace_ColorSpace_Auto = <span class="pl-c1">0</span>,</td>
      </tr>
      <tr>
        <td id="L232" class="blob-num js-line-number" data-line-number="232"></td>
        <td id="LC232" class="blob-code blob-code-inner js-file-line">	EColorSpace_ColorSpace_Gamma = <span class="pl-c1">1</span>,</td>
      </tr>
      <tr>
        <td id="L233" class="blob-num js-line-number" data-line-number="233"></td>
        <td id="LC233" class="blob-code blob-code-inner js-file-line">	EColorSpace_ColorSpace_Linear = <span class="pl-c1">2</span>,</td>
      </tr>
      <tr>
        <td id="L234" class="blob-num js-line-number" data-line-number="234"></td>
        <td id="LC234" class="blob-code blob-code-inner js-file-line">} EColorSpace;</td>
      </tr>
      <tr>
        <td id="L235" class="blob-num js-line-number" data-line-number="235"></td>
        <td id="LC235" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L236" class="blob-num js-line-number" data-line-number="236"></td>
        <td id="LC236" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">enum</span> ETrackingResult</td>
      </tr>
      <tr>
        <td id="L237" class="blob-num js-line-number" data-line-number="237"></td>
        <td id="LC237" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L238" class="blob-num js-line-number" data-line-number="238"></td>
        <td id="LC238" class="blob-code blob-code-inner js-file-line">	ETrackingResult_TrackingResult_Uninitialized = <span class="pl-c1">1</span>,</td>
      </tr>
      <tr>
        <td id="L239" class="blob-num js-line-number" data-line-number="239"></td>
        <td id="LC239" class="blob-code blob-code-inner js-file-line">	ETrackingResult_TrackingResult_Calibrating_InProgress = <span class="pl-c1">100</span>,</td>
      </tr>
      <tr>
        <td id="L240" class="blob-num js-line-number" data-line-number="240"></td>
        <td id="LC240" class="blob-code blob-code-inner js-file-line">	ETrackingResult_TrackingResult_Calibrating_OutOfRange = <span class="pl-c1">101</span>,</td>
      </tr>
      <tr>
        <td id="L241" class="blob-num js-line-number" data-line-number="241"></td>
        <td id="LC241" class="blob-code blob-code-inner js-file-line">	ETrackingResult_TrackingResult_Running_OK = <span class="pl-c1">200</span>,</td>
      </tr>
      <tr>
        <td id="L242" class="blob-num js-line-number" data-line-number="242"></td>
        <td id="LC242" class="blob-code blob-code-inner js-file-line">	ETrackingResult_TrackingResult_Running_OutOfRange = <span class="pl-c1">201</span>,</td>
      </tr>
      <tr>
        <td id="L243" class="blob-num js-line-number" data-line-number="243"></td>
        <td id="LC243" class="blob-code blob-code-inner js-file-line">} ETrackingResult;</td>
      </tr>
      <tr>
        <td id="L244" class="blob-num js-line-number" data-line-number="244"></td>
        <td id="LC244" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L245" class="blob-num js-line-number" data-line-number="245"></td>
        <td id="LC245" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">enum</span> ETrackedDeviceClass</td>
      </tr>
      <tr>
        <td id="L246" class="blob-num js-line-number" data-line-number="246"></td>
        <td id="LC246" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L247" class="blob-num js-line-number" data-line-number="247"></td>
        <td id="LC247" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceClass_TrackedDeviceClass_Invalid = <span class="pl-c1">0</span>,</td>
      </tr>
      <tr>
        <td id="L248" class="blob-num js-line-number" data-line-number="248"></td>
        <td id="LC248" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceClass_TrackedDeviceClass_HMD = <span class="pl-c1">1</span>,</td>
      </tr>
      <tr>
        <td id="L249" class="blob-num js-line-number" data-line-number="249"></td>
        <td id="LC249" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceClass_TrackedDeviceClass_Controller = <span class="pl-c1">2</span>,</td>
      </tr>
      <tr>
        <td id="L250" class="blob-num js-line-number" data-line-number="250"></td>
        <td id="LC250" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceClass_TrackedDeviceClass_GenericTracker = <span class="pl-c1">3</span>,</td>
      </tr>
      <tr>
        <td id="L251" class="blob-num js-line-number" data-line-number="251"></td>
        <td id="LC251" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceClass_TrackedDeviceClass_TrackingReference = <span class="pl-c1">4</span>,</td>
      </tr>
      <tr>
        <td id="L252" class="blob-num js-line-number" data-line-number="252"></td>
        <td id="LC252" class="blob-code blob-code-inner js-file-line">} ETrackedDeviceClass;</td>
      </tr>
      <tr>
        <td id="L253" class="blob-num js-line-number" data-line-number="253"></td>
        <td id="LC253" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L254" class="blob-num js-line-number" data-line-number="254"></td>
        <td id="LC254" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">enum</span> ETrackedControllerRole</td>
      </tr>
      <tr>
        <td id="L255" class="blob-num js-line-number" data-line-number="255"></td>
        <td id="LC255" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L256" class="blob-num js-line-number" data-line-number="256"></td>
        <td id="LC256" class="blob-code blob-code-inner js-file-line">	ETrackedControllerRole_TrackedControllerRole_Invalid = <span class="pl-c1">0</span>,</td>
      </tr>
      <tr>
        <td id="L257" class="blob-num js-line-number" data-line-number="257"></td>
        <td id="LC257" class="blob-code blob-code-inner js-file-line">	ETrackedControllerRole_TrackedControllerRole_LeftHand = <span class="pl-c1">1</span>,</td>
      </tr>
      <tr>
        <td id="L258" class="blob-num js-line-number" data-line-number="258"></td>
        <td id="LC258" class="blob-code blob-code-inner js-file-line">	ETrackedControllerRole_TrackedControllerRole_RightHand = <span class="pl-c1">2</span>,</td>
      </tr>
      <tr>
        <td id="L259" class="blob-num js-line-number" data-line-number="259"></td>
        <td id="LC259" class="blob-code blob-code-inner js-file-line">} ETrackedControllerRole;</td>
      </tr>
      <tr>
        <td id="L260" class="blob-num js-line-number" data-line-number="260"></td>
        <td id="LC260" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L261" class="blob-num js-line-number" data-line-number="261"></td>
        <td id="LC261" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">enum</span> ETrackingUniverseOrigin</td>
      </tr>
      <tr>
        <td id="L262" class="blob-num js-line-number" data-line-number="262"></td>
        <td id="LC262" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L263" class="blob-num js-line-number" data-line-number="263"></td>
        <td id="LC263" class="blob-code blob-code-inner js-file-line">	ETrackingUniverseOrigin_TrackingUniverseSeated = <span class="pl-c1">0</span>,</td>
      </tr>
      <tr>
        <td id="L264" class="blob-num js-line-number" data-line-number="264"></td>
        <td id="LC264" class="blob-code blob-code-inner js-file-line">	ETrackingUniverseOrigin_TrackingUniverseStanding = <span class="pl-c1">1</span>,</td>
      </tr>
      <tr>
        <td id="L265" class="blob-num js-line-number" data-line-number="265"></td>
        <td id="LC265" class="blob-code blob-code-inner js-file-line">	ETrackingUniverseOrigin_TrackingUniverseRawAndUncalibrated = <span class="pl-c1">2</span>,</td>
      </tr>
      <tr>
        <td id="L266" class="blob-num js-line-number" data-line-number="266"></td>
        <td id="LC266" class="blob-code blob-code-inner js-file-line">} ETrackingUniverseOrigin;</td>
      </tr>
      <tr>
        <td id="L267" class="blob-num js-line-number" data-line-number="267"></td>
        <td id="LC267" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L268" class="blob-num js-line-number" data-line-number="268"></td>
        <td id="LC268" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">enum</span> ETrackedDeviceProperty</td>
      </tr>
      <tr>
        <td id="L269" class="blob-num js-line-number" data-line-number="269"></td>
        <td id="LC269" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L270" class="blob-num js-line-number" data-line-number="270"></td>
        <td id="LC270" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_Invalid = <span class="pl-c1">0</span>,</td>
      </tr>
      <tr>
        <td id="L271" class="blob-num js-line-number" data-line-number="271"></td>
        <td id="LC271" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_TrackingSystemName_String = <span class="pl-c1">1000</span>,</td>
      </tr>
      <tr>
        <td id="L272" class="blob-num js-line-number" data-line-number="272"></td>
        <td id="LC272" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_ModelNumber_String = <span class="pl-c1">1001</span>,</td>
      </tr>
      <tr>
        <td id="L273" class="blob-num js-line-number" data-line-number="273"></td>
        <td id="LC273" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_SerialNumber_String = <span class="pl-c1">1002</span>,</td>
      </tr>
      <tr>
        <td id="L274" class="blob-num js-line-number" data-line-number="274"></td>
        <td id="LC274" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_RenderModelName_String = <span class="pl-c1">1003</span>,</td>
      </tr>
      <tr>
        <td id="L275" class="blob-num js-line-number" data-line-number="275"></td>
        <td id="LC275" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_WillDriftInYaw_Bool = <span class="pl-c1">1004</span>,</td>
      </tr>
      <tr>
        <td id="L276" class="blob-num js-line-number" data-line-number="276"></td>
        <td id="LC276" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_ManufacturerName_String = <span class="pl-c1">1005</span>,</td>
      </tr>
      <tr>
        <td id="L277" class="blob-num js-line-number" data-line-number="277"></td>
        <td id="LC277" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_TrackingFirmwareVersion_String = <span class="pl-c1">1006</span>,</td>
      </tr>
      <tr>
        <td id="L278" class="blob-num js-line-number" data-line-number="278"></td>
        <td id="LC278" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_HardwareRevision_String = <span class="pl-c1">1007</span>,</td>
      </tr>
      <tr>
        <td id="L279" class="blob-num js-line-number" data-line-number="279"></td>
        <td id="LC279" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_AllWirelessDongleDescriptions_String = <span class="pl-c1">1008</span>,</td>
      </tr>
      <tr>
        <td id="L280" class="blob-num js-line-number" data-line-number="280"></td>
        <td id="LC280" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_ConnectedWirelessDongle_String = <span class="pl-c1">1009</span>,</td>
      </tr>
      <tr>
        <td id="L281" class="blob-num js-line-number" data-line-number="281"></td>
        <td id="LC281" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_DeviceIsWireless_Bool = <span class="pl-c1">1010</span>,</td>
      </tr>
      <tr>
        <td id="L282" class="blob-num js-line-number" data-line-number="282"></td>
        <td id="LC282" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_DeviceIsCharging_Bool = <span class="pl-c1">1011</span>,</td>
      </tr>
      <tr>
        <td id="L283" class="blob-num js-line-number" data-line-number="283"></td>
        <td id="LC283" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_DeviceBatteryPercentage_Float = <span class="pl-c1">1012</span>,</td>
      </tr>
      <tr>
        <td id="L284" class="blob-num js-line-number" data-line-number="284"></td>
        <td id="LC284" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_StatusDisplayTransform_Matrix34 = <span class="pl-c1">1013</span>,</td>
      </tr>
      <tr>
        <td id="L285" class="blob-num js-line-number" data-line-number="285"></td>
        <td id="LC285" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_Firmware_UpdateAvailable_Bool = <span class="pl-c1">1014</span>,</td>
      </tr>
      <tr>
        <td id="L286" class="blob-num js-line-number" data-line-number="286"></td>
        <td id="LC286" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_Firmware_ManualUpdate_Bool = <span class="pl-c1">1015</span>,</td>
      </tr>
      <tr>
        <td id="L287" class="blob-num js-line-number" data-line-number="287"></td>
        <td id="LC287" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_Firmware_ManualUpdateURL_String = <span class="pl-c1">1016</span>,</td>
      </tr>
      <tr>
        <td id="L288" class="blob-num js-line-number" data-line-number="288"></td>
        <td id="LC288" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_HardwareRevision_Uint64 = <span class="pl-c1">1017</span>,</td>
      </tr>
      <tr>
        <td id="L289" class="blob-num js-line-number" data-line-number="289"></td>
        <td id="LC289" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_FirmwareVersion_Uint64 = <span class="pl-c1">1018</span>,</td>
      </tr>
      <tr>
        <td id="L290" class="blob-num js-line-number" data-line-number="290"></td>
        <td id="LC290" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_FPGAVersion_Uint64 = <span class="pl-c1">1019</span>,</td>
      </tr>
      <tr>
        <td id="L291" class="blob-num js-line-number" data-line-number="291"></td>
        <td id="LC291" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_VRCVersion_Uint64 = <span class="pl-c1">1020</span>,</td>
      </tr>
      <tr>
        <td id="L292" class="blob-num js-line-number" data-line-number="292"></td>
        <td id="LC292" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_RadioVersion_Uint64 = <span class="pl-c1">1021</span>,</td>
      </tr>
      <tr>
        <td id="L293" class="blob-num js-line-number" data-line-number="293"></td>
        <td id="LC293" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_DongleVersion_Uint64 = <span class="pl-c1">1022</span>,</td>
      </tr>
      <tr>
        <td id="L294" class="blob-num js-line-number" data-line-number="294"></td>
        <td id="LC294" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_BlockServerShutdown_Bool = <span class="pl-c1">1023</span>,</td>
      </tr>
      <tr>
        <td id="L295" class="blob-num js-line-number" data-line-number="295"></td>
        <td id="LC295" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_CanUnifyCoordinateSystemWithHmd_Bool = <span class="pl-c1">1024</span>,</td>
      </tr>
      <tr>
        <td id="L296" class="blob-num js-line-number" data-line-number="296"></td>
        <td id="LC296" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_ContainsProximitySensor_Bool = <span class="pl-c1">1025</span>,</td>
      </tr>
      <tr>
        <td id="L297" class="blob-num js-line-number" data-line-number="297"></td>
        <td id="LC297" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_DeviceProvidesBatteryStatus_Bool = <span class="pl-c1">1026</span>,</td>
      </tr>
      <tr>
        <td id="L298" class="blob-num js-line-number" data-line-number="298"></td>
        <td id="LC298" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_DeviceCanPowerOff_Bool = <span class="pl-c1">1027</span>,</td>
      </tr>
      <tr>
        <td id="L299" class="blob-num js-line-number" data-line-number="299"></td>
        <td id="LC299" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_Firmware_ProgrammingTarget_String = <span class="pl-c1">1028</span>,</td>
      </tr>
      <tr>
        <td id="L300" class="blob-num js-line-number" data-line-number="300"></td>
        <td id="LC300" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_DeviceClass_Int32 = <span class="pl-c1">1029</span>,</td>
      </tr>
      <tr>
        <td id="L301" class="blob-num js-line-number" data-line-number="301"></td>
        <td id="LC301" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_HasCamera_Bool = <span class="pl-c1">1030</span>,</td>
      </tr>
      <tr>
        <td id="L302" class="blob-num js-line-number" data-line-number="302"></td>
        <td id="LC302" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_DriverVersion_String = <span class="pl-c1">1031</span>,</td>
      </tr>
      <tr>
        <td id="L303" class="blob-num js-line-number" data-line-number="303"></td>
        <td id="LC303" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_Firmware_ForceUpdateRequired_Bool = <span class="pl-c1">1032</span>,</td>
      </tr>
      <tr>
        <td id="L304" class="blob-num js-line-number" data-line-number="304"></td>
        <td id="LC304" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_ViveSystemButtonFixRequired_Bool = <span class="pl-c1">1033</span>,</td>
      </tr>
      <tr>
        <td id="L305" class="blob-num js-line-number" data-line-number="305"></td>
        <td id="LC305" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_ReportsTimeSinceVSync_Bool = <span class="pl-c1">2000</span>,</td>
      </tr>
      <tr>
        <td id="L306" class="blob-num js-line-number" data-line-number="306"></td>
        <td id="LC306" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_SecondsFromVsyncToPhotons_Float = <span class="pl-c1">2001</span>,</td>
      </tr>
      <tr>
        <td id="L307" class="blob-num js-line-number" data-line-number="307"></td>
        <td id="LC307" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_DisplayFrequency_Float = <span class="pl-c1">2002</span>,</td>
      </tr>
      <tr>
        <td id="L308" class="blob-num js-line-number" data-line-number="308"></td>
        <td id="LC308" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_UserIpdMeters_Float = <span class="pl-c1">2003</span>,</td>
      </tr>
      <tr>
        <td id="L309" class="blob-num js-line-number" data-line-number="309"></td>
        <td id="LC309" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_CurrentUniverseId_Uint64 = <span class="pl-c1">2004</span>,</td>
      </tr>
      <tr>
        <td id="L310" class="blob-num js-line-number" data-line-number="310"></td>
        <td id="LC310" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_PreviousUniverseId_Uint64 = <span class="pl-c1">2005</span>,</td>
      </tr>
      <tr>
        <td id="L311" class="blob-num js-line-number" data-line-number="311"></td>
        <td id="LC311" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_DisplayFirmwareVersion_Uint64 = <span class="pl-c1">2006</span>,</td>
      </tr>
      <tr>
        <td id="L312" class="blob-num js-line-number" data-line-number="312"></td>
        <td id="LC312" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_IsOnDesktop_Bool = <span class="pl-c1">2007</span>,</td>
      </tr>
      <tr>
        <td id="L313" class="blob-num js-line-number" data-line-number="313"></td>
        <td id="LC313" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_DisplayMCType_Int32 = <span class="pl-c1">2008</span>,</td>
      </tr>
      <tr>
        <td id="L314" class="blob-num js-line-number" data-line-number="314"></td>
        <td id="LC314" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_DisplayMCOffset_Float = <span class="pl-c1">2009</span>,</td>
      </tr>
      <tr>
        <td id="L315" class="blob-num js-line-number" data-line-number="315"></td>
        <td id="LC315" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_DisplayMCScale_Float = <span class="pl-c1">2010</span>,</td>
      </tr>
      <tr>
        <td id="L316" class="blob-num js-line-number" data-line-number="316"></td>
        <td id="LC316" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_EdidVendorID_Int32 = <span class="pl-c1">2011</span>,</td>
      </tr>
      <tr>
        <td id="L317" class="blob-num js-line-number" data-line-number="317"></td>
        <td id="LC317" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_DisplayMCImageLeft_String = <span class="pl-c1">2012</span>,</td>
      </tr>
      <tr>
        <td id="L318" class="blob-num js-line-number" data-line-number="318"></td>
        <td id="LC318" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_DisplayMCImageRight_String = <span class="pl-c1">2013</span>,</td>
      </tr>
      <tr>
        <td id="L319" class="blob-num js-line-number" data-line-number="319"></td>
        <td id="LC319" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_DisplayGCBlackClamp_Float = <span class="pl-c1">2014</span>,</td>
      </tr>
      <tr>
        <td id="L320" class="blob-num js-line-number" data-line-number="320"></td>
        <td id="LC320" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_EdidProductID_Int32 = <span class="pl-c1">2015</span>,</td>
      </tr>
      <tr>
        <td id="L321" class="blob-num js-line-number" data-line-number="321"></td>
        <td id="LC321" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_CameraToHeadTransform_Matrix34 = <span class="pl-c1">2016</span>,</td>
      </tr>
      <tr>
        <td id="L322" class="blob-num js-line-number" data-line-number="322"></td>
        <td id="LC322" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_DisplayGCType_Int32 = <span class="pl-c1">2017</span>,</td>
      </tr>
      <tr>
        <td id="L323" class="blob-num js-line-number" data-line-number="323"></td>
        <td id="LC323" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_DisplayGCOffset_Float = <span class="pl-c1">2018</span>,</td>
      </tr>
      <tr>
        <td id="L324" class="blob-num js-line-number" data-line-number="324"></td>
        <td id="LC324" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_DisplayGCScale_Float = <span class="pl-c1">2019</span>,</td>
      </tr>
      <tr>
        <td id="L325" class="blob-num js-line-number" data-line-number="325"></td>
        <td id="LC325" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_DisplayGCPrescale_Float = <span class="pl-c1">2020</span>,</td>
      </tr>
      <tr>
        <td id="L326" class="blob-num js-line-number" data-line-number="326"></td>
        <td id="LC326" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_DisplayGCImage_String = <span class="pl-c1">2021</span>,</td>
      </tr>
      <tr>
        <td id="L327" class="blob-num js-line-number" data-line-number="327"></td>
        <td id="LC327" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_LensCenterLeftU_Float = <span class="pl-c1">2022</span>,</td>
      </tr>
      <tr>
        <td id="L328" class="blob-num js-line-number" data-line-number="328"></td>
        <td id="LC328" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_LensCenterLeftV_Float = <span class="pl-c1">2023</span>,</td>
      </tr>
      <tr>
        <td id="L329" class="blob-num js-line-number" data-line-number="329"></td>
        <td id="LC329" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_LensCenterRightU_Float = <span class="pl-c1">2024</span>,</td>
      </tr>
      <tr>
        <td id="L330" class="blob-num js-line-number" data-line-number="330"></td>
        <td id="LC330" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_LensCenterRightV_Float = <span class="pl-c1">2025</span>,</td>
      </tr>
      <tr>
        <td id="L331" class="blob-num js-line-number" data-line-number="331"></td>
        <td id="LC331" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_UserHeadToEyeDepthMeters_Float = <span class="pl-c1">2026</span>,</td>
      </tr>
      <tr>
        <td id="L332" class="blob-num js-line-number" data-line-number="332"></td>
        <td id="LC332" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_CameraFirmwareVersion_Uint64 = <span class="pl-c1">2027</span>,</td>
      </tr>
      <tr>
        <td id="L333" class="blob-num js-line-number" data-line-number="333"></td>
        <td id="LC333" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_CameraFirmwareDescription_String = <span class="pl-c1">2028</span>,</td>
      </tr>
      <tr>
        <td id="L334" class="blob-num js-line-number" data-line-number="334"></td>
        <td id="LC334" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_DisplayFPGAVersion_Uint64 = <span class="pl-c1">2029</span>,</td>
      </tr>
      <tr>
        <td id="L335" class="blob-num js-line-number" data-line-number="335"></td>
        <td id="LC335" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_DisplayBootloaderVersion_Uint64 = <span class="pl-c1">2030</span>,</td>
      </tr>
      <tr>
        <td id="L336" class="blob-num js-line-number" data-line-number="336"></td>
        <td id="LC336" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_DisplayHardwareVersion_Uint64 = <span class="pl-c1">2031</span>,</td>
      </tr>
      <tr>
        <td id="L337" class="blob-num js-line-number" data-line-number="337"></td>
        <td id="LC337" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_AudioFirmwareVersion_Uint64 = <span class="pl-c1">2032</span>,</td>
      </tr>
      <tr>
        <td id="L338" class="blob-num js-line-number" data-line-number="338"></td>
        <td id="LC338" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_CameraCompatibilityMode_Int32 = <span class="pl-c1">2033</span>,</td>
      </tr>
      <tr>
        <td id="L339" class="blob-num js-line-number" data-line-number="339"></td>
        <td id="LC339" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_ScreenshotHorizontalFieldOfViewDegrees_Float = <span class="pl-c1">2034</span>,</td>
      </tr>
      <tr>
        <td id="L340" class="blob-num js-line-number" data-line-number="340"></td>
        <td id="LC340" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_ScreenshotVerticalFieldOfViewDegrees_Float = <span class="pl-c1">2035</span>,</td>
      </tr>
      <tr>
        <td id="L341" class="blob-num js-line-number" data-line-number="341"></td>
        <td id="LC341" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_DisplaySuppressed_Bool = <span class="pl-c1">2036</span>,</td>
      </tr>
      <tr>
        <td id="L342" class="blob-num js-line-number" data-line-number="342"></td>
        <td id="LC342" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_DisplayAllowNightMode_Bool = <span class="pl-c1">2037</span>,</td>
      </tr>
      <tr>
        <td id="L343" class="blob-num js-line-number" data-line-number="343"></td>
        <td id="LC343" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_AttachedDeviceId_String = <span class="pl-c1">3000</span>,</td>
      </tr>
      <tr>
        <td id="L344" class="blob-num js-line-number" data-line-number="344"></td>
        <td id="LC344" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_SupportedButtons_Uint64 = <span class="pl-c1">3001</span>,</td>
      </tr>
      <tr>
        <td id="L345" class="blob-num js-line-number" data-line-number="345"></td>
        <td id="LC345" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_Axis0Type_Int32 = <span class="pl-c1">3002</span>,</td>
      </tr>
      <tr>
        <td id="L346" class="blob-num js-line-number" data-line-number="346"></td>
        <td id="LC346" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_Axis1Type_Int32 = <span class="pl-c1">3003</span>,</td>
      </tr>
      <tr>
        <td id="L347" class="blob-num js-line-number" data-line-number="347"></td>
        <td id="LC347" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_Axis2Type_Int32 = <span class="pl-c1">3004</span>,</td>
      </tr>
      <tr>
        <td id="L348" class="blob-num js-line-number" data-line-number="348"></td>
        <td id="LC348" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_Axis3Type_Int32 = <span class="pl-c1">3005</span>,</td>
      </tr>
      <tr>
        <td id="L349" class="blob-num js-line-number" data-line-number="349"></td>
        <td id="LC349" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_Axis4Type_Int32 = <span class="pl-c1">3006</span>,</td>
      </tr>
      <tr>
        <td id="L350" class="blob-num js-line-number" data-line-number="350"></td>
        <td id="LC350" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_ControllerRoleHint_Int32 = <span class="pl-c1">3007</span>,</td>
      </tr>
      <tr>
        <td id="L351" class="blob-num js-line-number" data-line-number="351"></td>
        <td id="LC351" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_FieldOfViewLeftDegrees_Float = <span class="pl-c1">4000</span>,</td>
      </tr>
      <tr>
        <td id="L352" class="blob-num js-line-number" data-line-number="352"></td>
        <td id="LC352" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_FieldOfViewRightDegrees_Float = <span class="pl-c1">4001</span>,</td>
      </tr>
      <tr>
        <td id="L353" class="blob-num js-line-number" data-line-number="353"></td>
        <td id="LC353" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_FieldOfViewTopDegrees_Float = <span class="pl-c1">4002</span>,</td>
      </tr>
      <tr>
        <td id="L354" class="blob-num js-line-number" data-line-number="354"></td>
        <td id="LC354" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_FieldOfViewBottomDegrees_Float = <span class="pl-c1">4003</span>,</td>
      </tr>
      <tr>
        <td id="L355" class="blob-num js-line-number" data-line-number="355"></td>
        <td id="LC355" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_TrackingRangeMinimumMeters_Float = <span class="pl-c1">4004</span>,</td>
      </tr>
      <tr>
        <td id="L356" class="blob-num js-line-number" data-line-number="356"></td>
        <td id="LC356" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_TrackingRangeMaximumMeters_Float = <span class="pl-c1">4005</span>,</td>
      </tr>
      <tr>
        <td id="L357" class="blob-num js-line-number" data-line-number="357"></td>
        <td id="LC357" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_ModeLabel_String = <span class="pl-c1">4006</span>,</td>
      </tr>
      <tr>
        <td id="L358" class="blob-num js-line-number" data-line-number="358"></td>
        <td id="LC358" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_IconPathName_String = <span class="pl-c1">5000</span>,</td>
      </tr>
      <tr>
        <td id="L359" class="blob-num js-line-number" data-line-number="359"></td>
        <td id="LC359" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_NamedIconPathDeviceOff_String = <span class="pl-c1">5001</span>,</td>
      </tr>
      <tr>
        <td id="L360" class="blob-num js-line-number" data-line-number="360"></td>
        <td id="LC360" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_NamedIconPathDeviceSearching_String = <span class="pl-c1">5002</span>,</td>
      </tr>
      <tr>
        <td id="L361" class="blob-num js-line-number" data-line-number="361"></td>
        <td id="LC361" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_NamedIconPathDeviceSearchingAlert_String = <span class="pl-c1">5003</span>,</td>
      </tr>
      <tr>
        <td id="L362" class="blob-num js-line-number" data-line-number="362"></td>
        <td id="LC362" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_NamedIconPathDeviceReady_String = <span class="pl-c1">5004</span>,</td>
      </tr>
      <tr>
        <td id="L363" class="blob-num js-line-number" data-line-number="363"></td>
        <td id="LC363" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_NamedIconPathDeviceReadyAlert_String = <span class="pl-c1">5005</span>,</td>
      </tr>
      <tr>
        <td id="L364" class="blob-num js-line-number" data-line-number="364"></td>
        <td id="LC364" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_NamedIconPathDeviceNotReady_String = <span class="pl-c1">5006</span>,</td>
      </tr>
      <tr>
        <td id="L365" class="blob-num js-line-number" data-line-number="365"></td>
        <td id="LC365" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_NamedIconPathDeviceStandby_String = <span class="pl-c1">5007</span>,</td>
      </tr>
      <tr>
        <td id="L366" class="blob-num js-line-number" data-line-number="366"></td>
        <td id="LC366" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_NamedIconPathDeviceAlertLow_String = <span class="pl-c1">5008</span>,</td>
      </tr>
      <tr>
        <td id="L367" class="blob-num js-line-number" data-line-number="367"></td>
        <td id="LC367" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_VendorSpecific_Reserved_Start = <span class="pl-c1">10000</span>,</td>
      </tr>
      <tr>
        <td id="L368" class="blob-num js-line-number" data-line-number="368"></td>
        <td id="LC368" class="blob-code blob-code-inner js-file-line">	ETrackedDeviceProperty_Prop_VendorSpecific_Reserved_End = <span class="pl-c1">10999</span>,</td>
      </tr>
      <tr>
        <td id="L369" class="blob-num js-line-number" data-line-number="369"></td>
        <td id="LC369" class="blob-code blob-code-inner js-file-line">} ETrackedDeviceProperty;</td>
      </tr>
      <tr>
        <td id="L370" class="blob-num js-line-number" data-line-number="370"></td>
        <td id="LC370" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L371" class="blob-num js-line-number" data-line-number="371"></td>
        <td id="LC371" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">enum</span> ETrackedPropertyError</td>
      </tr>
      <tr>
        <td id="L372" class="blob-num js-line-number" data-line-number="372"></td>
        <td id="LC372" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L373" class="blob-num js-line-number" data-line-number="373"></td>
        <td id="LC373" class="blob-code blob-code-inner js-file-line">	ETrackedPropertyError_TrackedProp_Success = <span class="pl-c1">0</span>,</td>
      </tr>
      <tr>
        <td id="L374" class="blob-num js-line-number" data-line-number="374"></td>
        <td id="LC374" class="blob-code blob-code-inner js-file-line">	ETrackedPropertyError_TrackedProp_WrongDataType = <span class="pl-c1">1</span>,</td>
      </tr>
      <tr>
        <td id="L375" class="blob-num js-line-number" data-line-number="375"></td>
        <td id="LC375" class="blob-code blob-code-inner js-file-line">	ETrackedPropertyError_TrackedProp_WrongDeviceClass = <span class="pl-c1">2</span>,</td>
      </tr>
      <tr>
        <td id="L376" class="blob-num js-line-number" data-line-number="376"></td>
        <td id="LC376" class="blob-code blob-code-inner js-file-line">	ETrackedPropertyError_TrackedProp_BufferTooSmall = <span class="pl-c1">3</span>,</td>
      </tr>
      <tr>
        <td id="L377" class="blob-num js-line-number" data-line-number="377"></td>
        <td id="LC377" class="blob-code blob-code-inner js-file-line">	ETrackedPropertyError_TrackedProp_UnknownProperty = <span class="pl-c1">4</span>,</td>
      </tr>
      <tr>
        <td id="L378" class="blob-num js-line-number" data-line-number="378"></td>
        <td id="LC378" class="blob-code blob-code-inner js-file-line">	ETrackedPropertyError_TrackedProp_InvalidDevice = <span class="pl-c1">5</span>,</td>
      </tr>
      <tr>
        <td id="L379" class="blob-num js-line-number" data-line-number="379"></td>
        <td id="LC379" class="blob-code blob-code-inner js-file-line">	ETrackedPropertyError_TrackedProp_CouldNotContactServer = <span class="pl-c1">6</span>,</td>
      </tr>
      <tr>
        <td id="L380" class="blob-num js-line-number" data-line-number="380"></td>
        <td id="LC380" class="blob-code blob-code-inner js-file-line">	ETrackedPropertyError_TrackedProp_ValueNotProvidedByDevice = <span class="pl-c1">7</span>,</td>
      </tr>
      <tr>
        <td id="L381" class="blob-num js-line-number" data-line-number="381"></td>
        <td id="LC381" class="blob-code blob-code-inner js-file-line">	ETrackedPropertyError_TrackedProp_StringExceedsMaximumLength = <span class="pl-c1">8</span>,</td>
      </tr>
      <tr>
        <td id="L382" class="blob-num js-line-number" data-line-number="382"></td>
        <td id="LC382" class="blob-code blob-code-inner js-file-line">	ETrackedPropertyError_TrackedProp_NotYetAvailable = <span class="pl-c1">9</span>,</td>
      </tr>
      <tr>
        <td id="L383" class="blob-num js-line-number" data-line-number="383"></td>
        <td id="LC383" class="blob-code blob-code-inner js-file-line">	ETrackedPropertyError_TrackedProp_PermissionDenied = <span class="pl-c1">10</span>,</td>
      </tr>
      <tr>
        <td id="L384" class="blob-num js-line-number" data-line-number="384"></td>
        <td id="LC384" class="blob-code blob-code-inner js-file-line">} ETrackedPropertyError;</td>
      </tr>
      <tr>
        <td id="L385" class="blob-num js-line-number" data-line-number="385"></td>
        <td id="LC385" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L386" class="blob-num js-line-number" data-line-number="386"></td>
        <td id="LC386" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">enum</span> EVRSubmitFlags</td>
      </tr>
      <tr>
        <td id="L387" class="blob-num js-line-number" data-line-number="387"></td>
        <td id="LC387" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L388" class="blob-num js-line-number" data-line-number="388"></td>
        <td id="LC388" class="blob-code blob-code-inner js-file-line">	EVRSubmitFlags_Submit_Default = <span class="pl-c1">0</span>,</td>
      </tr>
      <tr>
        <td id="L389" class="blob-num js-line-number" data-line-number="389"></td>
        <td id="LC389" class="blob-code blob-code-inner js-file-line">	EVRSubmitFlags_Submit_LensDistortionAlreadyApplied = <span class="pl-c1">1</span>,</td>
      </tr>
      <tr>
        <td id="L390" class="blob-num js-line-number" data-line-number="390"></td>
        <td id="LC390" class="blob-code blob-code-inner js-file-line">	EVRSubmitFlags_Submit_GlRenderBuffer = <span class="pl-c1">2</span>,</td>
      </tr>
      <tr>
        <td id="L391" class="blob-num js-line-number" data-line-number="391"></td>
        <td id="LC391" class="blob-code blob-code-inner js-file-line">	EVRSubmitFlags_Submit_Reserved = <span class="pl-c1">4</span>,</td>
      </tr>
      <tr>
        <td id="L392" class="blob-num js-line-number" data-line-number="392"></td>
        <td id="LC392" class="blob-code blob-code-inner js-file-line">} EVRSubmitFlags;</td>
      </tr>
      <tr>
        <td id="L393" class="blob-num js-line-number" data-line-number="393"></td>
        <td id="LC393" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L394" class="blob-num js-line-number" data-line-number="394"></td>
        <td id="LC394" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">enum</span> EVRState</td>
      </tr>
      <tr>
        <td id="L395" class="blob-num js-line-number" data-line-number="395"></td>
        <td id="LC395" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L396" class="blob-num js-line-number" data-line-number="396"></td>
        <td id="LC396" class="blob-code blob-code-inner js-file-line">	EVRState_VRState_Undefined = -<span class="pl-c1">1</span>,</td>
      </tr>
      <tr>
        <td id="L397" class="blob-num js-line-number" data-line-number="397"></td>
        <td id="LC397" class="blob-code blob-code-inner js-file-line">	EVRState_VRState_Off = <span class="pl-c1">0</span>,</td>
      </tr>
      <tr>
        <td id="L398" class="blob-num js-line-number" data-line-number="398"></td>
        <td id="LC398" class="blob-code blob-code-inner js-file-line">	EVRState_VRState_Searching = <span class="pl-c1">1</span>,</td>
      </tr>
      <tr>
        <td id="L399" class="blob-num js-line-number" data-line-number="399"></td>
        <td id="LC399" class="blob-code blob-code-inner js-file-line">	EVRState_VRState_Searching_Alert = <span class="pl-c1">2</span>,</td>
      </tr>
      <tr>
        <td id="L400" class="blob-num js-line-number" data-line-number="400"></td>
        <td id="LC400" class="blob-code blob-code-inner js-file-line">	EVRState_VRState_Ready = <span class="pl-c1">3</span>,</td>
      </tr>
      <tr>
        <td id="L401" class="blob-num js-line-number" data-line-number="401"></td>
        <td id="LC401" class="blob-code blob-code-inner js-file-line">	EVRState_VRState_Ready_Alert = <span class="pl-c1">4</span>,</td>
      </tr>
      <tr>
        <td id="L402" class="blob-num js-line-number" data-line-number="402"></td>
        <td id="LC402" class="blob-code blob-code-inner js-file-line">	EVRState_VRState_NotReady = <span class="pl-c1">5</span>,</td>
      </tr>
      <tr>
        <td id="L403" class="blob-num js-line-number" data-line-number="403"></td>
        <td id="LC403" class="blob-code blob-code-inner js-file-line">	EVRState_VRState_Standby = <span class="pl-c1">6</span>,</td>
      </tr>
      <tr>
        <td id="L404" class="blob-num js-line-number" data-line-number="404"></td>
        <td id="LC404" class="blob-code blob-code-inner js-file-line">	EVRState_VRState_Ready_Alert_Low = <span class="pl-c1">7</span>,</td>
      </tr>
      <tr>
        <td id="L405" class="blob-num js-line-number" data-line-number="405"></td>
        <td id="LC405" class="blob-code blob-code-inner js-file-line">} EVRState;</td>
      </tr>
      <tr>
        <td id="L406" class="blob-num js-line-number" data-line-number="406"></td>
        <td id="LC406" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L407" class="blob-num js-line-number" data-line-number="407"></td>
        <td id="LC407" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">enum</span> EVREventType</td>
      </tr>
      <tr>
        <td id="L408" class="blob-num js-line-number" data-line-number="408"></td>
        <td id="LC408" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L409" class="blob-num js-line-number" data-line-number="409"></td>
        <td id="LC409" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_None = <span class="pl-c1">0</span>,</td>
      </tr>
      <tr>
        <td id="L410" class="blob-num js-line-number" data-line-number="410"></td>
        <td id="LC410" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_TrackedDeviceActivated = <span class="pl-c1">100</span>,</td>
      </tr>
      <tr>
        <td id="L411" class="blob-num js-line-number" data-line-number="411"></td>
        <td id="LC411" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_TrackedDeviceDeactivated = <span class="pl-c1">101</span>,</td>
      </tr>
      <tr>
        <td id="L412" class="blob-num js-line-number" data-line-number="412"></td>
        <td id="LC412" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_TrackedDeviceUpdated = <span class="pl-c1">102</span>,</td>
      </tr>
      <tr>
        <td id="L413" class="blob-num js-line-number" data-line-number="413"></td>
        <td id="LC413" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_TrackedDeviceUserInteractionStarted = <span class="pl-c1">103</span>,</td>
      </tr>
      <tr>
        <td id="L414" class="blob-num js-line-number" data-line-number="414"></td>
        <td id="LC414" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_TrackedDeviceUserInteractionEnded = <span class="pl-c1">104</span>,</td>
      </tr>
      <tr>
        <td id="L415" class="blob-num js-line-number" data-line-number="415"></td>
        <td id="LC415" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_IpdChanged = <span class="pl-c1">105</span>,</td>
      </tr>
      <tr>
        <td id="L416" class="blob-num js-line-number" data-line-number="416"></td>
        <td id="LC416" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_EnterStandbyMode = <span class="pl-c1">106</span>,</td>
      </tr>
      <tr>
        <td id="L417" class="blob-num js-line-number" data-line-number="417"></td>
        <td id="LC417" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_LeaveStandbyMode = <span class="pl-c1">107</span>,</td>
      </tr>
      <tr>
        <td id="L418" class="blob-num js-line-number" data-line-number="418"></td>
        <td id="LC418" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_TrackedDeviceRoleChanged = <span class="pl-c1">108</span>,</td>
      </tr>
      <tr>
        <td id="L419" class="blob-num js-line-number" data-line-number="419"></td>
        <td id="LC419" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_WatchdogWakeUpRequested = <span class="pl-c1">109</span>,</td>
      </tr>
      <tr>
        <td id="L420" class="blob-num js-line-number" data-line-number="420"></td>
        <td id="LC420" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_LensDistortionChanged = <span class="pl-c1">110</span>,</td>
      </tr>
      <tr>
        <td id="L421" class="blob-num js-line-number" data-line-number="421"></td>
        <td id="LC421" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_ButtonPress = <span class="pl-c1">200</span>,</td>
      </tr>
      <tr>
        <td id="L422" class="blob-num js-line-number" data-line-number="422"></td>
        <td id="LC422" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_ButtonUnpress = <span class="pl-c1">201</span>,</td>
      </tr>
      <tr>
        <td id="L423" class="blob-num js-line-number" data-line-number="423"></td>
        <td id="LC423" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_ButtonTouch = <span class="pl-c1">202</span>,</td>
      </tr>
      <tr>
        <td id="L424" class="blob-num js-line-number" data-line-number="424"></td>
        <td id="LC424" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_ButtonUntouch = <span class="pl-c1">203</span>,</td>
      </tr>
      <tr>
        <td id="L425" class="blob-num js-line-number" data-line-number="425"></td>
        <td id="LC425" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_MouseMove = <span class="pl-c1">300</span>,</td>
      </tr>
      <tr>
        <td id="L426" class="blob-num js-line-number" data-line-number="426"></td>
        <td id="LC426" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_MouseButtonDown = <span class="pl-c1">301</span>,</td>
      </tr>
      <tr>
        <td id="L427" class="blob-num js-line-number" data-line-number="427"></td>
        <td id="LC427" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_MouseButtonUp = <span class="pl-c1">302</span>,</td>
      </tr>
      <tr>
        <td id="L428" class="blob-num js-line-number" data-line-number="428"></td>
        <td id="LC428" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_FocusEnter = <span class="pl-c1">303</span>,</td>
      </tr>
      <tr>
        <td id="L429" class="blob-num js-line-number" data-line-number="429"></td>
        <td id="LC429" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_FocusLeave = <span class="pl-c1">304</span>,</td>
      </tr>
      <tr>
        <td id="L430" class="blob-num js-line-number" data-line-number="430"></td>
        <td id="LC430" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_Scroll = <span class="pl-c1">305</span>,</td>
      </tr>
      <tr>
        <td id="L431" class="blob-num js-line-number" data-line-number="431"></td>
        <td id="LC431" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_TouchPadMove = <span class="pl-c1">306</span>,</td>
      </tr>
      <tr>
        <td id="L432" class="blob-num js-line-number" data-line-number="432"></td>
        <td id="LC432" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_OverlayFocusChanged = <span class="pl-c1">307</span>,</td>
      </tr>
      <tr>
        <td id="L433" class="blob-num js-line-number" data-line-number="433"></td>
        <td id="LC433" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_InputFocusCaptured = <span class="pl-c1">400</span>,</td>
      </tr>
      <tr>
        <td id="L434" class="blob-num js-line-number" data-line-number="434"></td>
        <td id="LC434" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_InputFocusReleased = <span class="pl-c1">401</span>,</td>
      </tr>
      <tr>
        <td id="L435" class="blob-num js-line-number" data-line-number="435"></td>
        <td id="LC435" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_SceneFocusLost = <span class="pl-c1">402</span>,</td>
      </tr>
      <tr>
        <td id="L436" class="blob-num js-line-number" data-line-number="436"></td>
        <td id="LC436" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_SceneFocusGained = <span class="pl-c1">403</span>,</td>
      </tr>
      <tr>
        <td id="L437" class="blob-num js-line-number" data-line-number="437"></td>
        <td id="LC437" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_SceneApplicationChanged = <span class="pl-c1">404</span>,</td>
      </tr>
      <tr>
        <td id="L438" class="blob-num js-line-number" data-line-number="438"></td>
        <td id="LC438" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_SceneFocusChanged = <span class="pl-c1">405</span>,</td>
      </tr>
      <tr>
        <td id="L439" class="blob-num js-line-number" data-line-number="439"></td>
        <td id="LC439" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_InputFocusChanged = <span class="pl-c1">406</span>,</td>
      </tr>
      <tr>
        <td id="L440" class="blob-num js-line-number" data-line-number="440"></td>
        <td id="LC440" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_SceneApplicationSecondaryRenderingStarted = <span class="pl-c1">407</span>,</td>
      </tr>
      <tr>
        <td id="L441" class="blob-num js-line-number" data-line-number="441"></td>
        <td id="LC441" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_HideRenderModels = <span class="pl-c1">410</span>,</td>
      </tr>
      <tr>
        <td id="L442" class="blob-num js-line-number" data-line-number="442"></td>
        <td id="LC442" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_ShowRenderModels = <span class="pl-c1">411</span>,</td>
      </tr>
      <tr>
        <td id="L443" class="blob-num js-line-number" data-line-number="443"></td>
        <td id="LC443" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_OverlayShown = <span class="pl-c1">500</span>,</td>
      </tr>
      <tr>
        <td id="L444" class="blob-num js-line-number" data-line-number="444"></td>
        <td id="LC444" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_OverlayHidden = <span class="pl-c1">501</span>,</td>
      </tr>
      <tr>
        <td id="L445" class="blob-num js-line-number" data-line-number="445"></td>
        <td id="LC445" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_DashboardActivated = <span class="pl-c1">502</span>,</td>
      </tr>
      <tr>
        <td id="L446" class="blob-num js-line-number" data-line-number="446"></td>
        <td id="LC446" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_DashboardDeactivated = <span class="pl-c1">503</span>,</td>
      </tr>
      <tr>
        <td id="L447" class="blob-num js-line-number" data-line-number="447"></td>
        <td id="LC447" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_DashboardThumbSelected = <span class="pl-c1">504</span>,</td>
      </tr>
      <tr>
        <td id="L448" class="blob-num js-line-number" data-line-number="448"></td>
        <td id="LC448" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_DashboardRequested = <span class="pl-c1">505</span>,</td>
      </tr>
      <tr>
        <td id="L449" class="blob-num js-line-number" data-line-number="449"></td>
        <td id="LC449" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_ResetDashboard = <span class="pl-c1">506</span>,</td>
      </tr>
      <tr>
        <td id="L450" class="blob-num js-line-number" data-line-number="450"></td>
        <td id="LC450" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_RenderToast = <span class="pl-c1">507</span>,</td>
      </tr>
      <tr>
        <td id="L451" class="blob-num js-line-number" data-line-number="451"></td>
        <td id="LC451" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_ImageLoaded = <span class="pl-c1">508</span>,</td>
      </tr>
      <tr>
        <td id="L452" class="blob-num js-line-number" data-line-number="452"></td>
        <td id="LC452" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_ShowKeyboard = <span class="pl-c1">509</span>,</td>
      </tr>
      <tr>
        <td id="L453" class="blob-num js-line-number" data-line-number="453"></td>
        <td id="LC453" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_HideKeyboard = <span class="pl-c1">510</span>,</td>
      </tr>
      <tr>
        <td id="L454" class="blob-num js-line-number" data-line-number="454"></td>
        <td id="LC454" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_OverlayGamepadFocusGained = <span class="pl-c1">511</span>,</td>
      </tr>
      <tr>
        <td id="L455" class="blob-num js-line-number" data-line-number="455"></td>
        <td id="LC455" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_OverlayGamepadFocusLost = <span class="pl-c1">512</span>,</td>
      </tr>
      <tr>
        <td id="L456" class="blob-num js-line-number" data-line-number="456"></td>
        <td id="LC456" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_OverlaySharedTextureChanged = <span class="pl-c1">513</span>,</td>
      </tr>
      <tr>
        <td id="L457" class="blob-num js-line-number" data-line-number="457"></td>
        <td id="LC457" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_DashboardGuideButtonDown = <span class="pl-c1">514</span>,</td>
      </tr>
      <tr>
        <td id="L458" class="blob-num js-line-number" data-line-number="458"></td>
        <td id="LC458" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_DashboardGuideButtonUp = <span class="pl-c1">515</span>,</td>
      </tr>
      <tr>
        <td id="L459" class="blob-num js-line-number" data-line-number="459"></td>
        <td id="LC459" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_ScreenshotTriggered = <span class="pl-c1">516</span>,</td>
      </tr>
      <tr>
        <td id="L460" class="blob-num js-line-number" data-line-number="460"></td>
        <td id="LC460" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_ImageFailed = <span class="pl-c1">517</span>,</td>
      </tr>
      <tr>
        <td id="L461" class="blob-num js-line-number" data-line-number="461"></td>
        <td id="LC461" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_DashboardOverlayCreated = <span class="pl-c1">518</span>,</td>
      </tr>
      <tr>
        <td id="L462" class="blob-num js-line-number" data-line-number="462"></td>
        <td id="LC462" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_RequestScreenshot = <span class="pl-c1">520</span>,</td>
      </tr>
      <tr>
        <td id="L463" class="blob-num js-line-number" data-line-number="463"></td>
        <td id="LC463" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_ScreenshotTaken = <span class="pl-c1">521</span>,</td>
      </tr>
      <tr>
        <td id="L464" class="blob-num js-line-number" data-line-number="464"></td>
        <td id="LC464" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_ScreenshotFailed = <span class="pl-c1">522</span>,</td>
      </tr>
      <tr>
        <td id="L465" class="blob-num js-line-number" data-line-number="465"></td>
        <td id="LC465" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_SubmitScreenshotToDashboard = <span class="pl-c1">523</span>,</td>
      </tr>
      <tr>
        <td id="L466" class="blob-num js-line-number" data-line-number="466"></td>
        <td id="LC466" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_ScreenshotProgressToDashboard = <span class="pl-c1">524</span>,</td>
      </tr>
      <tr>
        <td id="L467" class="blob-num js-line-number" data-line-number="467"></td>
        <td id="LC467" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_Notification_Shown = <span class="pl-c1">600</span>,</td>
      </tr>
      <tr>
        <td id="L468" class="blob-num js-line-number" data-line-number="468"></td>
        <td id="LC468" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_Notification_Hidden = <span class="pl-c1">601</span>,</td>
      </tr>
      <tr>
        <td id="L469" class="blob-num js-line-number" data-line-number="469"></td>
        <td id="LC469" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_Notification_BeginInteraction = <span class="pl-c1">602</span>,</td>
      </tr>
      <tr>
        <td id="L470" class="blob-num js-line-number" data-line-number="470"></td>
        <td id="LC470" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_Notification_Destroyed = <span class="pl-c1">603</span>,</td>
      </tr>
      <tr>
        <td id="L471" class="blob-num js-line-number" data-line-number="471"></td>
        <td id="LC471" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_Quit = <span class="pl-c1">700</span>,</td>
      </tr>
      <tr>
        <td id="L472" class="blob-num js-line-number" data-line-number="472"></td>
        <td id="LC472" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_ProcessQuit = <span class="pl-c1">701</span>,</td>
      </tr>
      <tr>
        <td id="L473" class="blob-num js-line-number" data-line-number="473"></td>
        <td id="LC473" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_QuitAborted_UserPrompt = <span class="pl-c1">702</span>,</td>
      </tr>
      <tr>
        <td id="L474" class="blob-num js-line-number" data-line-number="474"></td>
        <td id="LC474" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_QuitAcknowledged = <span class="pl-c1">703</span>,</td>
      </tr>
      <tr>
        <td id="L475" class="blob-num js-line-number" data-line-number="475"></td>
        <td id="LC475" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_DriverRequestedQuit = <span class="pl-c1">704</span>,</td>
      </tr>
      <tr>
        <td id="L476" class="blob-num js-line-number" data-line-number="476"></td>
        <td id="LC476" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_ChaperoneDataHasChanged = <span class="pl-c1">800</span>,</td>
      </tr>
      <tr>
        <td id="L477" class="blob-num js-line-number" data-line-number="477"></td>
        <td id="LC477" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_ChaperoneUniverseHasChanged = <span class="pl-c1">801</span>,</td>
      </tr>
      <tr>
        <td id="L478" class="blob-num js-line-number" data-line-number="478"></td>
        <td id="LC478" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_ChaperoneTempDataHasChanged = <span class="pl-c1">802</span>,</td>
      </tr>
      <tr>
        <td id="L479" class="blob-num js-line-number" data-line-number="479"></td>
        <td id="LC479" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_ChaperoneSettingsHaveChanged = <span class="pl-c1">803</span>,</td>
      </tr>
      <tr>
        <td id="L480" class="blob-num js-line-number" data-line-number="480"></td>
        <td id="LC480" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_SeatedZeroPoseReset = <span class="pl-c1">804</span>,</td>
      </tr>
      <tr>
        <td id="L481" class="blob-num js-line-number" data-line-number="481"></td>
        <td id="LC481" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_AudioSettingsHaveChanged = <span class="pl-c1">820</span>,</td>
      </tr>
      <tr>
        <td id="L482" class="blob-num js-line-number" data-line-number="482"></td>
        <td id="LC482" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_BackgroundSettingHasChanged = <span class="pl-c1">850</span>,</td>
      </tr>
      <tr>
        <td id="L483" class="blob-num js-line-number" data-line-number="483"></td>
        <td id="LC483" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_CameraSettingsHaveChanged = <span class="pl-c1">851</span>,</td>
      </tr>
      <tr>
        <td id="L484" class="blob-num js-line-number" data-line-number="484"></td>
        <td id="LC484" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_ReprojectionSettingHasChanged = <span class="pl-c1">852</span>,</td>
      </tr>
      <tr>
        <td id="L485" class="blob-num js-line-number" data-line-number="485"></td>
        <td id="LC485" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_ModelSkinSettingsHaveChanged = <span class="pl-c1">853</span>,</td>
      </tr>
      <tr>
        <td id="L486" class="blob-num js-line-number" data-line-number="486"></td>
        <td id="LC486" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_EnvironmentSettingsHaveChanged = <span class="pl-c1">854</span>,</td>
      </tr>
      <tr>
        <td id="L487" class="blob-num js-line-number" data-line-number="487"></td>
        <td id="LC487" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_PowerSettingsHaveChanged = <span class="pl-c1">855</span>,</td>
      </tr>
      <tr>
        <td id="L488" class="blob-num js-line-number" data-line-number="488"></td>
        <td id="LC488" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_StatusUpdate = <span class="pl-c1">900</span>,</td>
      </tr>
      <tr>
        <td id="L489" class="blob-num js-line-number" data-line-number="489"></td>
        <td id="LC489" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_MCImageUpdated = <span class="pl-c1">1000</span>,</td>
      </tr>
      <tr>
        <td id="L490" class="blob-num js-line-number" data-line-number="490"></td>
        <td id="LC490" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_FirmwareUpdateStarted = <span class="pl-c1">1100</span>,</td>
      </tr>
      <tr>
        <td id="L491" class="blob-num js-line-number" data-line-number="491"></td>
        <td id="LC491" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_FirmwareUpdateFinished = <span class="pl-c1">1101</span>,</td>
      </tr>
      <tr>
        <td id="L492" class="blob-num js-line-number" data-line-number="492"></td>
        <td id="LC492" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_KeyboardClosed = <span class="pl-c1">1200</span>,</td>
      </tr>
      <tr>
        <td id="L493" class="blob-num js-line-number" data-line-number="493"></td>
        <td id="LC493" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_KeyboardCharInput = <span class="pl-c1">1201</span>,</td>
      </tr>
      <tr>
        <td id="L494" class="blob-num js-line-number" data-line-number="494"></td>
        <td id="LC494" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_KeyboardDone = <span class="pl-c1">1202</span>,</td>
      </tr>
      <tr>
        <td id="L495" class="blob-num js-line-number" data-line-number="495"></td>
        <td id="LC495" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_ApplicationTransitionStarted = <span class="pl-c1">1300</span>,</td>
      </tr>
      <tr>
        <td id="L496" class="blob-num js-line-number" data-line-number="496"></td>
        <td id="LC496" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_ApplicationTransitionAborted = <span class="pl-c1">1301</span>,</td>
      </tr>
      <tr>
        <td id="L497" class="blob-num js-line-number" data-line-number="497"></td>
        <td id="LC497" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_ApplicationTransitionNewAppStarted = <span class="pl-c1">1302</span>,</td>
      </tr>
      <tr>
        <td id="L498" class="blob-num js-line-number" data-line-number="498"></td>
        <td id="LC498" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_ApplicationListUpdated = <span class="pl-c1">1303</span>,</td>
      </tr>
      <tr>
        <td id="L499" class="blob-num js-line-number" data-line-number="499"></td>
        <td id="LC499" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_ApplicationMimeTypeLoad = <span class="pl-c1">1304</span>,</td>
      </tr>
      <tr>
        <td id="L500" class="blob-num js-line-number" data-line-number="500"></td>
        <td id="LC500" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_Compositor_MirrorWindowShown = <span class="pl-c1">1400</span>,</td>
      </tr>
      <tr>
        <td id="L501" class="blob-num js-line-number" data-line-number="501"></td>
        <td id="LC501" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_Compositor_MirrorWindowHidden = <span class="pl-c1">1401</span>,</td>
      </tr>
      <tr>
        <td id="L502" class="blob-num js-line-number" data-line-number="502"></td>
        <td id="LC502" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_Compositor_ChaperoneBoundsShown = <span class="pl-c1">1410</span>,</td>
      </tr>
      <tr>
        <td id="L503" class="blob-num js-line-number" data-line-number="503"></td>
        <td id="LC503" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_Compositor_ChaperoneBoundsHidden = <span class="pl-c1">1411</span>,</td>
      </tr>
      <tr>
        <td id="L504" class="blob-num js-line-number" data-line-number="504"></td>
        <td id="LC504" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_TrackedCamera_StartVideoStream = <span class="pl-c1">1500</span>,</td>
      </tr>
      <tr>
        <td id="L505" class="blob-num js-line-number" data-line-number="505"></td>
        <td id="LC505" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_TrackedCamera_StopVideoStream = <span class="pl-c1">1501</span>,</td>
      </tr>
      <tr>
        <td id="L506" class="blob-num js-line-number" data-line-number="506"></td>
        <td id="LC506" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_TrackedCamera_PauseVideoStream = <span class="pl-c1">1502</span>,</td>
      </tr>
      <tr>
        <td id="L507" class="blob-num js-line-number" data-line-number="507"></td>
        <td id="LC507" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_TrackedCamera_ResumeVideoStream = <span class="pl-c1">1503</span>,</td>
      </tr>
      <tr>
        <td id="L508" class="blob-num js-line-number" data-line-number="508"></td>
        <td id="LC508" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_TrackedCamera_EditingSurface = <span class="pl-c1">1550</span>,</td>
      </tr>
      <tr>
        <td id="L509" class="blob-num js-line-number" data-line-number="509"></td>
        <td id="LC509" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_PerformanceTest_EnableCapture = <span class="pl-c1">1600</span>,</td>
      </tr>
      <tr>
        <td id="L510" class="blob-num js-line-number" data-line-number="510"></td>
        <td id="LC510" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_PerformanceTest_DisableCapture = <span class="pl-c1">1601</span>,</td>
      </tr>
      <tr>
        <td id="L511" class="blob-num js-line-number" data-line-number="511"></td>
        <td id="LC511" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_PerformanceTest_FidelityLevel = <span class="pl-c1">1602</span>,</td>
      </tr>
      <tr>
        <td id="L512" class="blob-num js-line-number" data-line-number="512"></td>
        <td id="LC512" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_MessageOverlay_Closed = <span class="pl-c1">1650</span>,</td>
      </tr>
      <tr>
        <td id="L513" class="blob-num js-line-number" data-line-number="513"></td>
        <td id="LC513" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_VendorSpecific_Reserved_Start = <span class="pl-c1">10000</span>,</td>
      </tr>
      <tr>
        <td id="L514" class="blob-num js-line-number" data-line-number="514"></td>
        <td id="LC514" class="blob-code blob-code-inner js-file-line">	EVREventType_VREvent_VendorSpecific_Reserved_End = <span class="pl-c1">19999</span>,</td>
      </tr>
      <tr>
        <td id="L515" class="blob-num js-line-number" data-line-number="515"></td>
        <td id="LC515" class="blob-code blob-code-inner js-file-line">} EVREventType;</td>
      </tr>
      <tr>
        <td id="L516" class="blob-num js-line-number" data-line-number="516"></td>
        <td id="LC516" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L517" class="blob-num js-line-number" data-line-number="517"></td>
        <td id="LC517" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">enum</span> EDeviceActivityLevel</td>
      </tr>
      <tr>
        <td id="L518" class="blob-num js-line-number" data-line-number="518"></td>
        <td id="LC518" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L519" class="blob-num js-line-number" data-line-number="519"></td>
        <td id="LC519" class="blob-code blob-code-inner js-file-line">	EDeviceActivityLevel_k_EDeviceActivityLevel_Unknown = -<span class="pl-c1">1</span>,</td>
      </tr>
      <tr>
        <td id="L520" class="blob-num js-line-number" data-line-number="520"></td>
        <td id="LC520" class="blob-code blob-code-inner js-file-line">	EDeviceActivityLevel_k_EDeviceActivityLevel_Idle = <span class="pl-c1">0</span>,</td>
      </tr>
      <tr>
        <td id="L521" class="blob-num js-line-number" data-line-number="521"></td>
        <td id="LC521" class="blob-code blob-code-inner js-file-line">	EDeviceActivityLevel_k_EDeviceActivityLevel_UserInteraction = <span class="pl-c1">1</span>,</td>
      </tr>
      <tr>
        <td id="L522" class="blob-num js-line-number" data-line-number="522"></td>
        <td id="LC522" class="blob-code blob-code-inner js-file-line">	EDeviceActivityLevel_k_EDeviceActivityLevel_UserInteraction_Timeout = <span class="pl-c1">2</span>,</td>
      </tr>
      <tr>
        <td id="L523" class="blob-num js-line-number" data-line-number="523"></td>
        <td id="LC523" class="blob-code blob-code-inner js-file-line">	EDeviceActivityLevel_k_EDeviceActivityLevel_Standby = <span class="pl-c1">3</span>,</td>
      </tr>
      <tr>
        <td id="L524" class="blob-num js-line-number" data-line-number="524"></td>
        <td id="LC524" class="blob-code blob-code-inner js-file-line">} EDeviceActivityLevel;</td>
      </tr>
      <tr>
        <td id="L525" class="blob-num js-line-number" data-line-number="525"></td>
        <td id="LC525" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L526" class="blob-num js-line-number" data-line-number="526"></td>
        <td id="LC526" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">enum</span> EVRButtonId</td>
      </tr>
      <tr>
        <td id="L527" class="blob-num js-line-number" data-line-number="527"></td>
        <td id="LC527" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L528" class="blob-num js-line-number" data-line-number="528"></td>
        <td id="LC528" class="blob-code blob-code-inner js-file-line">	EVRButtonId_k_EButton_System = <span class="pl-c1">0</span>,</td>
      </tr>
      <tr>
        <td id="L529" class="blob-num js-line-number" data-line-number="529"></td>
        <td id="LC529" class="blob-code blob-code-inner js-file-line">	EVRButtonId_k_EButton_ApplicationMenu = <span class="pl-c1">1</span>,</td>
      </tr>
      <tr>
        <td id="L530" class="blob-num js-line-number" data-line-number="530"></td>
        <td id="LC530" class="blob-code blob-code-inner js-file-line">	EVRButtonId_k_EButton_Grip = <span class="pl-c1">2</span>,</td>
      </tr>
      <tr>
        <td id="L531" class="blob-num js-line-number" data-line-number="531"></td>
        <td id="LC531" class="blob-code blob-code-inner js-file-line">	EVRButtonId_k_EButton_DPad_Left = <span class="pl-c1">3</span>,</td>
      </tr>
      <tr>
        <td id="L532" class="blob-num js-line-number" data-line-number="532"></td>
        <td id="LC532" class="blob-code blob-code-inner js-file-line">	EVRButtonId_k_EButton_DPad_Up = <span class="pl-c1">4</span>,</td>
      </tr>
      <tr>
        <td id="L533" class="blob-num js-line-number" data-line-number="533"></td>
        <td id="LC533" class="blob-code blob-code-inner js-file-line">	EVRButtonId_k_EButton_DPad_Right = <span class="pl-c1">5</span>,</td>
      </tr>
      <tr>
        <td id="L534" class="blob-num js-line-number" data-line-number="534"></td>
        <td id="LC534" class="blob-code blob-code-inner js-file-line">	EVRButtonId_k_EButton_DPad_Down = <span class="pl-c1">6</span>,</td>
      </tr>
      <tr>
        <td id="L535" class="blob-num js-line-number" data-line-number="535"></td>
        <td id="LC535" class="blob-code blob-code-inner js-file-line">	EVRButtonId_k_EButton_A = <span class="pl-c1">7</span>,</td>
      </tr>
      <tr>
        <td id="L536" class="blob-num js-line-number" data-line-number="536"></td>
        <td id="LC536" class="blob-code blob-code-inner js-file-line">	EVRButtonId_k_EButton_ProximitySensor = <span class="pl-c1">31</span>,</td>
      </tr>
      <tr>
        <td id="L537" class="blob-num js-line-number" data-line-number="537"></td>
        <td id="LC537" class="blob-code blob-code-inner js-file-line">	EVRButtonId_k_EButton_Axis0 = <span class="pl-c1">32</span>,</td>
      </tr>
      <tr>
        <td id="L538" class="blob-num js-line-number" data-line-number="538"></td>
        <td id="LC538" class="blob-code blob-code-inner js-file-line">	EVRButtonId_k_EButton_Axis1 = <span class="pl-c1">33</span>,</td>
      </tr>
      <tr>
        <td id="L539" class="blob-num js-line-number" data-line-number="539"></td>
        <td id="LC539" class="blob-code blob-code-inner js-file-line">	EVRButtonId_k_EButton_Axis2 = <span class="pl-c1">34</span>,</td>
      </tr>
      <tr>
        <td id="L540" class="blob-num js-line-number" data-line-number="540"></td>
        <td id="LC540" class="blob-code blob-code-inner js-file-line">	EVRButtonId_k_EButton_Axis3 = <span class="pl-c1">35</span>,</td>
      </tr>
      <tr>
        <td id="L541" class="blob-num js-line-number" data-line-number="541"></td>
        <td id="LC541" class="blob-code blob-code-inner js-file-line">	EVRButtonId_k_EButton_Axis4 = <span class="pl-c1">36</span>,</td>
      </tr>
      <tr>
        <td id="L542" class="blob-num js-line-number" data-line-number="542"></td>
        <td id="LC542" class="blob-code blob-code-inner js-file-line">	EVRButtonId_k_EButton_SteamVR_Touchpad = <span class="pl-c1">32</span>,</td>
      </tr>
      <tr>
        <td id="L543" class="blob-num js-line-number" data-line-number="543"></td>
        <td id="LC543" class="blob-code blob-code-inner js-file-line">	EVRButtonId_k_EButton_SteamVR_Trigger = <span class="pl-c1">33</span>,</td>
      </tr>
      <tr>
        <td id="L544" class="blob-num js-line-number" data-line-number="544"></td>
        <td id="LC544" class="blob-code blob-code-inner js-file-line">	EVRButtonId_k_EButton_Dashboard_Back = <span class="pl-c1">2</span>,</td>
      </tr>
      <tr>
        <td id="L545" class="blob-num js-line-number" data-line-number="545"></td>
        <td id="LC545" class="blob-code blob-code-inner js-file-line">	EVRButtonId_k_EButton_Max = <span class="pl-c1">64</span>,</td>
      </tr>
      <tr>
        <td id="L546" class="blob-num js-line-number" data-line-number="546"></td>
        <td id="LC546" class="blob-code blob-code-inner js-file-line">} EVRButtonId;</td>
      </tr>
      <tr>
        <td id="L547" class="blob-num js-line-number" data-line-number="547"></td>
        <td id="LC547" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L548" class="blob-num js-line-number" data-line-number="548"></td>
        <td id="LC548" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">enum</span> EVRMouseButton</td>
      </tr>
      <tr>
        <td id="L549" class="blob-num js-line-number" data-line-number="549"></td>
        <td id="LC549" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L550" class="blob-num js-line-number" data-line-number="550"></td>
        <td id="LC550" class="blob-code blob-code-inner js-file-line">	EVRMouseButton_VRMouseButton_Left = <span class="pl-c1">1</span>,</td>
      </tr>
      <tr>
        <td id="L551" class="blob-num js-line-number" data-line-number="551"></td>
        <td id="LC551" class="blob-code blob-code-inner js-file-line">	EVRMouseButton_VRMouseButton_Right = <span class="pl-c1">2</span>,</td>
      </tr>
      <tr>
        <td id="L552" class="blob-num js-line-number" data-line-number="552"></td>
        <td id="LC552" class="blob-code blob-code-inner js-file-line">	EVRMouseButton_VRMouseButton_Middle = <span class="pl-c1">4</span>,</td>
      </tr>
      <tr>
        <td id="L553" class="blob-num js-line-number" data-line-number="553"></td>
        <td id="LC553" class="blob-code blob-code-inner js-file-line">} EVRMouseButton;</td>
      </tr>
      <tr>
        <td id="L554" class="blob-num js-line-number" data-line-number="554"></td>
        <td id="LC554" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L555" class="blob-num js-line-number" data-line-number="555"></td>
        <td id="LC555" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">enum</span> EHiddenAreaMeshType</td>
      </tr>
      <tr>
        <td id="L556" class="blob-num js-line-number" data-line-number="556"></td>
        <td id="LC556" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L557" class="blob-num js-line-number" data-line-number="557"></td>
        <td id="LC557" class="blob-code blob-code-inner js-file-line">	EHiddenAreaMeshType_k_eHiddenAreaMesh_Standard = <span class="pl-c1">0</span>,</td>
      </tr>
      <tr>
        <td id="L558" class="blob-num js-line-number" data-line-number="558"></td>
        <td id="LC558" class="blob-code blob-code-inner js-file-line">	EHiddenAreaMeshType_k_eHiddenAreaMesh_Inverse = <span class="pl-c1">1</span>,</td>
      </tr>
      <tr>
        <td id="L559" class="blob-num js-line-number" data-line-number="559"></td>
        <td id="LC559" class="blob-code blob-code-inner js-file-line">	EHiddenAreaMeshType_k_eHiddenAreaMesh_LineLoop = <span class="pl-c1">2</span>,</td>
      </tr>
      <tr>
        <td id="L560" class="blob-num js-line-number" data-line-number="560"></td>
        <td id="LC560" class="blob-code blob-code-inner js-file-line">} EHiddenAreaMeshType;</td>
      </tr>
      <tr>
        <td id="L561" class="blob-num js-line-number" data-line-number="561"></td>
        <td id="LC561" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L562" class="blob-num js-line-number" data-line-number="562"></td>
        <td id="LC562" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">enum</span> EVRControllerAxisType</td>
      </tr>
      <tr>
        <td id="L563" class="blob-num js-line-number" data-line-number="563"></td>
        <td id="LC563" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L564" class="blob-num js-line-number" data-line-number="564"></td>
        <td id="LC564" class="blob-code blob-code-inner js-file-line">	EVRControllerAxisType_k_eControllerAxis_None = <span class="pl-c1">0</span>,</td>
      </tr>
      <tr>
        <td id="L565" class="blob-num js-line-number" data-line-number="565"></td>
        <td id="LC565" class="blob-code blob-code-inner js-file-line">	EVRControllerAxisType_k_eControllerAxis_TrackPad = <span class="pl-c1">1</span>,</td>
      </tr>
      <tr>
        <td id="L566" class="blob-num js-line-number" data-line-number="566"></td>
        <td id="LC566" class="blob-code blob-code-inner js-file-line">	EVRControllerAxisType_k_eControllerAxis_Joystick = <span class="pl-c1">2</span>,</td>
      </tr>
      <tr>
        <td id="L567" class="blob-num js-line-number" data-line-number="567"></td>
        <td id="LC567" class="blob-code blob-code-inner js-file-line">	EVRControllerAxisType_k_eControllerAxis_Trigger = <span class="pl-c1">3</span>,</td>
      </tr>
      <tr>
        <td id="L568" class="blob-num js-line-number" data-line-number="568"></td>
        <td id="LC568" class="blob-code blob-code-inner js-file-line">} EVRControllerAxisType;</td>
      </tr>
      <tr>
        <td id="L569" class="blob-num js-line-number" data-line-number="569"></td>
        <td id="LC569" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L570" class="blob-num js-line-number" data-line-number="570"></td>
        <td id="LC570" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">enum</span> EVRControllerEventOutputType</td>
      </tr>
      <tr>
        <td id="L571" class="blob-num js-line-number" data-line-number="571"></td>
        <td id="LC571" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L572" class="blob-num js-line-number" data-line-number="572"></td>
        <td id="LC572" class="blob-code blob-code-inner js-file-line">	EVRControllerEventOutputType_ControllerEventOutput_OSEvents = <span class="pl-c1">0</span>,</td>
      </tr>
      <tr>
        <td id="L573" class="blob-num js-line-number" data-line-number="573"></td>
        <td id="LC573" class="blob-code blob-code-inner js-file-line">	EVRControllerEventOutputType_ControllerEventOutput_VREvents = <span class="pl-c1">1</span>,</td>
      </tr>
      <tr>
        <td id="L574" class="blob-num js-line-number" data-line-number="574"></td>
        <td id="LC574" class="blob-code blob-code-inner js-file-line">} EVRControllerEventOutputType;</td>
      </tr>
      <tr>
        <td id="L575" class="blob-num js-line-number" data-line-number="575"></td>
        <td id="LC575" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L576" class="blob-num js-line-number" data-line-number="576"></td>
        <td id="LC576" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">enum</span> ECollisionBoundsStyle</td>
      </tr>
      <tr>
        <td id="L577" class="blob-num js-line-number" data-line-number="577"></td>
        <td id="LC577" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L578" class="blob-num js-line-number" data-line-number="578"></td>
        <td id="LC578" class="blob-code blob-code-inner js-file-line">	ECollisionBoundsStyle_COLLISION_BOUNDS_STYLE_BEGINNER = <span class="pl-c1">0</span>,</td>
      </tr>
      <tr>
        <td id="L579" class="blob-num js-line-number" data-line-number="579"></td>
        <td id="LC579" class="blob-code blob-code-inner js-file-line">	ECollisionBoundsStyle_COLLISION_BOUNDS_STYLE_INTERMEDIATE = <span class="pl-c1">1</span>,</td>
      </tr>
      <tr>
        <td id="L580" class="blob-num js-line-number" data-line-number="580"></td>
        <td id="LC580" class="blob-code blob-code-inner js-file-line">	ECollisionBoundsStyle_COLLISION_BOUNDS_STYLE_SQUARES = <span class="pl-c1">2</span>,</td>
      </tr>
      <tr>
        <td id="L581" class="blob-num js-line-number" data-line-number="581"></td>
        <td id="LC581" class="blob-code blob-code-inner js-file-line">	ECollisionBoundsStyle_COLLISION_BOUNDS_STYLE_ADVANCED = <span class="pl-c1">3</span>,</td>
      </tr>
      <tr>
        <td id="L582" class="blob-num js-line-number" data-line-number="582"></td>
        <td id="LC582" class="blob-code blob-code-inner js-file-line">	ECollisionBoundsStyle_COLLISION_BOUNDS_STYLE_NONE = <span class="pl-c1">4</span>,</td>
      </tr>
      <tr>
        <td id="L583" class="blob-num js-line-number" data-line-number="583"></td>
        <td id="LC583" class="blob-code blob-code-inner js-file-line">	ECollisionBoundsStyle_COLLISION_BOUNDS_STYLE_COUNT = <span class="pl-c1">5</span>,</td>
      </tr>
      <tr>
        <td id="L584" class="blob-num js-line-number" data-line-number="584"></td>
        <td id="LC584" class="blob-code blob-code-inner js-file-line">} ECollisionBoundsStyle;</td>
      </tr>
      <tr>
        <td id="L585" class="blob-num js-line-number" data-line-number="585"></td>
        <td id="LC585" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L586" class="blob-num js-line-number" data-line-number="586"></td>
        <td id="LC586" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">enum</span> EVROverlayError</td>
      </tr>
      <tr>
        <td id="L587" class="blob-num js-line-number" data-line-number="587"></td>
        <td id="LC587" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L588" class="blob-num js-line-number" data-line-number="588"></td>
        <td id="LC588" class="blob-code blob-code-inner js-file-line">	EVROverlayError_VROverlayError_None = <span class="pl-c1">0</span>,</td>
      </tr>
      <tr>
        <td id="L589" class="blob-num js-line-number" data-line-number="589"></td>
        <td id="LC589" class="blob-code blob-code-inner js-file-line">	EVROverlayError_VROverlayError_UnknownOverlay = <span class="pl-c1">10</span>,</td>
      </tr>
      <tr>
        <td id="L590" class="blob-num js-line-number" data-line-number="590"></td>
        <td id="LC590" class="blob-code blob-code-inner js-file-line">	EVROverlayError_VROverlayError_InvalidHandle = <span class="pl-c1">11</span>,</td>
      </tr>
      <tr>
        <td id="L591" class="blob-num js-line-number" data-line-number="591"></td>
        <td id="LC591" class="blob-code blob-code-inner js-file-line">	EVROverlayError_VROverlayError_PermissionDenied = <span class="pl-c1">12</span>,</td>
      </tr>
      <tr>
        <td id="L592" class="blob-num js-line-number" data-line-number="592"></td>
        <td id="LC592" class="blob-code blob-code-inner js-file-line">	EVROverlayError_VROverlayError_OverlayLimitExceeded = <span class="pl-c1">13</span>,</td>
      </tr>
      <tr>
        <td id="L593" class="blob-num js-line-number" data-line-number="593"></td>
        <td id="LC593" class="blob-code blob-code-inner js-file-line">	EVROverlayError_VROverlayError_WrongVisibilityType = <span class="pl-c1">14</span>,</td>
      </tr>
      <tr>
        <td id="L594" class="blob-num js-line-number" data-line-number="594"></td>
        <td id="LC594" class="blob-code blob-code-inner js-file-line">	EVROverlayError_VROverlayError_KeyTooLong = <span class="pl-c1">15</span>,</td>
      </tr>
      <tr>
        <td id="L595" class="blob-num js-line-number" data-line-number="595"></td>
        <td id="LC595" class="blob-code blob-code-inner js-file-line">	EVROverlayError_VROverlayError_NameTooLong = <span class="pl-c1">16</span>,</td>
      </tr>
      <tr>
        <td id="L596" class="blob-num js-line-number" data-line-number="596"></td>
        <td id="LC596" class="blob-code blob-code-inner js-file-line">	EVROverlayError_VROverlayError_KeyInUse = <span class="pl-c1">17</span>,</td>
      </tr>
      <tr>
        <td id="L597" class="blob-num js-line-number" data-line-number="597"></td>
        <td id="LC597" class="blob-code blob-code-inner js-file-line">	EVROverlayError_VROverlayError_WrongTransformType = <span class="pl-c1">18</span>,</td>
      </tr>
      <tr>
        <td id="L598" class="blob-num js-line-number" data-line-number="598"></td>
        <td id="LC598" class="blob-code blob-code-inner js-file-line">	EVROverlayError_VROverlayError_InvalidTrackedDevice = <span class="pl-c1">19</span>,</td>
      </tr>
      <tr>
        <td id="L599" class="blob-num js-line-number" data-line-number="599"></td>
        <td id="LC599" class="blob-code blob-code-inner js-file-line">	EVROverlayError_VROverlayError_InvalidParameter = <span class="pl-c1">20</span>,</td>
      </tr>
      <tr>
        <td id="L600" class="blob-num js-line-number" data-line-number="600"></td>
        <td id="LC600" class="blob-code blob-code-inner js-file-line">	EVROverlayError_VROverlayError_ThumbnailCantBeDestroyed = <span class="pl-c1">21</span>,</td>
      </tr>
      <tr>
        <td id="L601" class="blob-num js-line-number" data-line-number="601"></td>
        <td id="LC601" class="blob-code blob-code-inner js-file-line">	EVROverlayError_VROverlayError_ArrayTooSmall = <span class="pl-c1">22</span>,</td>
      </tr>
      <tr>
        <td id="L602" class="blob-num js-line-number" data-line-number="602"></td>
        <td id="LC602" class="blob-code blob-code-inner js-file-line">	EVROverlayError_VROverlayError_RequestFailed = <span class="pl-c1">23</span>,</td>
      </tr>
      <tr>
        <td id="L603" class="blob-num js-line-number" data-line-number="603"></td>
        <td id="LC603" class="blob-code blob-code-inner js-file-line">	EVROverlayError_VROverlayError_InvalidTexture = <span class="pl-c1">24</span>,</td>
      </tr>
      <tr>
        <td id="L604" class="blob-num js-line-number" data-line-number="604"></td>
        <td id="LC604" class="blob-code blob-code-inner js-file-line">	EVROverlayError_VROverlayError_UnableToLoadFile = <span class="pl-c1">25</span>,</td>
      </tr>
      <tr>
        <td id="L605" class="blob-num js-line-number" data-line-number="605"></td>
        <td id="LC605" class="blob-code blob-code-inner js-file-line">	EVROverlayError_VROverlayError_KeyboardAlreadyInUse = <span class="pl-c1">26</span>,</td>
      </tr>
      <tr>
        <td id="L606" class="blob-num js-line-number" data-line-number="606"></td>
        <td id="LC606" class="blob-code blob-code-inner js-file-line">	EVROverlayError_VROverlayError_NoNeighbor = <span class="pl-c1">27</span>,</td>
      </tr>
      <tr>
        <td id="L607" class="blob-num js-line-number" data-line-number="607"></td>
        <td id="LC607" class="blob-code blob-code-inner js-file-line">	EVROverlayError_VROverlayError_TooManyMaskPrimitives = <span class="pl-c1">29</span>,</td>
      </tr>
      <tr>
        <td id="L608" class="blob-num js-line-number" data-line-number="608"></td>
        <td id="LC608" class="blob-code blob-code-inner js-file-line">	EVROverlayError_VROverlayError_BadMaskPrimitive = <span class="pl-c1">30</span>,</td>
      </tr>
      <tr>
        <td id="L609" class="blob-num js-line-number" data-line-number="609"></td>
        <td id="LC609" class="blob-code blob-code-inner js-file-line">} EVROverlayError;</td>
      </tr>
      <tr>
        <td id="L610" class="blob-num js-line-number" data-line-number="610"></td>
        <td id="LC610" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L611" class="blob-num js-line-number" data-line-number="611"></td>
        <td id="LC611" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">enum</span> EVRApplicationType</td>
      </tr>
      <tr>
        <td id="L612" class="blob-num js-line-number" data-line-number="612"></td>
        <td id="LC612" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L613" class="blob-num js-line-number" data-line-number="613"></td>
        <td id="LC613" class="blob-code blob-code-inner js-file-line">	EVRApplicationType_VRApplication_Other = <span class="pl-c1">0</span>,</td>
      </tr>
      <tr>
        <td id="L614" class="blob-num js-line-number" data-line-number="614"></td>
        <td id="LC614" class="blob-code blob-code-inner js-file-line">	EVRApplicationType_VRApplication_Scene = <span class="pl-c1">1</span>,</td>
      </tr>
      <tr>
        <td id="L615" class="blob-num js-line-number" data-line-number="615"></td>
        <td id="LC615" class="blob-code blob-code-inner js-file-line">	EVRApplicationType_VRApplication_Overlay = <span class="pl-c1">2</span>,</td>
      </tr>
      <tr>
        <td id="L616" class="blob-num js-line-number" data-line-number="616"></td>
        <td id="LC616" class="blob-code blob-code-inner js-file-line">	EVRApplicationType_VRApplication_Background = <span class="pl-c1">3</span>,</td>
      </tr>
      <tr>
        <td id="L617" class="blob-num js-line-number" data-line-number="617"></td>
        <td id="LC617" class="blob-code blob-code-inner js-file-line">	EVRApplicationType_VRApplication_Utility = <span class="pl-c1">4</span>,</td>
      </tr>
      <tr>
        <td id="L618" class="blob-num js-line-number" data-line-number="618"></td>
        <td id="LC618" class="blob-code blob-code-inner js-file-line">	EVRApplicationType_VRApplication_VRMonitor = <span class="pl-c1">5</span>,</td>
      </tr>
      <tr>
        <td id="L619" class="blob-num js-line-number" data-line-number="619"></td>
        <td id="LC619" class="blob-code blob-code-inner js-file-line">	EVRApplicationType_VRApplication_SteamWatchdog = <span class="pl-c1">6</span>,</td>
      </tr>
      <tr>
        <td id="L620" class="blob-num js-line-number" data-line-number="620"></td>
        <td id="LC620" class="blob-code blob-code-inner js-file-line">	EVRApplicationType_VRApplication_Max = <span class="pl-c1">7</span>,</td>
      </tr>
      <tr>
        <td id="L621" class="blob-num js-line-number" data-line-number="621"></td>
        <td id="LC621" class="blob-code blob-code-inner js-file-line">} EVRApplicationType;</td>
      </tr>
      <tr>
        <td id="L622" class="blob-num js-line-number" data-line-number="622"></td>
        <td id="LC622" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L623" class="blob-num js-line-number" data-line-number="623"></td>
        <td id="LC623" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">enum</span> EVRFirmwareError</td>
      </tr>
      <tr>
        <td id="L624" class="blob-num js-line-number" data-line-number="624"></td>
        <td id="LC624" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L625" class="blob-num js-line-number" data-line-number="625"></td>
        <td id="LC625" class="blob-code blob-code-inner js-file-line">	EVRFirmwareError_VRFirmwareError_None = <span class="pl-c1">0</span>,</td>
      </tr>
      <tr>
        <td id="L626" class="blob-num js-line-number" data-line-number="626"></td>
        <td id="LC626" class="blob-code blob-code-inner js-file-line">	EVRFirmwareError_VRFirmwareError_Success = <span class="pl-c1">1</span>,</td>
      </tr>
      <tr>
        <td id="L627" class="blob-num js-line-number" data-line-number="627"></td>
        <td id="LC627" class="blob-code blob-code-inner js-file-line">	EVRFirmwareError_VRFirmwareError_Fail = <span class="pl-c1">2</span>,</td>
      </tr>
      <tr>
        <td id="L628" class="blob-num js-line-number" data-line-number="628"></td>
        <td id="LC628" class="blob-code blob-code-inner js-file-line">} EVRFirmwareError;</td>
      </tr>
      <tr>
        <td id="L629" class="blob-num js-line-number" data-line-number="629"></td>
        <td id="LC629" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L630" class="blob-num js-line-number" data-line-number="630"></td>
        <td id="LC630" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">enum</span> EVRNotificationError</td>
      </tr>
      <tr>
        <td id="L631" class="blob-num js-line-number" data-line-number="631"></td>
        <td id="LC631" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L632" class="blob-num js-line-number" data-line-number="632"></td>
        <td id="LC632" class="blob-code blob-code-inner js-file-line">	EVRNotificationError_VRNotificationError_OK = <span class="pl-c1">0</span>,</td>
      </tr>
      <tr>
        <td id="L633" class="blob-num js-line-number" data-line-number="633"></td>
        <td id="LC633" class="blob-code blob-code-inner js-file-line">	EVRNotificationError_VRNotificationError_InvalidNotificationId = <span class="pl-c1">100</span>,</td>
      </tr>
      <tr>
        <td id="L634" class="blob-num js-line-number" data-line-number="634"></td>
        <td id="LC634" class="blob-code blob-code-inner js-file-line">	EVRNotificationError_VRNotificationError_NotificationQueueFull = <span class="pl-c1">101</span>,</td>
      </tr>
      <tr>
        <td id="L635" class="blob-num js-line-number" data-line-number="635"></td>
        <td id="LC635" class="blob-code blob-code-inner js-file-line">	EVRNotificationError_VRNotificationError_InvalidOverlayHandle = <span class="pl-c1">102</span>,</td>
      </tr>
      <tr>
        <td id="L636" class="blob-num js-line-number" data-line-number="636"></td>
        <td id="LC636" class="blob-code blob-code-inner js-file-line">	EVRNotificationError_VRNotificationError_SystemWithUserValueAlreadyExists = <span class="pl-c1">103</span>,</td>
      </tr>
      <tr>
        <td id="L637" class="blob-num js-line-number" data-line-number="637"></td>
        <td id="LC637" class="blob-code blob-code-inner js-file-line">} EVRNotificationError;</td>
      </tr>
      <tr>
        <td id="L638" class="blob-num js-line-number" data-line-number="638"></td>
        <td id="LC638" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L639" class="blob-num js-line-number" data-line-number="639"></td>
        <td id="LC639" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">enum</span> EVRInitError</td>
      </tr>
      <tr>
        <td id="L640" class="blob-num js-line-number" data-line-number="640"></td>
        <td id="LC640" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L641" class="blob-num js-line-number" data-line-number="641"></td>
        <td id="LC641" class="blob-code blob-code-inner js-file-line">	EVRInitError_VRInitError_None = <span class="pl-c1">0</span>,</td>
      </tr>
      <tr>
        <td id="L642" class="blob-num js-line-number" data-line-number="642"></td>
        <td id="LC642" class="blob-code blob-code-inner js-file-line">	EVRInitError_VRInitError_Unknown = <span class="pl-c1">1</span>,</td>
      </tr>
      <tr>
        <td id="L643" class="blob-num js-line-number" data-line-number="643"></td>
        <td id="LC643" class="blob-code blob-code-inner js-file-line">	EVRInitError_VRInitError_Init_InstallationNotFound = <span class="pl-c1">100</span>,</td>
      </tr>
      <tr>
        <td id="L644" class="blob-num js-line-number" data-line-number="644"></td>
        <td id="LC644" class="blob-code blob-code-inner js-file-line">	EVRInitError_VRInitError_Init_InstallationCorrupt = <span class="pl-c1">101</span>,</td>
      </tr>
      <tr>
        <td id="L645" class="blob-num js-line-number" data-line-number="645"></td>
        <td id="LC645" class="blob-code blob-code-inner js-file-line">	EVRInitError_VRInitError_Init_VRClientDLLNotFound = <span class="pl-c1">102</span>,</td>
      </tr>
      <tr>
        <td id="L646" class="blob-num js-line-number" data-line-number="646"></td>
        <td id="LC646" class="blob-code blob-code-inner js-file-line">	EVRInitError_VRInitError_Init_FileNotFound = <span class="pl-c1">103</span>,</td>
      </tr>
      <tr>
        <td id="L647" class="blob-num js-line-number" data-line-number="647"></td>
        <td id="LC647" class="blob-code blob-code-inner js-file-line">	EVRInitError_VRInitError_Init_FactoryNotFound = <span class="pl-c1">104</span>,</td>
      </tr>
      <tr>
        <td id="L648" class="blob-num js-line-number" data-line-number="648"></td>
        <td id="LC648" class="blob-code blob-code-inner js-file-line">	EVRInitError_VRInitError_Init_InterfaceNotFound = <span class="pl-c1">105</span>,</td>
      </tr>
      <tr>
        <td id="L649" class="blob-num js-line-number" data-line-number="649"></td>
        <td id="LC649" class="blob-code blob-code-inner js-file-line">	EVRInitError_VRInitError_Init_InvalidInterface = <span class="pl-c1">106</span>,</td>
      </tr>
      <tr>
        <td id="L650" class="blob-num js-line-number" data-line-number="650"></td>
        <td id="LC650" class="blob-code blob-code-inner js-file-line">	EVRInitError_VRInitError_Init_UserConfigDirectoryInvalid = <span class="pl-c1">107</span>,</td>
      </tr>
      <tr>
        <td id="L651" class="blob-num js-line-number" data-line-number="651"></td>
        <td id="LC651" class="blob-code blob-code-inner js-file-line">	EVRInitError_VRInitError_Init_HmdNotFound = <span class="pl-c1">108</span>,</td>
      </tr>
      <tr>
        <td id="L652" class="blob-num js-line-number" data-line-number="652"></td>
        <td id="LC652" class="blob-code blob-code-inner js-file-line">	EVRInitError_VRInitError_Init_NotInitialized = <span class="pl-c1">109</span>,</td>
      </tr>
      <tr>
        <td id="L653" class="blob-num js-line-number" data-line-number="653"></td>
        <td id="LC653" class="blob-code blob-code-inner js-file-line">	EVRInitError_VRInitError_Init_PathRegistryNotFound = <span class="pl-c1">110</span>,</td>
      </tr>
      <tr>
        <td id="L654" class="blob-num js-line-number" data-line-number="654"></td>
        <td id="LC654" class="blob-code blob-code-inner js-file-line">	EVRInitError_VRInitError_Init_NoConfigPath = <span class="pl-c1">111</span>,</td>
      </tr>
      <tr>
        <td id="L655" class="blob-num js-line-number" data-line-number="655"></td>
        <td id="LC655" class="blob-code blob-code-inner js-file-line">	EVRInitError_VRInitError_Init_NoLogPath = <span class="pl-c1">112</span>,</td>
      </tr>
      <tr>
        <td id="L656" class="blob-num js-line-number" data-line-number="656"></td>
        <td id="LC656" class="blob-code blob-code-inner js-file-line">	EVRInitError_VRInitError_Init_PathRegistryNotWritable = <span class="pl-c1">113</span>,</td>
      </tr>
      <tr>
        <td id="L657" class="blob-num js-line-number" data-line-number="657"></td>
        <td id="LC657" class="blob-code blob-code-inner js-file-line">	EVRInitError_VRInitError_Init_AppInfoInitFailed = <span class="pl-c1">114</span>,</td>
      </tr>
      <tr>
        <td id="L658" class="blob-num js-line-number" data-line-number="658"></td>
        <td id="LC658" class="blob-code blob-code-inner js-file-line">	EVRInitError_VRInitError_Init_Retry = <span class="pl-c1">115</span>,</td>
      </tr>
      <tr>
        <td id="L659" class="blob-num js-line-number" data-line-number="659"></td>
        <td id="LC659" class="blob-code blob-code-inner js-file-line">	EVRInitError_VRInitError_Init_InitCanceledByUser = <span class="pl-c1">116</span>,</td>
      </tr>
      <tr>
        <td id="L660" class="blob-num js-line-number" data-line-number="660"></td>
        <td id="LC660" class="blob-code blob-code-inner js-file-line">	EVRInitError_VRInitError_Init_AnotherAppLaunching = <span class="pl-c1">117</span>,</td>
      </tr>
      <tr>
        <td id="L661" class="blob-num js-line-number" data-line-number="661"></td>
        <td id="LC661" class="blob-code blob-code-inner js-file-line">	EVRInitError_VRInitError_Init_SettingsInitFailed = <span class="pl-c1">118</span>,</td>
      </tr>
      <tr>
        <td id="L662" class="blob-num js-line-number" data-line-number="662"></td>
        <td id="LC662" class="blob-code blob-code-inner js-file-line">	EVRInitError_VRInitError_Init_ShuttingDown = <span class="pl-c1">119</span>,</td>
      </tr>
      <tr>
        <td id="L663" class="blob-num js-line-number" data-line-number="663"></td>
        <td id="LC663" class="blob-code blob-code-inner js-file-line">	EVRInitError_VRInitError_Init_TooManyObjects = <span class="pl-c1">120</span>,</td>
      </tr>
      <tr>
        <td id="L664" class="blob-num js-line-number" data-line-number="664"></td>
        <td id="LC664" class="blob-code blob-code-inner js-file-line">	EVRInitError_VRInitError_Init_NoServerForBackgroundApp = <span class="pl-c1">121</span>,</td>
      </tr>
      <tr>
        <td id="L665" class="blob-num js-line-number" data-line-number="665"></td>
        <td id="LC665" class="blob-code blob-code-inner js-file-line">	EVRInitError_VRInitError_Init_NotSupportedWithCompositor = <span class="pl-c1">122</span>,</td>
      </tr>
      <tr>
        <td id="L666" class="blob-num js-line-number" data-line-number="666"></td>
        <td id="LC666" class="blob-code blob-code-inner js-file-line">	EVRInitError_VRInitError_Init_NotAvailableToUtilityApps = <span class="pl-c1">123</span>,</td>
      </tr>
      <tr>
        <td id="L667" class="blob-num js-line-number" data-line-number="667"></td>
        <td id="LC667" class="blob-code blob-code-inner js-file-line">	EVRInitError_VRInitError_Init_Internal = <span class="pl-c1">124</span>,</td>
      </tr>
      <tr>
        <td id="L668" class="blob-num js-line-number" data-line-number="668"></td>
        <td id="LC668" class="blob-code blob-code-inner js-file-line">	EVRInitError_VRInitError_Init_HmdDriverIdIsNone = <span class="pl-c1">125</span>,</td>
      </tr>
      <tr>
        <td id="L669" class="blob-num js-line-number" data-line-number="669"></td>
        <td id="LC669" class="blob-code blob-code-inner js-file-line">	EVRInitError_VRInitError_Init_HmdNotFoundPresenceFailed = <span class="pl-c1">126</span>,</td>
      </tr>
      <tr>
        <td id="L670" class="blob-num js-line-number" data-line-number="670"></td>
        <td id="LC670" class="blob-code blob-code-inner js-file-line">	EVRInitError_VRInitError_Init_VRMonitorNotFound = <span class="pl-c1">127</span>,</td>
      </tr>
      <tr>
        <td id="L671" class="blob-num js-line-number" data-line-number="671"></td>
        <td id="LC671" class="blob-code blob-code-inner js-file-line">	EVRInitError_VRInitError_Init_VRMonitorStartupFailed = <span class="pl-c1">128</span>,</td>
      </tr>
      <tr>
        <td id="L672" class="blob-num js-line-number" data-line-number="672"></td>
        <td id="LC672" class="blob-code blob-code-inner js-file-line">	EVRInitError_VRInitError_Init_LowPowerWatchdogNotSupported = <span class="pl-c1">129</span>,</td>
      </tr>
      <tr>
        <td id="L673" class="blob-num js-line-number" data-line-number="673"></td>
        <td id="LC673" class="blob-code blob-code-inner js-file-line">	EVRInitError_VRInitError_Init_InvalidApplicationType = <span class="pl-c1">130</span>,</td>
      </tr>
      <tr>
        <td id="L674" class="blob-num js-line-number" data-line-number="674"></td>
        <td id="LC674" class="blob-code blob-code-inner js-file-line">	EVRInitError_VRInitError_Init_NotAvailableToWatchdogApps = <span class="pl-c1">131</span>,</td>
      </tr>
      <tr>
        <td id="L675" class="blob-num js-line-number" data-line-number="675"></td>
        <td id="LC675" class="blob-code blob-code-inner js-file-line">	EVRInitError_VRInitError_Init_WatchdogDisabledInSettings = <span class="pl-c1">132</span>,</td>
      </tr>
      <tr>
        <td id="L676" class="blob-num js-line-number" data-line-number="676"></td>
        <td id="LC676" class="blob-code blob-code-inner js-file-line">	EVRInitError_VRInitError_Init_VRDashboardNotFound = <span class="pl-c1">133</span>,</td>
      </tr>
      <tr>
        <td id="L677" class="blob-num js-line-number" data-line-number="677"></td>
        <td id="LC677" class="blob-code blob-code-inner js-file-line">	EVRInitError_VRInitError_Init_VRDashboardStartupFailed = <span class="pl-c1">134</span>,</td>
      </tr>
      <tr>
        <td id="L678" class="blob-num js-line-number" data-line-number="678"></td>
        <td id="LC678" class="blob-code blob-code-inner js-file-line">	EVRInitError_VRInitError_Driver_Failed = <span class="pl-c1">200</span>,</td>
      </tr>
      <tr>
        <td id="L679" class="blob-num js-line-number" data-line-number="679"></td>
        <td id="LC679" class="blob-code blob-code-inner js-file-line">	EVRInitError_VRInitError_Driver_Unknown = <span class="pl-c1">201</span>,</td>
      </tr>
      <tr>
        <td id="L680" class="blob-num js-line-number" data-line-number="680"></td>
        <td id="LC680" class="blob-code blob-code-inner js-file-line">	EVRInitError_VRInitError_Driver_HmdUnknown = <span class="pl-c1">202</span>,</td>
      </tr>
      <tr>
        <td id="L681" class="blob-num js-line-number" data-line-number="681"></td>
        <td id="LC681" class="blob-code blob-code-inner js-file-line">	EVRInitError_VRInitError_Driver_NotLoaded = <span class="pl-c1">203</span>,</td>
      </tr>
      <tr>
        <td id="L682" class="blob-num js-line-number" data-line-number="682"></td>
        <td id="LC682" class="blob-code blob-code-inner js-file-line">	EVRInitError_VRInitError_Driver_RuntimeOutOfDate = <span class="pl-c1">204</span>,</td>
      </tr>
      <tr>
        <td id="L683" class="blob-num js-line-number" data-line-number="683"></td>
        <td id="LC683" class="blob-code blob-code-inner js-file-line">	EVRInitError_VRInitError_Driver_HmdInUse = <span class="pl-c1">205</span>,</td>
      </tr>
      <tr>
        <td id="L684" class="blob-num js-line-number" data-line-number="684"></td>
        <td id="LC684" class="blob-code blob-code-inner js-file-line">	EVRInitError_VRInitError_Driver_NotCalibrated = <span class="pl-c1">206</span>,</td>
      </tr>
      <tr>
        <td id="L685" class="blob-num js-line-number" data-line-number="685"></td>
        <td id="LC685" class="blob-code blob-code-inner js-file-line">	EVRInitError_VRInitError_Driver_CalibrationInvalid = <span class="pl-c1">207</span>,</td>
      </tr>
      <tr>
        <td id="L686" class="blob-num js-line-number" data-line-number="686"></td>
        <td id="LC686" class="blob-code blob-code-inner js-file-line">	EVRInitError_VRInitError_Driver_HmdDisplayNotFound = <span class="pl-c1">208</span>,</td>
      </tr>
      <tr>
        <td id="L687" class="blob-num js-line-number" data-line-number="687"></td>
        <td id="LC687" class="blob-code blob-code-inner js-file-line">	EVRInitError_VRInitError_Driver_TrackedDeviceInterfaceUnknown = <span class="pl-c1">209</span>,</td>
      </tr>
      <tr>
        <td id="L688" class="blob-num js-line-number" data-line-number="688"></td>
        <td id="LC688" class="blob-code blob-code-inner js-file-line">	EVRInitError_VRInitError_Driver_HmdDriverIdOutOfBounds = <span class="pl-c1">211</span>,</td>
      </tr>
      <tr>
        <td id="L689" class="blob-num js-line-number" data-line-number="689"></td>
        <td id="LC689" class="blob-code blob-code-inner js-file-line">	EVRInitError_VRInitError_Driver_HmdDisplayMirrored = <span class="pl-c1">212</span>,</td>
      </tr>
      <tr>
        <td id="L690" class="blob-num js-line-number" data-line-number="690"></td>
        <td id="LC690" class="blob-code blob-code-inner js-file-line">	EVRInitError_VRInitError_IPC_ServerInitFailed = <span class="pl-c1">300</span>,</td>
      </tr>
      <tr>
        <td id="L691" class="blob-num js-line-number" data-line-number="691"></td>
        <td id="LC691" class="blob-code blob-code-inner js-file-line">	EVRInitError_VRInitError_IPC_ConnectFailed = <span class="pl-c1">301</span>,</td>
      </tr>
      <tr>
        <td id="L692" class="blob-num js-line-number" data-line-number="692"></td>
        <td id="LC692" class="blob-code blob-code-inner js-file-line">	EVRInitError_VRInitError_IPC_SharedStateInitFailed = <span class="pl-c1">302</span>,</td>
      </tr>
      <tr>
        <td id="L693" class="blob-num js-line-number" data-line-number="693"></td>
        <td id="LC693" class="blob-code blob-code-inner js-file-line">	EVRInitError_VRInitError_IPC_CompositorInitFailed = <span class="pl-c1">303</span>,</td>
      </tr>
      <tr>
        <td id="L694" class="blob-num js-line-number" data-line-number="694"></td>
        <td id="LC694" class="blob-code blob-code-inner js-file-line">	EVRInitError_VRInitError_IPC_MutexInitFailed = <span class="pl-c1">304</span>,</td>
      </tr>
      <tr>
        <td id="L695" class="blob-num js-line-number" data-line-number="695"></td>
        <td id="LC695" class="blob-code blob-code-inner js-file-line">	EVRInitError_VRInitError_IPC_Failed = <span class="pl-c1">305</span>,</td>
      </tr>
      <tr>
        <td id="L696" class="blob-num js-line-number" data-line-number="696"></td>
        <td id="LC696" class="blob-code blob-code-inner js-file-line">	EVRInitError_VRInitError_IPC_CompositorConnectFailed = <span class="pl-c1">306</span>,</td>
      </tr>
      <tr>
        <td id="L697" class="blob-num js-line-number" data-line-number="697"></td>
        <td id="LC697" class="blob-code blob-code-inner js-file-line">	EVRInitError_VRInitError_IPC_CompositorInvalidConnectResponse = <span class="pl-c1">307</span>,</td>
      </tr>
      <tr>
        <td id="L698" class="blob-num js-line-number" data-line-number="698"></td>
        <td id="LC698" class="blob-code blob-code-inner js-file-line">	EVRInitError_VRInitError_IPC_ConnectFailedAfterMultipleAttempts = <span class="pl-c1">308</span>,</td>
      </tr>
      <tr>
        <td id="L699" class="blob-num js-line-number" data-line-number="699"></td>
        <td id="LC699" class="blob-code blob-code-inner js-file-line">	EVRInitError_VRInitError_Compositor_Failed = <span class="pl-c1">400</span>,</td>
      </tr>
      <tr>
        <td id="L700" class="blob-num js-line-number" data-line-number="700"></td>
        <td id="LC700" class="blob-code blob-code-inner js-file-line">	EVRInitError_VRInitError_Compositor_D3D11HardwareRequired = <span class="pl-c1">401</span>,</td>
      </tr>
      <tr>
        <td id="L701" class="blob-num js-line-number" data-line-number="701"></td>
        <td id="LC701" class="blob-code blob-code-inner js-file-line">	EVRInitError_VRInitError_Compositor_FirmwareRequiresUpdate = <span class="pl-c1">402</span>,</td>
      </tr>
      <tr>
        <td id="L702" class="blob-num js-line-number" data-line-number="702"></td>
        <td id="LC702" class="blob-code blob-code-inner js-file-line">	EVRInitError_VRInitError_Compositor_OverlayInitFailed = <span class="pl-c1">403</span>,</td>
      </tr>
      <tr>
        <td id="L703" class="blob-num js-line-number" data-line-number="703"></td>
        <td id="LC703" class="blob-code blob-code-inner js-file-line">	EVRInitError_VRInitError_Compositor_ScreenshotsInitFailed = <span class="pl-c1">404</span>,</td>
      </tr>
      <tr>
        <td id="L704" class="blob-num js-line-number" data-line-number="704"></td>
        <td id="LC704" class="blob-code blob-code-inner js-file-line">	EVRInitError_VRInitError_VendorSpecific_UnableToConnectToOculusRuntime = <span class="pl-c1">1000</span>,</td>
      </tr>
      <tr>
        <td id="L705" class="blob-num js-line-number" data-line-number="705"></td>
        <td id="LC705" class="blob-code blob-code-inner js-file-line">	EVRInitError_VRInitError_VendorSpecific_HmdFound_CantOpenDevice = <span class="pl-c1">1101</span>,</td>
      </tr>
      <tr>
        <td id="L706" class="blob-num js-line-number" data-line-number="706"></td>
        <td id="LC706" class="blob-code blob-code-inner js-file-line">	EVRInitError_VRInitError_VendorSpecific_HmdFound_UnableToRequestConfigStart = <span class="pl-c1">1102</span>,</td>
      </tr>
      <tr>
        <td id="L707" class="blob-num js-line-number" data-line-number="707"></td>
        <td id="LC707" class="blob-code blob-code-inner js-file-line">	EVRInitError_VRInitError_VendorSpecific_HmdFound_NoStoredConfig = <span class="pl-c1">1103</span>,</td>
      </tr>
      <tr>
        <td id="L708" class="blob-num js-line-number" data-line-number="708"></td>
        <td id="LC708" class="blob-code blob-code-inner js-file-line">	EVRInitError_VRInitError_VendorSpecific_HmdFound_ConfigTooBig = <span class="pl-c1">1104</span>,</td>
      </tr>
      <tr>
        <td id="L709" class="blob-num js-line-number" data-line-number="709"></td>
        <td id="LC709" class="blob-code blob-code-inner js-file-line">	EVRInitError_VRInitError_VendorSpecific_HmdFound_ConfigTooSmall = <span class="pl-c1">1105</span>,</td>
      </tr>
      <tr>
        <td id="L710" class="blob-num js-line-number" data-line-number="710"></td>
        <td id="LC710" class="blob-code blob-code-inner js-file-line">	EVRInitError_VRInitError_VendorSpecific_HmdFound_UnableToInitZLib = <span class="pl-c1">1106</span>,</td>
      </tr>
      <tr>
        <td id="L711" class="blob-num js-line-number" data-line-number="711"></td>
        <td id="LC711" class="blob-code blob-code-inner js-file-line">	EVRInitError_VRInitError_VendorSpecific_HmdFound_CantReadFirmwareVersion = <span class="pl-c1">1107</span>,</td>
      </tr>
      <tr>
        <td id="L712" class="blob-num js-line-number" data-line-number="712"></td>
        <td id="LC712" class="blob-code blob-code-inner js-file-line">	EVRInitError_VRInitError_VendorSpecific_HmdFound_UnableToSendUserDataStart = <span class="pl-c1">1108</span>,</td>
      </tr>
      <tr>
        <td id="L713" class="blob-num js-line-number" data-line-number="713"></td>
        <td id="LC713" class="blob-code blob-code-inner js-file-line">	EVRInitError_VRInitError_VendorSpecific_HmdFound_UnableToGetUserDataStart = <span class="pl-c1">1109</span>,</td>
      </tr>
      <tr>
        <td id="L714" class="blob-num js-line-number" data-line-number="714"></td>
        <td id="LC714" class="blob-code blob-code-inner js-file-line">	EVRInitError_VRInitError_VendorSpecific_HmdFound_UnableToGetUserDataNext = <span class="pl-c1">1110</span>,</td>
      </tr>
      <tr>
        <td id="L715" class="blob-num js-line-number" data-line-number="715"></td>
        <td id="LC715" class="blob-code blob-code-inner js-file-line">	EVRInitError_VRInitError_VendorSpecific_HmdFound_UserDataAddressRange = <span class="pl-c1">1111</span>,</td>
      </tr>
      <tr>
        <td id="L716" class="blob-num js-line-number" data-line-number="716"></td>
        <td id="LC716" class="blob-code blob-code-inner js-file-line">	EVRInitError_VRInitError_VendorSpecific_HmdFound_UserDataError = <span class="pl-c1">1112</span>,</td>
      </tr>
      <tr>
        <td id="L717" class="blob-num js-line-number" data-line-number="717"></td>
        <td id="LC717" class="blob-code blob-code-inner js-file-line">	EVRInitError_VRInitError_VendorSpecific_HmdFound_ConfigFailedSanityCheck = <span class="pl-c1">1113</span>,</td>
      </tr>
      <tr>
        <td id="L718" class="blob-num js-line-number" data-line-number="718"></td>
        <td id="LC718" class="blob-code blob-code-inner js-file-line">	EVRInitError_VRInitError_Steam_SteamInstallationNotFound = <span class="pl-c1">2000</span>,</td>
      </tr>
      <tr>
        <td id="L719" class="blob-num js-line-number" data-line-number="719"></td>
        <td id="LC719" class="blob-code blob-code-inner js-file-line">} EVRInitError;</td>
      </tr>
      <tr>
        <td id="L720" class="blob-num js-line-number" data-line-number="720"></td>
        <td id="LC720" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L721" class="blob-num js-line-number" data-line-number="721"></td>
        <td id="LC721" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">enum</span> EVRScreenshotType</td>
      </tr>
      <tr>
        <td id="L722" class="blob-num js-line-number" data-line-number="722"></td>
        <td id="LC722" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L723" class="blob-num js-line-number" data-line-number="723"></td>
        <td id="LC723" class="blob-code blob-code-inner js-file-line">	EVRScreenshotType_VRScreenshotType_None = <span class="pl-c1">0</span>,</td>
      </tr>
      <tr>
        <td id="L724" class="blob-num js-line-number" data-line-number="724"></td>
        <td id="LC724" class="blob-code blob-code-inner js-file-line">	EVRScreenshotType_VRScreenshotType_Mono = <span class="pl-c1">1</span>,</td>
      </tr>
      <tr>
        <td id="L725" class="blob-num js-line-number" data-line-number="725"></td>
        <td id="LC725" class="blob-code blob-code-inner js-file-line">	EVRScreenshotType_VRScreenshotType_Stereo = <span class="pl-c1">2</span>,</td>
      </tr>
      <tr>
        <td id="L726" class="blob-num js-line-number" data-line-number="726"></td>
        <td id="LC726" class="blob-code blob-code-inner js-file-line">	EVRScreenshotType_VRScreenshotType_Cubemap = <span class="pl-c1">3</span>,</td>
      </tr>
      <tr>
        <td id="L727" class="blob-num js-line-number" data-line-number="727"></td>
        <td id="LC727" class="blob-code blob-code-inner js-file-line">	EVRScreenshotType_VRScreenshotType_MonoPanorama = <span class="pl-c1">4</span>,</td>
      </tr>
      <tr>
        <td id="L728" class="blob-num js-line-number" data-line-number="728"></td>
        <td id="LC728" class="blob-code blob-code-inner js-file-line">	EVRScreenshotType_VRScreenshotType_StereoPanorama = <span class="pl-c1">5</span>,</td>
      </tr>
      <tr>
        <td id="L729" class="blob-num js-line-number" data-line-number="729"></td>
        <td id="LC729" class="blob-code blob-code-inner js-file-line">} EVRScreenshotType;</td>
      </tr>
      <tr>
        <td id="L730" class="blob-num js-line-number" data-line-number="730"></td>
        <td id="LC730" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L731" class="blob-num js-line-number" data-line-number="731"></td>
        <td id="LC731" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">enum</span> EVRScreenshotPropertyFilenames</td>
      </tr>
      <tr>
        <td id="L732" class="blob-num js-line-number" data-line-number="732"></td>
        <td id="LC732" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L733" class="blob-num js-line-number" data-line-number="733"></td>
        <td id="LC733" class="blob-code blob-code-inner js-file-line">	EVRScreenshotPropertyFilenames_VRScreenshotPropertyFilenames_Preview = <span class="pl-c1">0</span>,</td>
      </tr>
      <tr>
        <td id="L734" class="blob-num js-line-number" data-line-number="734"></td>
        <td id="LC734" class="blob-code blob-code-inner js-file-line">	EVRScreenshotPropertyFilenames_VRScreenshotPropertyFilenames_VR = <span class="pl-c1">1</span>,</td>
      </tr>
      <tr>
        <td id="L735" class="blob-num js-line-number" data-line-number="735"></td>
        <td id="LC735" class="blob-code blob-code-inner js-file-line">} EVRScreenshotPropertyFilenames;</td>
      </tr>
      <tr>
        <td id="L736" class="blob-num js-line-number" data-line-number="736"></td>
        <td id="LC736" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L737" class="blob-num js-line-number" data-line-number="737"></td>
        <td id="LC737" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">enum</span> EVRTrackedCameraError</td>
      </tr>
      <tr>
        <td id="L738" class="blob-num js-line-number" data-line-number="738"></td>
        <td id="LC738" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L739" class="blob-num js-line-number" data-line-number="739"></td>
        <td id="LC739" class="blob-code blob-code-inner js-file-line">	EVRTrackedCameraError_VRTrackedCameraError_None = <span class="pl-c1">0</span>,</td>
      </tr>
      <tr>
        <td id="L740" class="blob-num js-line-number" data-line-number="740"></td>
        <td id="LC740" class="blob-code blob-code-inner js-file-line">	EVRTrackedCameraError_VRTrackedCameraError_OperationFailed = <span class="pl-c1">100</span>,</td>
      </tr>
      <tr>
        <td id="L741" class="blob-num js-line-number" data-line-number="741"></td>
        <td id="LC741" class="blob-code blob-code-inner js-file-line">	EVRTrackedCameraError_VRTrackedCameraError_InvalidHandle = <span class="pl-c1">101</span>,</td>
      </tr>
      <tr>
        <td id="L742" class="blob-num js-line-number" data-line-number="742"></td>
        <td id="LC742" class="blob-code blob-code-inner js-file-line">	EVRTrackedCameraError_VRTrackedCameraError_InvalidFrameHeaderVersion = <span class="pl-c1">102</span>,</td>
      </tr>
      <tr>
        <td id="L743" class="blob-num js-line-number" data-line-number="743"></td>
        <td id="LC743" class="blob-code blob-code-inner js-file-line">	EVRTrackedCameraError_VRTrackedCameraError_OutOfHandles = <span class="pl-c1">103</span>,</td>
      </tr>
      <tr>
        <td id="L744" class="blob-num js-line-number" data-line-number="744"></td>
        <td id="LC744" class="blob-code blob-code-inner js-file-line">	EVRTrackedCameraError_VRTrackedCameraError_IPCFailure = <span class="pl-c1">104</span>,</td>
      </tr>
      <tr>
        <td id="L745" class="blob-num js-line-number" data-line-number="745"></td>
        <td id="LC745" class="blob-code blob-code-inner js-file-line">	EVRTrackedCameraError_VRTrackedCameraError_NotSupportedForThisDevice = <span class="pl-c1">105</span>,</td>
      </tr>
      <tr>
        <td id="L746" class="blob-num js-line-number" data-line-number="746"></td>
        <td id="LC746" class="blob-code blob-code-inner js-file-line">	EVRTrackedCameraError_VRTrackedCameraError_SharedMemoryFailure = <span class="pl-c1">106</span>,</td>
      </tr>
      <tr>
        <td id="L747" class="blob-num js-line-number" data-line-number="747"></td>
        <td id="LC747" class="blob-code blob-code-inner js-file-line">	EVRTrackedCameraError_VRTrackedCameraError_FrameBufferingFailure = <span class="pl-c1">107</span>,</td>
      </tr>
      <tr>
        <td id="L748" class="blob-num js-line-number" data-line-number="748"></td>
        <td id="LC748" class="blob-code blob-code-inner js-file-line">	EVRTrackedCameraError_VRTrackedCameraError_StreamSetupFailure = <span class="pl-c1">108</span>,</td>
      </tr>
      <tr>
        <td id="L749" class="blob-num js-line-number" data-line-number="749"></td>
        <td id="LC749" class="blob-code blob-code-inner js-file-line">	EVRTrackedCameraError_VRTrackedCameraError_InvalidGLTextureId = <span class="pl-c1">109</span>,</td>
      </tr>
      <tr>
        <td id="L750" class="blob-num js-line-number" data-line-number="750"></td>
        <td id="LC750" class="blob-code blob-code-inner js-file-line">	EVRTrackedCameraError_VRTrackedCameraError_InvalidSharedTextureHandle = <span class="pl-c1">110</span>,</td>
      </tr>
      <tr>
        <td id="L751" class="blob-num js-line-number" data-line-number="751"></td>
        <td id="LC751" class="blob-code blob-code-inner js-file-line">	EVRTrackedCameraError_VRTrackedCameraError_FailedToGetGLTextureId = <span class="pl-c1">111</span>,</td>
      </tr>
      <tr>
        <td id="L752" class="blob-num js-line-number" data-line-number="752"></td>
        <td id="LC752" class="blob-code blob-code-inner js-file-line">	EVRTrackedCameraError_VRTrackedCameraError_SharedTextureFailure = <span class="pl-c1">112</span>,</td>
      </tr>
      <tr>
        <td id="L753" class="blob-num js-line-number" data-line-number="753"></td>
        <td id="LC753" class="blob-code blob-code-inner js-file-line">	EVRTrackedCameraError_VRTrackedCameraError_NoFrameAvailable = <span class="pl-c1">113</span>,</td>
      </tr>
      <tr>
        <td id="L754" class="blob-num js-line-number" data-line-number="754"></td>
        <td id="LC754" class="blob-code blob-code-inner js-file-line">	EVRTrackedCameraError_VRTrackedCameraError_InvalidArgument = <span class="pl-c1">114</span>,</td>
      </tr>
      <tr>
        <td id="L755" class="blob-num js-line-number" data-line-number="755"></td>
        <td id="LC755" class="blob-code blob-code-inner js-file-line">	EVRTrackedCameraError_VRTrackedCameraError_InvalidFrameBufferSize = <span class="pl-c1">115</span>,</td>
      </tr>
      <tr>
        <td id="L756" class="blob-num js-line-number" data-line-number="756"></td>
        <td id="LC756" class="blob-code blob-code-inner js-file-line">} EVRTrackedCameraError;</td>
      </tr>
      <tr>
        <td id="L757" class="blob-num js-line-number" data-line-number="757"></td>
        <td id="LC757" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L758" class="blob-num js-line-number" data-line-number="758"></td>
        <td id="LC758" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">enum</span> EVRTrackedCameraFrameType</td>
      </tr>
      <tr>
        <td id="L759" class="blob-num js-line-number" data-line-number="759"></td>
        <td id="LC759" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L760" class="blob-num js-line-number" data-line-number="760"></td>
        <td id="LC760" class="blob-code blob-code-inner js-file-line">	EVRTrackedCameraFrameType_VRTrackedCameraFrameType_Distorted = <span class="pl-c1">0</span>,</td>
      </tr>
      <tr>
        <td id="L761" class="blob-num js-line-number" data-line-number="761"></td>
        <td id="LC761" class="blob-code blob-code-inner js-file-line">	EVRTrackedCameraFrameType_VRTrackedCameraFrameType_Undistorted = <span class="pl-c1">1</span>,</td>
      </tr>
      <tr>
        <td id="L762" class="blob-num js-line-number" data-line-number="762"></td>
        <td id="LC762" class="blob-code blob-code-inner js-file-line">	EVRTrackedCameraFrameType_VRTrackedCameraFrameType_MaximumUndistorted = <span class="pl-c1">2</span>,</td>
      </tr>
      <tr>
        <td id="L763" class="blob-num js-line-number" data-line-number="763"></td>
        <td id="LC763" class="blob-code blob-code-inner js-file-line">	EVRTrackedCameraFrameType_MAX_CAMERA_FRAME_TYPES = <span class="pl-c1">3</span>,</td>
      </tr>
      <tr>
        <td id="L764" class="blob-num js-line-number" data-line-number="764"></td>
        <td id="LC764" class="blob-code blob-code-inner js-file-line">} EVRTrackedCameraFrameType;</td>
      </tr>
      <tr>
        <td id="L765" class="blob-num js-line-number" data-line-number="765"></td>
        <td id="LC765" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L766" class="blob-num js-line-number" data-line-number="766"></td>
        <td id="LC766" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">enum</span> EVRApplicationError</td>
      </tr>
      <tr>
        <td id="L767" class="blob-num js-line-number" data-line-number="767"></td>
        <td id="LC767" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L768" class="blob-num js-line-number" data-line-number="768"></td>
        <td id="LC768" class="blob-code blob-code-inner js-file-line">	EVRApplicationError_VRApplicationError_None = <span class="pl-c1">0</span>,</td>
      </tr>
      <tr>
        <td id="L769" class="blob-num js-line-number" data-line-number="769"></td>
        <td id="LC769" class="blob-code blob-code-inner js-file-line">	EVRApplicationError_VRApplicationError_AppKeyAlreadyExists = <span class="pl-c1">100</span>,</td>
      </tr>
      <tr>
        <td id="L770" class="blob-num js-line-number" data-line-number="770"></td>
        <td id="LC770" class="blob-code blob-code-inner js-file-line">	EVRApplicationError_VRApplicationError_NoManifest = <span class="pl-c1">101</span>,</td>
      </tr>
      <tr>
        <td id="L771" class="blob-num js-line-number" data-line-number="771"></td>
        <td id="LC771" class="blob-code blob-code-inner js-file-line">	EVRApplicationError_VRApplicationError_NoApplication = <span class="pl-c1">102</span>,</td>
      </tr>
      <tr>
        <td id="L772" class="blob-num js-line-number" data-line-number="772"></td>
        <td id="LC772" class="blob-code blob-code-inner js-file-line">	EVRApplicationError_VRApplicationError_InvalidIndex = <span class="pl-c1">103</span>,</td>
      </tr>
      <tr>
        <td id="L773" class="blob-num js-line-number" data-line-number="773"></td>
        <td id="LC773" class="blob-code blob-code-inner js-file-line">	EVRApplicationError_VRApplicationError_UnknownApplication = <span class="pl-c1">104</span>,</td>
      </tr>
      <tr>
        <td id="L774" class="blob-num js-line-number" data-line-number="774"></td>
        <td id="LC774" class="blob-code blob-code-inner js-file-line">	EVRApplicationError_VRApplicationError_IPCFailed = <span class="pl-c1">105</span>,</td>
      </tr>
      <tr>
        <td id="L775" class="blob-num js-line-number" data-line-number="775"></td>
        <td id="LC775" class="blob-code blob-code-inner js-file-line">	EVRApplicationError_VRApplicationError_ApplicationAlreadyRunning = <span class="pl-c1">106</span>,</td>
      </tr>
      <tr>
        <td id="L776" class="blob-num js-line-number" data-line-number="776"></td>
        <td id="LC776" class="blob-code blob-code-inner js-file-line">	EVRApplicationError_VRApplicationError_InvalidManifest = <span class="pl-c1">107</span>,</td>
      </tr>
      <tr>
        <td id="L777" class="blob-num js-line-number" data-line-number="777"></td>
        <td id="LC777" class="blob-code blob-code-inner js-file-line">	EVRApplicationError_VRApplicationError_InvalidApplication = <span class="pl-c1">108</span>,</td>
      </tr>
      <tr>
        <td id="L778" class="blob-num js-line-number" data-line-number="778"></td>
        <td id="LC778" class="blob-code blob-code-inner js-file-line">	EVRApplicationError_VRApplicationError_LaunchFailed = <span class="pl-c1">109</span>,</td>
      </tr>
      <tr>
        <td id="L779" class="blob-num js-line-number" data-line-number="779"></td>
        <td id="LC779" class="blob-code blob-code-inner js-file-line">	EVRApplicationError_VRApplicationError_ApplicationAlreadyStarting = <span class="pl-c1">110</span>,</td>
      </tr>
      <tr>
        <td id="L780" class="blob-num js-line-number" data-line-number="780"></td>
        <td id="LC780" class="blob-code blob-code-inner js-file-line">	EVRApplicationError_VRApplicationError_LaunchInProgress = <span class="pl-c1">111</span>,</td>
      </tr>
      <tr>
        <td id="L781" class="blob-num js-line-number" data-line-number="781"></td>
        <td id="LC781" class="blob-code blob-code-inner js-file-line">	EVRApplicationError_VRApplicationError_OldApplicationQuitting = <span class="pl-c1">112</span>,</td>
      </tr>
      <tr>
        <td id="L782" class="blob-num js-line-number" data-line-number="782"></td>
        <td id="LC782" class="blob-code blob-code-inner js-file-line">	EVRApplicationError_VRApplicationError_TransitionAborted = <span class="pl-c1">113</span>,</td>
      </tr>
      <tr>
        <td id="L783" class="blob-num js-line-number" data-line-number="783"></td>
        <td id="LC783" class="blob-code blob-code-inner js-file-line">	EVRApplicationError_VRApplicationError_IsTemplate = <span class="pl-c1">114</span>,</td>
      </tr>
      <tr>
        <td id="L784" class="blob-num js-line-number" data-line-number="784"></td>
        <td id="LC784" class="blob-code blob-code-inner js-file-line">	EVRApplicationError_VRApplicationError_BufferTooSmall = <span class="pl-c1">200</span>,</td>
      </tr>
      <tr>
        <td id="L785" class="blob-num js-line-number" data-line-number="785"></td>
        <td id="LC785" class="blob-code blob-code-inner js-file-line">	EVRApplicationError_VRApplicationError_PropertyNotSet = <span class="pl-c1">201</span>,</td>
      </tr>
      <tr>
        <td id="L786" class="blob-num js-line-number" data-line-number="786"></td>
        <td id="LC786" class="blob-code blob-code-inner js-file-line">	EVRApplicationError_VRApplicationError_UnknownProperty = <span class="pl-c1">202</span>,</td>
      </tr>
      <tr>
        <td id="L787" class="blob-num js-line-number" data-line-number="787"></td>
        <td id="LC787" class="blob-code blob-code-inner js-file-line">	EVRApplicationError_VRApplicationError_InvalidParameter = <span class="pl-c1">203</span>,</td>
      </tr>
      <tr>
        <td id="L788" class="blob-num js-line-number" data-line-number="788"></td>
        <td id="LC788" class="blob-code blob-code-inner js-file-line">} EVRApplicationError;</td>
      </tr>
      <tr>
        <td id="L789" class="blob-num js-line-number" data-line-number="789"></td>
        <td id="LC789" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L790" class="blob-num js-line-number" data-line-number="790"></td>
        <td id="LC790" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">enum</span> EVRApplicationProperty</td>
      </tr>
      <tr>
        <td id="L791" class="blob-num js-line-number" data-line-number="791"></td>
        <td id="LC791" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L792" class="blob-num js-line-number" data-line-number="792"></td>
        <td id="LC792" class="blob-code blob-code-inner js-file-line">	EVRApplicationProperty_VRApplicationProperty_Name_String = <span class="pl-c1">0</span>,</td>
      </tr>
      <tr>
        <td id="L793" class="blob-num js-line-number" data-line-number="793"></td>
        <td id="LC793" class="blob-code blob-code-inner js-file-line">	EVRApplicationProperty_VRApplicationProperty_LaunchType_String = <span class="pl-c1">11</span>,</td>
      </tr>
      <tr>
        <td id="L794" class="blob-num js-line-number" data-line-number="794"></td>
        <td id="LC794" class="blob-code blob-code-inner js-file-line">	EVRApplicationProperty_VRApplicationProperty_WorkingDirectory_String = <span class="pl-c1">12</span>,</td>
      </tr>
      <tr>
        <td id="L795" class="blob-num js-line-number" data-line-number="795"></td>
        <td id="LC795" class="blob-code blob-code-inner js-file-line">	EVRApplicationProperty_VRApplicationProperty_BinaryPath_String = <span class="pl-c1">13</span>,</td>
      </tr>
      <tr>
        <td id="L796" class="blob-num js-line-number" data-line-number="796"></td>
        <td id="LC796" class="blob-code blob-code-inner js-file-line">	EVRApplicationProperty_VRApplicationProperty_Arguments_String = <span class="pl-c1">14</span>,</td>
      </tr>
      <tr>
        <td id="L797" class="blob-num js-line-number" data-line-number="797"></td>
        <td id="LC797" class="blob-code blob-code-inner js-file-line">	EVRApplicationProperty_VRApplicationProperty_URL_String = <span class="pl-c1">15</span>,</td>
      </tr>
      <tr>
        <td id="L798" class="blob-num js-line-number" data-line-number="798"></td>
        <td id="LC798" class="blob-code blob-code-inner js-file-line">	EVRApplicationProperty_VRApplicationProperty_Description_String = <span class="pl-c1">50</span>,</td>
      </tr>
      <tr>
        <td id="L799" class="blob-num js-line-number" data-line-number="799"></td>
        <td id="LC799" class="blob-code blob-code-inner js-file-line">	EVRApplicationProperty_VRApplicationProperty_NewsURL_String = <span class="pl-c1">51</span>,</td>
      </tr>
      <tr>
        <td id="L800" class="blob-num js-line-number" data-line-number="800"></td>
        <td id="LC800" class="blob-code blob-code-inner js-file-line">	EVRApplicationProperty_VRApplicationProperty_ImagePath_String = <span class="pl-c1">52</span>,</td>
      </tr>
      <tr>
        <td id="L801" class="blob-num js-line-number" data-line-number="801"></td>
        <td id="LC801" class="blob-code blob-code-inner js-file-line">	EVRApplicationProperty_VRApplicationProperty_Source_String = <span class="pl-c1">53</span>,</td>
      </tr>
      <tr>
        <td id="L802" class="blob-num js-line-number" data-line-number="802"></td>
        <td id="LC802" class="blob-code blob-code-inner js-file-line">	EVRApplicationProperty_VRApplicationProperty_IsDashboardOverlay_Bool = <span class="pl-c1">60</span>,</td>
      </tr>
      <tr>
        <td id="L803" class="blob-num js-line-number" data-line-number="803"></td>
        <td id="LC803" class="blob-code blob-code-inner js-file-line">	EVRApplicationProperty_VRApplicationProperty_IsTemplate_Bool = <span class="pl-c1">61</span>,</td>
      </tr>
      <tr>
        <td id="L804" class="blob-num js-line-number" data-line-number="804"></td>
        <td id="LC804" class="blob-code blob-code-inner js-file-line">	EVRApplicationProperty_VRApplicationProperty_IsInstanced_Bool = <span class="pl-c1">62</span>,</td>
      </tr>
      <tr>
        <td id="L805" class="blob-num js-line-number" data-line-number="805"></td>
        <td id="LC805" class="blob-code blob-code-inner js-file-line">	EVRApplicationProperty_VRApplicationProperty_LastLaunchTime_Uint64 = <span class="pl-c1">70</span>,</td>
      </tr>
      <tr>
        <td id="L806" class="blob-num js-line-number" data-line-number="806"></td>
        <td id="LC806" class="blob-code blob-code-inner js-file-line">} EVRApplicationProperty;</td>
      </tr>
      <tr>
        <td id="L807" class="blob-num js-line-number" data-line-number="807"></td>
        <td id="LC807" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L808" class="blob-num js-line-number" data-line-number="808"></td>
        <td id="LC808" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">enum</span> EVRApplicationTransitionState</td>
      </tr>
      <tr>
        <td id="L809" class="blob-num js-line-number" data-line-number="809"></td>
        <td id="LC809" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L810" class="blob-num js-line-number" data-line-number="810"></td>
        <td id="LC810" class="blob-code blob-code-inner js-file-line">	EVRApplicationTransitionState_VRApplicationTransition_None = <span class="pl-c1">0</span>,</td>
      </tr>
      <tr>
        <td id="L811" class="blob-num js-line-number" data-line-number="811"></td>
        <td id="LC811" class="blob-code blob-code-inner js-file-line">	EVRApplicationTransitionState_VRApplicationTransition_OldAppQuitSent = <span class="pl-c1">10</span>,</td>
      </tr>
      <tr>
        <td id="L812" class="blob-num js-line-number" data-line-number="812"></td>
        <td id="LC812" class="blob-code blob-code-inner js-file-line">	EVRApplicationTransitionState_VRApplicationTransition_WaitingForExternalLaunch = <span class="pl-c1">11</span>,</td>
      </tr>
      <tr>
        <td id="L813" class="blob-num js-line-number" data-line-number="813"></td>
        <td id="LC813" class="blob-code blob-code-inner js-file-line">	EVRApplicationTransitionState_VRApplicationTransition_NewAppLaunched = <span class="pl-c1">20</span>,</td>
      </tr>
      <tr>
        <td id="L814" class="blob-num js-line-number" data-line-number="814"></td>
        <td id="LC814" class="blob-code blob-code-inner js-file-line">} EVRApplicationTransitionState;</td>
      </tr>
      <tr>
        <td id="L815" class="blob-num js-line-number" data-line-number="815"></td>
        <td id="LC815" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L816" class="blob-num js-line-number" data-line-number="816"></td>
        <td id="LC816" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">enum</span> ChaperoneCalibrationState</td>
      </tr>
      <tr>
        <td id="L817" class="blob-num js-line-number" data-line-number="817"></td>
        <td id="LC817" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L818" class="blob-num js-line-number" data-line-number="818"></td>
        <td id="LC818" class="blob-code blob-code-inner js-file-line">	ChaperoneCalibrationState_OK = <span class="pl-c1">1</span>,</td>
      </tr>
      <tr>
        <td id="L819" class="blob-num js-line-number" data-line-number="819"></td>
        <td id="LC819" class="blob-code blob-code-inner js-file-line">	ChaperoneCalibrationState_Warning = <span class="pl-c1">100</span>,</td>
      </tr>
      <tr>
        <td id="L820" class="blob-num js-line-number" data-line-number="820"></td>
        <td id="LC820" class="blob-code blob-code-inner js-file-line">	ChaperoneCalibrationState_Warning_BaseStationMayHaveMoved = <span class="pl-c1">101</span>,</td>
      </tr>
      <tr>
        <td id="L821" class="blob-num js-line-number" data-line-number="821"></td>
        <td id="LC821" class="blob-code blob-code-inner js-file-line">	ChaperoneCalibrationState_Warning_BaseStationRemoved = <span class="pl-c1">102</span>,</td>
      </tr>
      <tr>
        <td id="L822" class="blob-num js-line-number" data-line-number="822"></td>
        <td id="LC822" class="blob-code blob-code-inner js-file-line">	ChaperoneCalibrationState_Warning_SeatedBoundsInvalid = <span class="pl-c1">103</span>,</td>
      </tr>
      <tr>
        <td id="L823" class="blob-num js-line-number" data-line-number="823"></td>
        <td id="LC823" class="blob-code blob-code-inner js-file-line">	ChaperoneCalibrationState_Error = <span class="pl-c1">200</span>,</td>
      </tr>
      <tr>
        <td id="L824" class="blob-num js-line-number" data-line-number="824"></td>
        <td id="LC824" class="blob-code blob-code-inner js-file-line">	ChaperoneCalibrationState_Error_BaseStationUninitialized = <span class="pl-c1">201</span>,</td>
      </tr>
      <tr>
        <td id="L825" class="blob-num js-line-number" data-line-number="825"></td>
        <td id="LC825" class="blob-code blob-code-inner js-file-line">	ChaperoneCalibrationState_Error_BaseStationConflict = <span class="pl-c1">202</span>,</td>
      </tr>
      <tr>
        <td id="L826" class="blob-num js-line-number" data-line-number="826"></td>
        <td id="LC826" class="blob-code blob-code-inner js-file-line">	ChaperoneCalibrationState_Error_PlayAreaInvalid = <span class="pl-c1">203</span>,</td>
      </tr>
      <tr>
        <td id="L827" class="blob-num js-line-number" data-line-number="827"></td>
        <td id="LC827" class="blob-code blob-code-inner js-file-line">	ChaperoneCalibrationState_Error_CollisionBoundsInvalid = <span class="pl-c1">204</span>,</td>
      </tr>
      <tr>
        <td id="L828" class="blob-num js-line-number" data-line-number="828"></td>
        <td id="LC828" class="blob-code blob-code-inner js-file-line">} ChaperoneCalibrationState;</td>
      </tr>
      <tr>
        <td id="L829" class="blob-num js-line-number" data-line-number="829"></td>
        <td id="LC829" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L830" class="blob-num js-line-number" data-line-number="830"></td>
        <td id="LC830" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">enum</span> EChaperoneConfigFile</td>
      </tr>
      <tr>
        <td id="L831" class="blob-num js-line-number" data-line-number="831"></td>
        <td id="LC831" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L832" class="blob-num js-line-number" data-line-number="832"></td>
        <td id="LC832" class="blob-code blob-code-inner js-file-line">	EChaperoneConfigFile_Live = <span class="pl-c1">1</span>,</td>
      </tr>
      <tr>
        <td id="L833" class="blob-num js-line-number" data-line-number="833"></td>
        <td id="LC833" class="blob-code blob-code-inner js-file-line">	EChaperoneConfigFile_Temp = <span class="pl-c1">2</span>,</td>
      </tr>
      <tr>
        <td id="L834" class="blob-num js-line-number" data-line-number="834"></td>
        <td id="LC834" class="blob-code blob-code-inner js-file-line">} EChaperoneConfigFile;</td>
      </tr>
      <tr>
        <td id="L835" class="blob-num js-line-number" data-line-number="835"></td>
        <td id="LC835" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L836" class="blob-num js-line-number" data-line-number="836"></td>
        <td id="LC836" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">enum</span> EChaperoneImportFlags</td>
      </tr>
      <tr>
        <td id="L837" class="blob-num js-line-number" data-line-number="837"></td>
        <td id="LC837" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L838" class="blob-num js-line-number" data-line-number="838"></td>
        <td id="LC838" class="blob-code blob-code-inner js-file-line">	EChaperoneImportFlags_EChaperoneImport_BoundsOnly = <span class="pl-c1">1</span>,</td>
      </tr>
      <tr>
        <td id="L839" class="blob-num js-line-number" data-line-number="839"></td>
        <td id="LC839" class="blob-code blob-code-inner js-file-line">} EChaperoneImportFlags;</td>
      </tr>
      <tr>
        <td id="L840" class="blob-num js-line-number" data-line-number="840"></td>
        <td id="LC840" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L841" class="blob-num js-line-number" data-line-number="841"></td>
        <td id="LC841" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">enum</span> EVRCompositorError</td>
      </tr>
      <tr>
        <td id="L842" class="blob-num js-line-number" data-line-number="842"></td>
        <td id="LC842" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L843" class="blob-num js-line-number" data-line-number="843"></td>
        <td id="LC843" class="blob-code blob-code-inner js-file-line">	EVRCompositorError_VRCompositorError_None = <span class="pl-c1">0</span>,</td>
      </tr>
      <tr>
        <td id="L844" class="blob-num js-line-number" data-line-number="844"></td>
        <td id="LC844" class="blob-code blob-code-inner js-file-line">	EVRCompositorError_VRCompositorError_RequestFailed = <span class="pl-c1">1</span>,</td>
      </tr>
      <tr>
        <td id="L845" class="blob-num js-line-number" data-line-number="845"></td>
        <td id="LC845" class="blob-code blob-code-inner js-file-line">	EVRCompositorError_VRCompositorError_IncompatibleVersion = <span class="pl-c1">100</span>,</td>
      </tr>
      <tr>
        <td id="L846" class="blob-num js-line-number" data-line-number="846"></td>
        <td id="LC846" class="blob-code blob-code-inner js-file-line">	EVRCompositorError_VRCompositorError_DoNotHaveFocus = <span class="pl-c1">101</span>,</td>
      </tr>
      <tr>
        <td id="L847" class="blob-num js-line-number" data-line-number="847"></td>
        <td id="LC847" class="blob-code blob-code-inner js-file-line">	EVRCompositorError_VRCompositorError_InvalidTexture = <span class="pl-c1">102</span>,</td>
      </tr>
      <tr>
        <td id="L848" class="blob-num js-line-number" data-line-number="848"></td>
        <td id="LC848" class="blob-code blob-code-inner js-file-line">	EVRCompositorError_VRCompositorError_IsNotSceneApplication = <span class="pl-c1">103</span>,</td>
      </tr>
      <tr>
        <td id="L849" class="blob-num js-line-number" data-line-number="849"></td>
        <td id="LC849" class="blob-code blob-code-inner js-file-line">	EVRCompositorError_VRCompositorError_TextureIsOnWrongDevice = <span class="pl-c1">104</span>,</td>
      </tr>
      <tr>
        <td id="L850" class="blob-num js-line-number" data-line-number="850"></td>
        <td id="LC850" class="blob-code blob-code-inner js-file-line">	EVRCompositorError_VRCompositorError_TextureUsesUnsupportedFormat = <span class="pl-c1">105</span>,</td>
      </tr>
      <tr>
        <td id="L851" class="blob-num js-line-number" data-line-number="851"></td>
        <td id="LC851" class="blob-code blob-code-inner js-file-line">	EVRCompositorError_VRCompositorError_SharedTexturesNotSupported = <span class="pl-c1">106</span>,</td>
      </tr>
      <tr>
        <td id="L852" class="blob-num js-line-number" data-line-number="852"></td>
        <td id="LC852" class="blob-code blob-code-inner js-file-line">	EVRCompositorError_VRCompositorError_IndexOutOfRange = <span class="pl-c1">107</span>,</td>
      </tr>
      <tr>
        <td id="L853" class="blob-num js-line-number" data-line-number="853"></td>
        <td id="LC853" class="blob-code blob-code-inner js-file-line">	EVRCompositorError_VRCompositorError_AlreadySubmitted = <span class="pl-c1">108</span>,</td>
      </tr>
      <tr>
        <td id="L854" class="blob-num js-line-number" data-line-number="854"></td>
        <td id="LC854" class="blob-code blob-code-inner js-file-line">} EVRCompositorError;</td>
      </tr>
      <tr>
        <td id="L855" class="blob-num js-line-number" data-line-number="855"></td>
        <td id="LC855" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L856" class="blob-num js-line-number" data-line-number="856"></td>
        <td id="LC856" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">enum</span> VROverlayInputMethod</td>
      </tr>
      <tr>
        <td id="L857" class="blob-num js-line-number" data-line-number="857"></td>
        <td id="LC857" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L858" class="blob-num js-line-number" data-line-number="858"></td>
        <td id="LC858" class="blob-code blob-code-inner js-file-line">	VROverlayInputMethod_None = <span class="pl-c1">0</span>,</td>
      </tr>
      <tr>
        <td id="L859" class="blob-num js-line-number" data-line-number="859"></td>
        <td id="LC859" class="blob-code blob-code-inner js-file-line">	VROverlayInputMethod_Mouse = <span class="pl-c1">1</span>,</td>
      </tr>
      <tr>
        <td id="L860" class="blob-num js-line-number" data-line-number="860"></td>
        <td id="LC860" class="blob-code blob-code-inner js-file-line">} VROverlayInputMethod;</td>
      </tr>
      <tr>
        <td id="L861" class="blob-num js-line-number" data-line-number="861"></td>
        <td id="LC861" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L862" class="blob-num js-line-number" data-line-number="862"></td>
        <td id="LC862" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">enum</span> VROverlayTransformType</td>
      </tr>
      <tr>
        <td id="L863" class="blob-num js-line-number" data-line-number="863"></td>
        <td id="LC863" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L864" class="blob-num js-line-number" data-line-number="864"></td>
        <td id="LC864" class="blob-code blob-code-inner js-file-line">	VROverlayTransformType_VROverlayTransform_Absolute = <span class="pl-c1">0</span>,</td>
      </tr>
      <tr>
        <td id="L865" class="blob-num js-line-number" data-line-number="865"></td>
        <td id="LC865" class="blob-code blob-code-inner js-file-line">	VROverlayTransformType_VROverlayTransform_TrackedDeviceRelative = <span class="pl-c1">1</span>,</td>
      </tr>
      <tr>
        <td id="L866" class="blob-num js-line-number" data-line-number="866"></td>
        <td id="LC866" class="blob-code blob-code-inner js-file-line">	VROverlayTransformType_VROverlayTransform_SystemOverlay = <span class="pl-c1">2</span>,</td>
      </tr>
      <tr>
        <td id="L867" class="blob-num js-line-number" data-line-number="867"></td>
        <td id="LC867" class="blob-code blob-code-inner js-file-line">	VROverlayTransformType_VROverlayTransform_TrackedComponent = <span class="pl-c1">3</span>,</td>
      </tr>
      <tr>
        <td id="L868" class="blob-num js-line-number" data-line-number="868"></td>
        <td id="LC868" class="blob-code blob-code-inner js-file-line">} VROverlayTransformType;</td>
      </tr>
      <tr>
        <td id="L869" class="blob-num js-line-number" data-line-number="869"></td>
        <td id="LC869" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L870" class="blob-num js-line-number" data-line-number="870"></td>
        <td id="LC870" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">enum</span> VROverlayFlags</td>
      </tr>
      <tr>
        <td id="L871" class="blob-num js-line-number" data-line-number="871"></td>
        <td id="LC871" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L872" class="blob-num js-line-number" data-line-number="872"></td>
        <td id="LC872" class="blob-code blob-code-inner js-file-line">	VROverlayFlags_None = <span class="pl-c1">0</span>,</td>
      </tr>
      <tr>
        <td id="L873" class="blob-num js-line-number" data-line-number="873"></td>
        <td id="LC873" class="blob-code blob-code-inner js-file-line">	VROverlayFlags_Curved = <span class="pl-c1">1</span>,</td>
      </tr>
      <tr>
        <td id="L874" class="blob-num js-line-number" data-line-number="874"></td>
        <td id="LC874" class="blob-code blob-code-inner js-file-line">	VROverlayFlags_RGSS4X = <span class="pl-c1">2</span>,</td>
      </tr>
      <tr>
        <td id="L875" class="blob-num js-line-number" data-line-number="875"></td>
        <td id="LC875" class="blob-code blob-code-inner js-file-line">	VROverlayFlags_NoDashboardTab = <span class="pl-c1">3</span>,</td>
      </tr>
      <tr>
        <td id="L876" class="blob-num js-line-number" data-line-number="876"></td>
        <td id="LC876" class="blob-code blob-code-inner js-file-line">	VROverlayFlags_AcceptsGamepadEvents = <span class="pl-c1">4</span>,</td>
      </tr>
      <tr>
        <td id="L877" class="blob-num js-line-number" data-line-number="877"></td>
        <td id="LC877" class="blob-code blob-code-inner js-file-line">	VROverlayFlags_ShowGamepadFocus = <span class="pl-c1">5</span>,</td>
      </tr>
      <tr>
        <td id="L878" class="blob-num js-line-number" data-line-number="878"></td>
        <td id="LC878" class="blob-code blob-code-inner js-file-line">	VROverlayFlags_SendVRScrollEvents = <span class="pl-c1">6</span>,</td>
      </tr>
      <tr>
        <td id="L879" class="blob-num js-line-number" data-line-number="879"></td>
        <td id="LC879" class="blob-code blob-code-inner js-file-line">	VROverlayFlags_SendVRTouchpadEvents = <span class="pl-c1">7</span>,</td>
      </tr>
      <tr>
        <td id="L880" class="blob-num js-line-number" data-line-number="880"></td>
        <td id="LC880" class="blob-code blob-code-inner js-file-line">	VROverlayFlags_ShowTouchPadScrollWheel = <span class="pl-c1">8</span>,</td>
      </tr>
      <tr>
        <td id="L881" class="blob-num js-line-number" data-line-number="881"></td>
        <td id="LC881" class="blob-code blob-code-inner js-file-line">	VROverlayFlags_TransferOwnershipToInternalProcess = <span class="pl-c1">9</span>,</td>
      </tr>
      <tr>
        <td id="L882" class="blob-num js-line-number" data-line-number="882"></td>
        <td id="LC882" class="blob-code blob-code-inner js-file-line">	VROverlayFlags_SideBySide_Parallel = <span class="pl-c1">10</span>,</td>
      </tr>
      <tr>
        <td id="L883" class="blob-num js-line-number" data-line-number="883"></td>
        <td id="LC883" class="blob-code blob-code-inner js-file-line">	VROverlayFlags_SideBySide_Crossed = <span class="pl-c1">11</span>,</td>
      </tr>
      <tr>
        <td id="L884" class="blob-num js-line-number" data-line-number="884"></td>
        <td id="LC884" class="blob-code blob-code-inner js-file-line">	VROverlayFlags_Panorama = <span class="pl-c1">12</span>,</td>
      </tr>
      <tr>
        <td id="L885" class="blob-num js-line-number" data-line-number="885"></td>
        <td id="LC885" class="blob-code blob-code-inner js-file-line">	VROverlayFlags_StereoPanorama = <span class="pl-c1">13</span>,</td>
      </tr>
      <tr>
        <td id="L886" class="blob-num js-line-number" data-line-number="886"></td>
        <td id="LC886" class="blob-code blob-code-inner js-file-line">	VROverlayFlags_SortWithNonSceneOverlays = <span class="pl-c1">14</span>,</td>
      </tr>
      <tr>
        <td id="L887" class="blob-num js-line-number" data-line-number="887"></td>
        <td id="LC887" class="blob-code blob-code-inner js-file-line">	VROverlayFlags_VisibleInDashboard = <span class="pl-c1">15</span>,</td>
      </tr>
      <tr>
        <td id="L888" class="blob-num js-line-number" data-line-number="888"></td>
        <td id="LC888" class="blob-code blob-code-inner js-file-line">} VROverlayFlags;</td>
      </tr>
      <tr>
        <td id="L889" class="blob-num js-line-number" data-line-number="889"></td>
        <td id="LC889" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L890" class="blob-num js-line-number" data-line-number="890"></td>
        <td id="LC890" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">enum</span> VRMessageOverlayResponse</td>
      </tr>
      <tr>
        <td id="L891" class="blob-num js-line-number" data-line-number="891"></td>
        <td id="LC891" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L892" class="blob-num js-line-number" data-line-number="892"></td>
        <td id="LC892" class="blob-code blob-code-inner js-file-line">	VRMessageOverlayResponse_ButtonPress_0 = <span class="pl-c1">0</span>,</td>
      </tr>
      <tr>
        <td id="L893" class="blob-num js-line-number" data-line-number="893"></td>
        <td id="LC893" class="blob-code blob-code-inner js-file-line">	VRMessageOverlayResponse_ButtonPress_1 = <span class="pl-c1">1</span>,</td>
      </tr>
      <tr>
        <td id="L894" class="blob-num js-line-number" data-line-number="894"></td>
        <td id="LC894" class="blob-code blob-code-inner js-file-line">	VRMessageOverlayResponse_ButtonPress_2 = <span class="pl-c1">2</span>,</td>
      </tr>
      <tr>
        <td id="L895" class="blob-num js-line-number" data-line-number="895"></td>
        <td id="LC895" class="blob-code blob-code-inner js-file-line">	VRMessageOverlayResponse_ButtonPress_3 = <span class="pl-c1">3</span>,</td>
      </tr>
      <tr>
        <td id="L896" class="blob-num js-line-number" data-line-number="896"></td>
        <td id="LC896" class="blob-code blob-code-inner js-file-line">	VRMessageOverlayResponse_CouldntFindSystemOverlay = <span class="pl-c1">4</span>,</td>
      </tr>
      <tr>
        <td id="L897" class="blob-num js-line-number" data-line-number="897"></td>
        <td id="LC897" class="blob-code blob-code-inner js-file-line">	VRMessageOverlayResponse_CouldntFindOrCreateClientOverlay = <span class="pl-c1">5</span>,</td>
      </tr>
      <tr>
        <td id="L898" class="blob-num js-line-number" data-line-number="898"></td>
        <td id="LC898" class="blob-code blob-code-inner js-file-line">	VRMessageOverlayResponse_ApplicationQuit = <span class="pl-c1">6</span>,</td>
      </tr>
      <tr>
        <td id="L899" class="blob-num js-line-number" data-line-number="899"></td>
        <td id="LC899" class="blob-code blob-code-inner js-file-line">} VRMessageOverlayResponse;</td>
      </tr>
      <tr>
        <td id="L900" class="blob-num js-line-number" data-line-number="900"></td>
        <td id="LC900" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L901" class="blob-num js-line-number" data-line-number="901"></td>
        <td id="LC901" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">enum</span> EGamepadTextInputMode</td>
      </tr>
      <tr>
        <td id="L902" class="blob-num js-line-number" data-line-number="902"></td>
        <td id="LC902" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L903" class="blob-num js-line-number" data-line-number="903"></td>
        <td id="LC903" class="blob-code blob-code-inner js-file-line">	EGamepadTextInputMode_k_EGamepadTextInputModeNormal = <span class="pl-c1">0</span>,</td>
      </tr>
      <tr>
        <td id="L904" class="blob-num js-line-number" data-line-number="904"></td>
        <td id="LC904" class="blob-code blob-code-inner js-file-line">	EGamepadTextInputMode_k_EGamepadTextInputModePassword = <span class="pl-c1">1</span>,</td>
      </tr>
      <tr>
        <td id="L905" class="blob-num js-line-number" data-line-number="905"></td>
        <td id="LC905" class="blob-code blob-code-inner js-file-line">	EGamepadTextInputMode_k_EGamepadTextInputModeSubmit = <span class="pl-c1">2</span>,</td>
      </tr>
      <tr>
        <td id="L906" class="blob-num js-line-number" data-line-number="906"></td>
        <td id="LC906" class="blob-code blob-code-inner js-file-line">} EGamepadTextInputMode;</td>
      </tr>
      <tr>
        <td id="L907" class="blob-num js-line-number" data-line-number="907"></td>
        <td id="LC907" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L908" class="blob-num js-line-number" data-line-number="908"></td>
        <td id="LC908" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">enum</span> EGamepadTextInputLineMode</td>
      </tr>
      <tr>
        <td id="L909" class="blob-num js-line-number" data-line-number="909"></td>
        <td id="LC909" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L910" class="blob-num js-line-number" data-line-number="910"></td>
        <td id="LC910" class="blob-code blob-code-inner js-file-line">	EGamepadTextInputLineMode_k_EGamepadTextInputLineModeSingleLine = <span class="pl-c1">0</span>,</td>
      </tr>
      <tr>
        <td id="L911" class="blob-num js-line-number" data-line-number="911"></td>
        <td id="LC911" class="blob-code blob-code-inner js-file-line">	EGamepadTextInputLineMode_k_EGamepadTextInputLineModeMultipleLines = <span class="pl-c1">1</span>,</td>
      </tr>
      <tr>
        <td id="L912" class="blob-num js-line-number" data-line-number="912"></td>
        <td id="LC912" class="blob-code blob-code-inner js-file-line">} EGamepadTextInputLineMode;</td>
      </tr>
      <tr>
        <td id="L913" class="blob-num js-line-number" data-line-number="913"></td>
        <td id="LC913" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L914" class="blob-num js-line-number" data-line-number="914"></td>
        <td id="LC914" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">enum</span> EOverlayDirection</td>
      </tr>
      <tr>
        <td id="L915" class="blob-num js-line-number" data-line-number="915"></td>
        <td id="LC915" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L916" class="blob-num js-line-number" data-line-number="916"></td>
        <td id="LC916" class="blob-code blob-code-inner js-file-line">	EOverlayDirection_OverlayDirection_Up = <span class="pl-c1">0</span>,</td>
      </tr>
      <tr>
        <td id="L917" class="blob-num js-line-number" data-line-number="917"></td>
        <td id="LC917" class="blob-code blob-code-inner js-file-line">	EOverlayDirection_OverlayDirection_Down = <span class="pl-c1">1</span>,</td>
      </tr>
      <tr>
        <td id="L918" class="blob-num js-line-number" data-line-number="918"></td>
        <td id="LC918" class="blob-code blob-code-inner js-file-line">	EOverlayDirection_OverlayDirection_Left = <span class="pl-c1">2</span>,</td>
      </tr>
      <tr>
        <td id="L919" class="blob-num js-line-number" data-line-number="919"></td>
        <td id="LC919" class="blob-code blob-code-inner js-file-line">	EOverlayDirection_OverlayDirection_Right = <span class="pl-c1">3</span>,</td>
      </tr>
      <tr>
        <td id="L920" class="blob-num js-line-number" data-line-number="920"></td>
        <td id="LC920" class="blob-code blob-code-inner js-file-line">	EOverlayDirection_OverlayDirection_Count = <span class="pl-c1">4</span>,</td>
      </tr>
      <tr>
        <td id="L921" class="blob-num js-line-number" data-line-number="921"></td>
        <td id="LC921" class="blob-code blob-code-inner js-file-line">} EOverlayDirection;</td>
      </tr>
      <tr>
        <td id="L922" class="blob-num js-line-number" data-line-number="922"></td>
        <td id="LC922" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L923" class="blob-num js-line-number" data-line-number="923"></td>
        <td id="LC923" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">enum</span> EVROverlayIntersectionMaskPrimitiveType</td>
      </tr>
      <tr>
        <td id="L924" class="blob-num js-line-number" data-line-number="924"></td>
        <td id="LC924" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L925" class="blob-num js-line-number" data-line-number="925"></td>
        <td id="LC925" class="blob-code blob-code-inner js-file-line">	EVROverlayIntersectionMaskPrimitiveType_OverlayIntersectionPrimitiveType_Rectangle = <span class="pl-c1">0</span>,</td>
      </tr>
      <tr>
        <td id="L926" class="blob-num js-line-number" data-line-number="926"></td>
        <td id="LC926" class="blob-code blob-code-inner js-file-line">	EVROverlayIntersectionMaskPrimitiveType_OverlayIntersectionPrimitiveType_Circle = <span class="pl-c1">1</span>,</td>
      </tr>
      <tr>
        <td id="L927" class="blob-num js-line-number" data-line-number="927"></td>
        <td id="LC927" class="blob-code blob-code-inner js-file-line">} EVROverlayIntersectionMaskPrimitiveType;</td>
      </tr>
      <tr>
        <td id="L928" class="blob-num js-line-number" data-line-number="928"></td>
        <td id="LC928" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L929" class="blob-num js-line-number" data-line-number="929"></td>
        <td id="LC929" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">enum</span> EVRRenderModelError</td>
      </tr>
      <tr>
        <td id="L930" class="blob-num js-line-number" data-line-number="930"></td>
        <td id="LC930" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L931" class="blob-num js-line-number" data-line-number="931"></td>
        <td id="LC931" class="blob-code blob-code-inner js-file-line">	EVRRenderModelError_VRRenderModelError_None = <span class="pl-c1">0</span>,</td>
      </tr>
      <tr>
        <td id="L932" class="blob-num js-line-number" data-line-number="932"></td>
        <td id="LC932" class="blob-code blob-code-inner js-file-line">	EVRRenderModelError_VRRenderModelError_Loading = <span class="pl-c1">100</span>,</td>
      </tr>
      <tr>
        <td id="L933" class="blob-num js-line-number" data-line-number="933"></td>
        <td id="LC933" class="blob-code blob-code-inner js-file-line">	EVRRenderModelError_VRRenderModelError_NotSupported = <span class="pl-c1">200</span>,</td>
      </tr>
      <tr>
        <td id="L934" class="blob-num js-line-number" data-line-number="934"></td>
        <td id="LC934" class="blob-code blob-code-inner js-file-line">	EVRRenderModelError_VRRenderModelError_InvalidArg = <span class="pl-c1">300</span>,</td>
      </tr>
      <tr>
        <td id="L935" class="blob-num js-line-number" data-line-number="935"></td>
        <td id="LC935" class="blob-code blob-code-inner js-file-line">	EVRRenderModelError_VRRenderModelError_InvalidModel = <span class="pl-c1">301</span>,</td>
      </tr>
      <tr>
        <td id="L936" class="blob-num js-line-number" data-line-number="936"></td>
        <td id="LC936" class="blob-code blob-code-inner js-file-line">	EVRRenderModelError_VRRenderModelError_NoShapes = <span class="pl-c1">302</span>,</td>
      </tr>
      <tr>
        <td id="L937" class="blob-num js-line-number" data-line-number="937"></td>
        <td id="LC937" class="blob-code blob-code-inner js-file-line">	EVRRenderModelError_VRRenderModelError_MultipleShapes = <span class="pl-c1">303</span>,</td>
      </tr>
      <tr>
        <td id="L938" class="blob-num js-line-number" data-line-number="938"></td>
        <td id="LC938" class="blob-code blob-code-inner js-file-line">	EVRRenderModelError_VRRenderModelError_TooManyVertices = <span class="pl-c1">304</span>,</td>
      </tr>
      <tr>
        <td id="L939" class="blob-num js-line-number" data-line-number="939"></td>
        <td id="LC939" class="blob-code blob-code-inner js-file-line">	EVRRenderModelError_VRRenderModelError_MultipleTextures = <span class="pl-c1">305</span>,</td>
      </tr>
      <tr>
        <td id="L940" class="blob-num js-line-number" data-line-number="940"></td>
        <td id="LC940" class="blob-code blob-code-inner js-file-line">	EVRRenderModelError_VRRenderModelError_BufferTooSmall = <span class="pl-c1">306</span>,</td>
      </tr>
      <tr>
        <td id="L941" class="blob-num js-line-number" data-line-number="941"></td>
        <td id="LC941" class="blob-code blob-code-inner js-file-line">	EVRRenderModelError_VRRenderModelError_NotEnoughNormals = <span class="pl-c1">307</span>,</td>
      </tr>
      <tr>
        <td id="L942" class="blob-num js-line-number" data-line-number="942"></td>
        <td id="LC942" class="blob-code blob-code-inner js-file-line">	EVRRenderModelError_VRRenderModelError_NotEnoughTexCoords = <span class="pl-c1">308</span>,</td>
      </tr>
      <tr>
        <td id="L943" class="blob-num js-line-number" data-line-number="943"></td>
        <td id="LC943" class="blob-code blob-code-inner js-file-line">	EVRRenderModelError_VRRenderModelError_InvalidTexture = <span class="pl-c1">400</span>,</td>
      </tr>
      <tr>
        <td id="L944" class="blob-num js-line-number" data-line-number="944"></td>
        <td id="LC944" class="blob-code blob-code-inner js-file-line">} EVRRenderModelError;</td>
      </tr>
      <tr>
        <td id="L945" class="blob-num js-line-number" data-line-number="945"></td>
        <td id="LC945" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L946" class="blob-num js-line-number" data-line-number="946"></td>
        <td id="LC946" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">enum</span> EVRComponentProperty</td>
      </tr>
      <tr>
        <td id="L947" class="blob-num js-line-number" data-line-number="947"></td>
        <td id="LC947" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L948" class="blob-num js-line-number" data-line-number="948"></td>
        <td id="LC948" class="blob-code blob-code-inner js-file-line">	EVRComponentProperty_VRComponentProperty_IsStatic = <span class="pl-c1">1</span>,</td>
      </tr>
      <tr>
        <td id="L949" class="blob-num js-line-number" data-line-number="949"></td>
        <td id="LC949" class="blob-code blob-code-inner js-file-line">	EVRComponentProperty_VRComponentProperty_IsVisible = <span class="pl-c1">2</span>,</td>
      </tr>
      <tr>
        <td id="L950" class="blob-num js-line-number" data-line-number="950"></td>
        <td id="LC950" class="blob-code blob-code-inner js-file-line">	EVRComponentProperty_VRComponentProperty_IsTouched = <span class="pl-c1">4</span>,</td>
      </tr>
      <tr>
        <td id="L951" class="blob-num js-line-number" data-line-number="951"></td>
        <td id="LC951" class="blob-code blob-code-inner js-file-line">	EVRComponentProperty_VRComponentProperty_IsPressed = <span class="pl-c1">8</span>,</td>
      </tr>
      <tr>
        <td id="L952" class="blob-num js-line-number" data-line-number="952"></td>
        <td id="LC952" class="blob-code blob-code-inner js-file-line">	EVRComponentProperty_VRComponentProperty_IsScrolled = <span class="pl-c1">16</span>,</td>
      </tr>
      <tr>
        <td id="L953" class="blob-num js-line-number" data-line-number="953"></td>
        <td id="LC953" class="blob-code blob-code-inner js-file-line">} EVRComponentProperty;</td>
      </tr>
      <tr>
        <td id="L954" class="blob-num js-line-number" data-line-number="954"></td>
        <td id="LC954" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L955" class="blob-num js-line-number" data-line-number="955"></td>
        <td id="LC955" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">enum</span> EVRNotificationType</td>
      </tr>
      <tr>
        <td id="L956" class="blob-num js-line-number" data-line-number="956"></td>
        <td id="LC956" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L957" class="blob-num js-line-number" data-line-number="957"></td>
        <td id="LC957" class="blob-code blob-code-inner js-file-line">	EVRNotificationType_Transient = <span class="pl-c1">0</span>,</td>
      </tr>
      <tr>
        <td id="L958" class="blob-num js-line-number" data-line-number="958"></td>
        <td id="LC958" class="blob-code blob-code-inner js-file-line">	EVRNotificationType_Persistent = <span class="pl-c1">1</span>,</td>
      </tr>
      <tr>
        <td id="L959" class="blob-num js-line-number" data-line-number="959"></td>
        <td id="LC959" class="blob-code blob-code-inner js-file-line">	EVRNotificationType_Transient_SystemWithUserValue = <span class="pl-c1">2</span>,</td>
      </tr>
      <tr>
        <td id="L960" class="blob-num js-line-number" data-line-number="960"></td>
        <td id="LC960" class="blob-code blob-code-inner js-file-line">} EVRNotificationType;</td>
      </tr>
      <tr>
        <td id="L961" class="blob-num js-line-number" data-line-number="961"></td>
        <td id="LC961" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L962" class="blob-num js-line-number" data-line-number="962"></td>
        <td id="LC962" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">enum</span> EVRNotificationStyle</td>
      </tr>
      <tr>
        <td id="L963" class="blob-num js-line-number" data-line-number="963"></td>
        <td id="LC963" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L964" class="blob-num js-line-number" data-line-number="964"></td>
        <td id="LC964" class="blob-code blob-code-inner js-file-line">	EVRNotificationStyle_None = <span class="pl-c1">0</span>,</td>
      </tr>
      <tr>
        <td id="L965" class="blob-num js-line-number" data-line-number="965"></td>
        <td id="LC965" class="blob-code blob-code-inner js-file-line">	EVRNotificationStyle_Application = <span class="pl-c1">100</span>,</td>
      </tr>
      <tr>
        <td id="L966" class="blob-num js-line-number" data-line-number="966"></td>
        <td id="LC966" class="blob-code blob-code-inner js-file-line">	EVRNotificationStyle_Contact_Disabled = <span class="pl-c1">200</span>,</td>
      </tr>
      <tr>
        <td id="L967" class="blob-num js-line-number" data-line-number="967"></td>
        <td id="LC967" class="blob-code blob-code-inner js-file-line">	EVRNotificationStyle_Contact_Enabled = <span class="pl-c1">201</span>,</td>
      </tr>
      <tr>
        <td id="L968" class="blob-num js-line-number" data-line-number="968"></td>
        <td id="LC968" class="blob-code blob-code-inner js-file-line">	EVRNotificationStyle_Contact_Active = <span class="pl-c1">202</span>,</td>
      </tr>
      <tr>
        <td id="L969" class="blob-num js-line-number" data-line-number="969"></td>
        <td id="LC969" class="blob-code blob-code-inner js-file-line">} EVRNotificationStyle;</td>
      </tr>
      <tr>
        <td id="L970" class="blob-num js-line-number" data-line-number="970"></td>
        <td id="LC970" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L971" class="blob-num js-line-number" data-line-number="971"></td>
        <td id="LC971" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">enum</span> EVRSettingsError</td>
      </tr>
      <tr>
        <td id="L972" class="blob-num js-line-number" data-line-number="972"></td>
        <td id="LC972" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L973" class="blob-num js-line-number" data-line-number="973"></td>
        <td id="LC973" class="blob-code blob-code-inner js-file-line">	EVRSettingsError_VRSettingsError_None = <span class="pl-c1">0</span>,</td>
      </tr>
      <tr>
        <td id="L974" class="blob-num js-line-number" data-line-number="974"></td>
        <td id="LC974" class="blob-code blob-code-inner js-file-line">	EVRSettingsError_VRSettingsError_IPCFailed = <span class="pl-c1">1</span>,</td>
      </tr>
      <tr>
        <td id="L975" class="blob-num js-line-number" data-line-number="975"></td>
        <td id="LC975" class="blob-code blob-code-inner js-file-line">	EVRSettingsError_VRSettingsError_WriteFailed = <span class="pl-c1">2</span>,</td>
      </tr>
      <tr>
        <td id="L976" class="blob-num js-line-number" data-line-number="976"></td>
        <td id="LC976" class="blob-code blob-code-inner js-file-line">	EVRSettingsError_VRSettingsError_ReadFailed = <span class="pl-c1">3</span>,</td>
      </tr>
      <tr>
        <td id="L977" class="blob-num js-line-number" data-line-number="977"></td>
        <td id="LC977" class="blob-code blob-code-inner js-file-line">	EVRSettingsError_VRSettingsError_JsonParseFailed = <span class="pl-c1">4</span>,</td>
      </tr>
      <tr>
        <td id="L978" class="blob-num js-line-number" data-line-number="978"></td>
        <td id="LC978" class="blob-code blob-code-inner js-file-line">	EVRSettingsError_VRSettingsError_UnsetSettingHasNoDefault = <span class="pl-c1">5</span>,</td>
      </tr>
      <tr>
        <td id="L979" class="blob-num js-line-number" data-line-number="979"></td>
        <td id="LC979" class="blob-code blob-code-inner js-file-line">} EVRSettingsError;</td>
      </tr>
      <tr>
        <td id="L980" class="blob-num js-line-number" data-line-number="980"></td>
        <td id="LC980" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L981" class="blob-num js-line-number" data-line-number="981"></td>
        <td id="LC981" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">enum</span> EVRScreenshotError</td>
      </tr>
      <tr>
        <td id="L982" class="blob-num js-line-number" data-line-number="982"></td>
        <td id="LC982" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L983" class="blob-num js-line-number" data-line-number="983"></td>
        <td id="LC983" class="blob-code blob-code-inner js-file-line">	EVRScreenshotError_VRScreenshotError_None = <span class="pl-c1">0</span>,</td>
      </tr>
      <tr>
        <td id="L984" class="blob-num js-line-number" data-line-number="984"></td>
        <td id="LC984" class="blob-code blob-code-inner js-file-line">	EVRScreenshotError_VRScreenshotError_RequestFailed = <span class="pl-c1">1</span>,</td>
      </tr>
      <tr>
        <td id="L985" class="blob-num js-line-number" data-line-number="985"></td>
        <td id="LC985" class="blob-code blob-code-inner js-file-line">	EVRScreenshotError_VRScreenshotError_IncompatibleVersion = <span class="pl-c1">100</span>,</td>
      </tr>
      <tr>
        <td id="L986" class="blob-num js-line-number" data-line-number="986"></td>
        <td id="LC986" class="blob-code blob-code-inner js-file-line">	EVRScreenshotError_VRScreenshotError_NotFound = <span class="pl-c1">101</span>,</td>
      </tr>
      <tr>
        <td id="L987" class="blob-num js-line-number" data-line-number="987"></td>
        <td id="LC987" class="blob-code blob-code-inner js-file-line">	EVRScreenshotError_VRScreenshotError_BufferTooSmall = <span class="pl-c1">102</span>,</td>
      </tr>
      <tr>
        <td id="L988" class="blob-num js-line-number" data-line-number="988"></td>
        <td id="LC988" class="blob-code blob-code-inner js-file-line">	EVRScreenshotError_VRScreenshotError_ScreenshotAlreadyInProgress = <span class="pl-c1">108</span>,</td>
      </tr>
      <tr>
        <td id="L989" class="blob-num js-line-number" data-line-number="989"></td>
        <td id="LC989" class="blob-code blob-code-inner js-file-line">} EVRScreenshotError;</td>
      </tr>
      <tr>
        <td id="L990" class="blob-num js-line-number" data-line-number="990"></td>
        <td id="LC990" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L991" class="blob-num js-line-number" data-line-number="991"></td>
        <td id="LC991" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L992" class="blob-num js-line-number" data-line-number="992"></td>
        <td id="LC992" class="blob-code blob-code-inner js-file-line"><span class="pl-c"><span class="pl-c">//</span> OpenVR typedefs</span></td>
      </tr>
      <tr>
        <td id="L993" class="blob-num js-line-number" data-line-number="993"></td>
        <td id="LC993" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L994" class="blob-num js-line-number" data-line-number="994"></td>
        <td id="LC994" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-c1">uint32_t</span> TrackedDeviceIndex_t;</td>
      </tr>
      <tr>
        <td id="L995" class="blob-num js-line-number" data-line-number="995"></td>
        <td id="LC995" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-c1">uint32_t</span> VRNotificationId;</td>
      </tr>
      <tr>
        <td id="L996" class="blob-num js-line-number" data-line-number="996"></td>
        <td id="LC996" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-c1">uint64_t</span> VROverlayHandle_t;</td>
      </tr>
      <tr>
        <td id="L997" class="blob-num js-line-number" data-line-number="997"></td>
        <td id="LC997" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">void</span> * glSharedTextureHandle_t;</td>
      </tr>
      <tr>
        <td id="L998" class="blob-num js-line-number" data-line-number="998"></td>
        <td id="LC998" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-c1">int32_t</span> glInt_t;</td>
      </tr>
      <tr>
        <td id="L999" class="blob-num js-line-number" data-line-number="999"></td>
        <td id="LC999" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-c1">uint32_t</span> glUInt_t;</td>
      </tr>
      <tr>
        <td id="L1000" class="blob-num js-line-number" data-line-number="1000"></td>
        <td id="LC1000" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-c1">uint64_t</span> SharedTextureHandle_t;</td>
      </tr>
      <tr>
        <td id="L1001" class="blob-num js-line-number" data-line-number="1001"></td>
        <td id="LC1001" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-c1">uint32_t</span> TrackedDeviceIndex_t;</td>
      </tr>
      <tr>
        <td id="L1002" class="blob-num js-line-number" data-line-number="1002"></td>
        <td id="LC1002" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-c1">uint64_t</span> VROverlayHandle_t;</td>
      </tr>
      <tr>
        <td id="L1003" class="blob-num js-line-number" data-line-number="1003"></td>
        <td id="LC1003" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-c1">uint64_t</span> TrackedCameraHandle_t;</td>
      </tr>
      <tr>
        <td id="L1004" class="blob-num js-line-number" data-line-number="1004"></td>
        <td id="LC1004" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-c1">uint32_t</span> ScreenshotHandle_t;</td>
      </tr>
      <tr>
        <td id="L1005" class="blob-num js-line-number" data-line-number="1005"></td>
        <td id="LC1005" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-c1">uint32_t</span> VRComponentProperties;</td>
      </tr>
      <tr>
        <td id="L1006" class="blob-num js-line-number" data-line-number="1006"></td>
        <td id="LC1006" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-c1">int32_t</span> TextureID_t;</td>
      </tr>
      <tr>
        <td id="L1007" class="blob-num js-line-number" data-line-number="1007"></td>
        <td id="LC1007" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-c1">uint32_t</span> VRNotificationId;</td>
      </tr>
      <tr>
        <td id="L1008" class="blob-num js-line-number" data-line-number="1008"></td>
        <td id="LC1008" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> EVRInitError HmdError;</td>
      </tr>
      <tr>
        <td id="L1009" class="blob-num js-line-number" data-line-number="1009"></td>
        <td id="LC1009" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> EVREye Hmd_Eye;</td>
      </tr>
      <tr>
        <td id="L1010" class="blob-num js-line-number" data-line-number="1010"></td>
        <td id="LC1010" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> EColorSpace ColorSpace;</td>
      </tr>
      <tr>
        <td id="L1011" class="blob-num js-line-number" data-line-number="1011"></td>
        <td id="LC1011" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> ETrackingResult HmdTrackingResult;</td>
      </tr>
      <tr>
        <td id="L1012" class="blob-num js-line-number" data-line-number="1012"></td>
        <td id="LC1012" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> ETrackedDeviceClass TrackedDeviceClass;</td>
      </tr>
      <tr>
        <td id="L1013" class="blob-num js-line-number" data-line-number="1013"></td>
        <td id="LC1013" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> ETrackingUniverseOrigin TrackingUniverseOrigin;</td>
      </tr>
      <tr>
        <td id="L1014" class="blob-num js-line-number" data-line-number="1014"></td>
        <td id="LC1014" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> ETrackedDeviceProperty TrackedDeviceProperty;</td>
      </tr>
      <tr>
        <td id="L1015" class="blob-num js-line-number" data-line-number="1015"></td>
        <td id="LC1015" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> ETrackedPropertyError TrackedPropertyError;</td>
      </tr>
      <tr>
        <td id="L1016" class="blob-num js-line-number" data-line-number="1016"></td>
        <td id="LC1016" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> EVRSubmitFlags VRSubmitFlags_t;</td>
      </tr>
      <tr>
        <td id="L1017" class="blob-num js-line-number" data-line-number="1017"></td>
        <td id="LC1017" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> EVRState VRState_t;</td>
      </tr>
      <tr>
        <td id="L1018" class="blob-num js-line-number" data-line-number="1018"></td>
        <td id="LC1018" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> ECollisionBoundsStyle CollisionBoundsStyle_t;</td>
      </tr>
      <tr>
        <td id="L1019" class="blob-num js-line-number" data-line-number="1019"></td>
        <td id="LC1019" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> EVROverlayError VROverlayError;</td>
      </tr>
      <tr>
        <td id="L1020" class="blob-num js-line-number" data-line-number="1020"></td>
        <td id="LC1020" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> EVRFirmwareError VRFirmwareError;</td>
      </tr>
      <tr>
        <td id="L1021" class="blob-num js-line-number" data-line-number="1021"></td>
        <td id="LC1021" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> EVRCompositorError VRCompositorError;</td>
      </tr>
      <tr>
        <td id="L1022" class="blob-num js-line-number" data-line-number="1022"></td>
        <td id="LC1022" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> EVRScreenshotError VRScreenshotsError;</td>
      </tr>
      <tr>
        <td id="L1023" class="blob-num js-line-number" data-line-number="1023"></td>
        <td id="LC1023" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L1024" class="blob-num js-line-number" data-line-number="1024"></td>
        <td id="LC1024" class="blob-code blob-code-inner js-file-line"><span class="pl-c"><span class="pl-c">//</span> OpenVR Structs</span></td>
      </tr>
      <tr>
        <td id="L1025" class="blob-num js-line-number" data-line-number="1025"></td>
        <td id="LC1025" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L1026" class="blob-num js-line-number" data-line-number="1026"></td>
        <td id="LC1026" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">struct</span> HmdMatrix34_t</td>
      </tr>
      <tr>
        <td id="L1027" class="blob-num js-line-number" data-line-number="1027"></td>
        <td id="LC1027" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L1028" class="blob-num js-line-number" data-line-number="1028"></td>
        <td id="LC1028" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">float</span> m[<span class="pl-c1">3</span>][<span class="pl-c1">4</span>]; <span class="pl-c"><span class="pl-c">//</span>float[3][4]</span></td>
      </tr>
      <tr>
        <td id="L1029" class="blob-num js-line-number" data-line-number="1029"></td>
        <td id="LC1029" class="blob-code blob-code-inner js-file-line">} HmdMatrix34_t;</td>
      </tr>
      <tr>
        <td id="L1030" class="blob-num js-line-number" data-line-number="1030"></td>
        <td id="LC1030" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L1031" class="blob-num js-line-number" data-line-number="1031"></td>
        <td id="LC1031" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">struct</span> HmdMatrix44_t</td>
      </tr>
      <tr>
        <td id="L1032" class="blob-num js-line-number" data-line-number="1032"></td>
        <td id="LC1032" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L1033" class="blob-num js-line-number" data-line-number="1033"></td>
        <td id="LC1033" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">float</span> m[<span class="pl-c1">4</span>][<span class="pl-c1">4</span>]; <span class="pl-c"><span class="pl-c">//</span>float[4][4]</span></td>
      </tr>
      <tr>
        <td id="L1034" class="blob-num js-line-number" data-line-number="1034"></td>
        <td id="LC1034" class="blob-code blob-code-inner js-file-line">} HmdMatrix44_t;</td>
      </tr>
      <tr>
        <td id="L1035" class="blob-num js-line-number" data-line-number="1035"></td>
        <td id="LC1035" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L1036" class="blob-num js-line-number" data-line-number="1036"></td>
        <td id="LC1036" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">struct</span> HmdVector3_t</td>
      </tr>
      <tr>
        <td id="L1037" class="blob-num js-line-number" data-line-number="1037"></td>
        <td id="LC1037" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L1038" class="blob-num js-line-number" data-line-number="1038"></td>
        <td id="LC1038" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">float</span> v[<span class="pl-c1">3</span>]; <span class="pl-c"><span class="pl-c">//</span>float[3]</span></td>
      </tr>
      <tr>
        <td id="L1039" class="blob-num js-line-number" data-line-number="1039"></td>
        <td id="LC1039" class="blob-code blob-code-inner js-file-line">} HmdVector3_t;</td>
      </tr>
      <tr>
        <td id="L1040" class="blob-num js-line-number" data-line-number="1040"></td>
        <td id="LC1040" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L1041" class="blob-num js-line-number" data-line-number="1041"></td>
        <td id="LC1041" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">struct</span> HmdVector4_t</td>
      </tr>
      <tr>
        <td id="L1042" class="blob-num js-line-number" data-line-number="1042"></td>
        <td id="LC1042" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L1043" class="blob-num js-line-number" data-line-number="1043"></td>
        <td id="LC1043" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">float</span> v[<span class="pl-c1">4</span>]; <span class="pl-c"><span class="pl-c">//</span>float[4]</span></td>
      </tr>
      <tr>
        <td id="L1044" class="blob-num js-line-number" data-line-number="1044"></td>
        <td id="LC1044" class="blob-code blob-code-inner js-file-line">} HmdVector4_t;</td>
      </tr>
      <tr>
        <td id="L1045" class="blob-num js-line-number" data-line-number="1045"></td>
        <td id="LC1045" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L1046" class="blob-num js-line-number" data-line-number="1046"></td>
        <td id="LC1046" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">struct</span> HmdVector3d_t</td>
      </tr>
      <tr>
        <td id="L1047" class="blob-num js-line-number" data-line-number="1047"></td>
        <td id="LC1047" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L1048" class="blob-num js-line-number" data-line-number="1048"></td>
        <td id="LC1048" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">double</span> v[<span class="pl-c1">3</span>]; <span class="pl-c"><span class="pl-c">//</span>double[3]</span></td>
      </tr>
      <tr>
        <td id="L1049" class="blob-num js-line-number" data-line-number="1049"></td>
        <td id="LC1049" class="blob-code blob-code-inner js-file-line">} HmdVector3d_t;</td>
      </tr>
      <tr>
        <td id="L1050" class="blob-num js-line-number" data-line-number="1050"></td>
        <td id="LC1050" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L1051" class="blob-num js-line-number" data-line-number="1051"></td>
        <td id="LC1051" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">struct</span> HmdVector2_t</td>
      </tr>
      <tr>
        <td id="L1052" class="blob-num js-line-number" data-line-number="1052"></td>
        <td id="LC1052" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L1053" class="blob-num js-line-number" data-line-number="1053"></td>
        <td id="LC1053" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">float</span> v[<span class="pl-c1">2</span>]; <span class="pl-c"><span class="pl-c">//</span>float[2]</span></td>
      </tr>
      <tr>
        <td id="L1054" class="blob-num js-line-number" data-line-number="1054"></td>
        <td id="LC1054" class="blob-code blob-code-inner js-file-line">} HmdVector2_t;</td>
      </tr>
      <tr>
        <td id="L1055" class="blob-num js-line-number" data-line-number="1055"></td>
        <td id="LC1055" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L1056" class="blob-num js-line-number" data-line-number="1056"></td>
        <td id="LC1056" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">struct</span> HmdQuaternion_t</td>
      </tr>
      <tr>
        <td id="L1057" class="blob-num js-line-number" data-line-number="1057"></td>
        <td id="LC1057" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L1058" class="blob-num js-line-number" data-line-number="1058"></td>
        <td id="LC1058" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">double</span> w;</td>
      </tr>
      <tr>
        <td id="L1059" class="blob-num js-line-number" data-line-number="1059"></td>
        <td id="LC1059" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">double</span> x;</td>
      </tr>
      <tr>
        <td id="L1060" class="blob-num js-line-number" data-line-number="1060"></td>
        <td id="LC1060" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">double</span> y;</td>
      </tr>
      <tr>
        <td id="L1061" class="blob-num js-line-number" data-line-number="1061"></td>
        <td id="LC1061" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">double</span> z;</td>
      </tr>
      <tr>
        <td id="L1062" class="blob-num js-line-number" data-line-number="1062"></td>
        <td id="LC1062" class="blob-code blob-code-inner js-file-line">} HmdQuaternion_t;</td>
      </tr>
      <tr>
        <td id="L1063" class="blob-num js-line-number" data-line-number="1063"></td>
        <td id="LC1063" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L1064" class="blob-num js-line-number" data-line-number="1064"></td>
        <td id="LC1064" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">struct</span> HmdColor_t</td>
      </tr>
      <tr>
        <td id="L1065" class="blob-num js-line-number" data-line-number="1065"></td>
        <td id="LC1065" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L1066" class="blob-num js-line-number" data-line-number="1066"></td>
        <td id="LC1066" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">float</span> r;</td>
      </tr>
      <tr>
        <td id="L1067" class="blob-num js-line-number" data-line-number="1067"></td>
        <td id="LC1067" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">float</span> g;</td>
      </tr>
      <tr>
        <td id="L1068" class="blob-num js-line-number" data-line-number="1068"></td>
        <td id="LC1068" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">float</span> b;</td>
      </tr>
      <tr>
        <td id="L1069" class="blob-num js-line-number" data-line-number="1069"></td>
        <td id="LC1069" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">float</span> a;</td>
      </tr>
      <tr>
        <td id="L1070" class="blob-num js-line-number" data-line-number="1070"></td>
        <td id="LC1070" class="blob-code blob-code-inner js-file-line">} HmdColor_t;</td>
      </tr>
      <tr>
        <td id="L1071" class="blob-num js-line-number" data-line-number="1071"></td>
        <td id="LC1071" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L1072" class="blob-num js-line-number" data-line-number="1072"></td>
        <td id="LC1072" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">struct</span> HmdQuad_t</td>
      </tr>
      <tr>
        <td id="L1073" class="blob-num js-line-number" data-line-number="1073"></td>
        <td id="LC1073" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L1074" class="blob-num js-line-number" data-line-number="1074"></td>
        <td id="LC1074" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">struct</span> HmdVector3_t vCorners[<span class="pl-c1">4</span>]; <span class="pl-c"><span class="pl-c">//</span>struct vr::HmdVector3_t[4]</span></td>
      </tr>
      <tr>
        <td id="L1075" class="blob-num js-line-number" data-line-number="1075"></td>
        <td id="LC1075" class="blob-code blob-code-inner js-file-line">} HmdQuad_t;</td>
      </tr>
      <tr>
        <td id="L1076" class="blob-num js-line-number" data-line-number="1076"></td>
        <td id="LC1076" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L1077" class="blob-num js-line-number" data-line-number="1077"></td>
        <td id="LC1077" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">struct</span> HmdRect2_t</td>
      </tr>
      <tr>
        <td id="L1078" class="blob-num js-line-number" data-line-number="1078"></td>
        <td id="LC1078" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L1079" class="blob-num js-line-number" data-line-number="1079"></td>
        <td id="LC1079" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">struct</span> HmdVector2_t vTopLeft;</td>
      </tr>
      <tr>
        <td id="L1080" class="blob-num js-line-number" data-line-number="1080"></td>
        <td id="LC1080" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">struct</span> HmdVector2_t vBottomRight;</td>
      </tr>
      <tr>
        <td id="L1081" class="blob-num js-line-number" data-line-number="1081"></td>
        <td id="LC1081" class="blob-code blob-code-inner js-file-line">} HmdRect2_t;</td>
      </tr>
      <tr>
        <td id="L1082" class="blob-num js-line-number" data-line-number="1082"></td>
        <td id="LC1082" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L1083" class="blob-num js-line-number" data-line-number="1083"></td>
        <td id="LC1083" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">struct</span> DistortionCoordinates_t</td>
      </tr>
      <tr>
        <td id="L1084" class="blob-num js-line-number" data-line-number="1084"></td>
        <td id="LC1084" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L1085" class="blob-num js-line-number" data-line-number="1085"></td>
        <td id="LC1085" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">float</span> rfRed[<span class="pl-c1">2</span>]; <span class="pl-c"><span class="pl-c">//</span>float[2]</span></td>
      </tr>
      <tr>
        <td id="L1086" class="blob-num js-line-number" data-line-number="1086"></td>
        <td id="LC1086" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">float</span> rfGreen[<span class="pl-c1">2</span>]; <span class="pl-c"><span class="pl-c">//</span>float[2]</span></td>
      </tr>
      <tr>
        <td id="L1087" class="blob-num js-line-number" data-line-number="1087"></td>
        <td id="LC1087" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">float</span> rfBlue[<span class="pl-c1">2</span>]; <span class="pl-c"><span class="pl-c">//</span>float[2]</span></td>
      </tr>
      <tr>
        <td id="L1088" class="blob-num js-line-number" data-line-number="1088"></td>
        <td id="LC1088" class="blob-code blob-code-inner js-file-line">} DistortionCoordinates_t;</td>
      </tr>
      <tr>
        <td id="L1089" class="blob-num js-line-number" data-line-number="1089"></td>
        <td id="LC1089" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L1090" class="blob-num js-line-number" data-line-number="1090"></td>
        <td id="LC1090" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">struct</span> Texture_t</td>
      </tr>
      <tr>
        <td id="L1091" class="blob-num js-line-number" data-line-number="1091"></td>
        <td id="LC1091" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L1092" class="blob-num js-line-number" data-line-number="1092"></td>
        <td id="LC1092" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">void</span> * handle; <span class="pl-c"><span class="pl-c">//</span> void *</span></td>
      </tr>
      <tr>
        <td id="L1093" class="blob-num js-line-number" data-line-number="1093"></td>
        <td id="LC1093" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">enum</span> ETextureType eType;</td>
      </tr>
      <tr>
        <td id="L1094" class="blob-num js-line-number" data-line-number="1094"></td>
        <td id="LC1094" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">enum</span> EColorSpace eColorSpace;</td>
      </tr>
      <tr>
        <td id="L1095" class="blob-num js-line-number" data-line-number="1095"></td>
        <td id="LC1095" class="blob-code blob-code-inner js-file-line">} Texture_t;</td>
      </tr>
      <tr>
        <td id="L1096" class="blob-num js-line-number" data-line-number="1096"></td>
        <td id="LC1096" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L1097" class="blob-num js-line-number" data-line-number="1097"></td>
        <td id="LC1097" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">struct</span> TrackedDevicePose_t</td>
      </tr>
      <tr>
        <td id="L1098" class="blob-num js-line-number" data-line-number="1098"></td>
        <td id="LC1098" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L1099" class="blob-num js-line-number" data-line-number="1099"></td>
        <td id="LC1099" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">struct</span> HmdMatrix34_t mDeviceToAbsoluteTracking;</td>
      </tr>
      <tr>
        <td id="L1100" class="blob-num js-line-number" data-line-number="1100"></td>
        <td id="LC1100" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">struct</span> HmdVector3_t vVelocity;</td>
      </tr>
      <tr>
        <td id="L1101" class="blob-num js-line-number" data-line-number="1101"></td>
        <td id="LC1101" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">struct</span> HmdVector3_t vAngularVelocity;</td>
      </tr>
      <tr>
        <td id="L1102" class="blob-num js-line-number" data-line-number="1102"></td>
        <td id="LC1102" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">enum</span> ETrackingResult eTrackingResult;</td>
      </tr>
      <tr>
        <td id="L1103" class="blob-num js-line-number" data-line-number="1103"></td>
        <td id="LC1103" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">bool</span> bPoseIsValid;</td>
      </tr>
      <tr>
        <td id="L1104" class="blob-num js-line-number" data-line-number="1104"></td>
        <td id="LC1104" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">bool</span> bDeviceIsConnected;</td>
      </tr>
      <tr>
        <td id="L1105" class="blob-num js-line-number" data-line-number="1105"></td>
        <td id="LC1105" class="blob-code blob-code-inner js-file-line">} TrackedDevicePose_t;</td>
      </tr>
      <tr>
        <td id="L1106" class="blob-num js-line-number" data-line-number="1106"></td>
        <td id="LC1106" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L1107" class="blob-num js-line-number" data-line-number="1107"></td>
        <td id="LC1107" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">struct</span> VRTextureBounds_t</td>
      </tr>
      <tr>
        <td id="L1108" class="blob-num js-line-number" data-line-number="1108"></td>
        <td id="LC1108" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L1109" class="blob-num js-line-number" data-line-number="1109"></td>
        <td id="LC1109" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">float</span> uMin;</td>
      </tr>
      <tr>
        <td id="L1110" class="blob-num js-line-number" data-line-number="1110"></td>
        <td id="LC1110" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">float</span> vMin;</td>
      </tr>
      <tr>
        <td id="L1111" class="blob-num js-line-number" data-line-number="1111"></td>
        <td id="LC1111" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">float</span> uMax;</td>
      </tr>
      <tr>
        <td id="L1112" class="blob-num js-line-number" data-line-number="1112"></td>
        <td id="LC1112" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">float</span> vMax;</td>
      </tr>
      <tr>
        <td id="L1113" class="blob-num js-line-number" data-line-number="1113"></td>
        <td id="LC1113" class="blob-code blob-code-inner js-file-line">} VRTextureBounds_t;</td>
      </tr>
      <tr>
        <td id="L1114" class="blob-num js-line-number" data-line-number="1114"></td>
        <td id="LC1114" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L1115" class="blob-num js-line-number" data-line-number="1115"></td>
        <td id="LC1115" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">struct</span> VRVulkanTextureData_t</td>
      </tr>
      <tr>
        <td id="L1116" class="blob-num js-line-number" data-line-number="1116"></td>
        <td id="LC1116" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L1117" class="blob-num js-line-number" data-line-number="1117"></td>
        <td id="LC1117" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint64_t</span> m_nImage;</td>
      </tr>
      <tr>
        <td id="L1118" class="blob-num js-line-number" data-line-number="1118"></td>
        <td id="LC1118" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">struct</span> VkDevice_T * m_pDevice; <span class="pl-c"><span class="pl-c">//</span> struct VkDevice_T *</span></td>
      </tr>
      <tr>
        <td id="L1119" class="blob-num js-line-number" data-line-number="1119"></td>
        <td id="LC1119" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">struct</span> VkPhysicalDevice_T * m_pPhysicalDevice; <span class="pl-c"><span class="pl-c">//</span> struct VkPhysicalDevice_T *</span></td>
      </tr>
      <tr>
        <td id="L1120" class="blob-num js-line-number" data-line-number="1120"></td>
        <td id="LC1120" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">struct</span> VkInstance_T * m_pInstance; <span class="pl-c"><span class="pl-c">//</span> struct VkInstance_T *</span></td>
      </tr>
      <tr>
        <td id="L1121" class="blob-num js-line-number" data-line-number="1121"></td>
        <td id="LC1121" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">struct</span> VkQueue_T * m_pQueue; <span class="pl-c"><span class="pl-c">//</span> struct VkQueue_T *</span></td>
      </tr>
      <tr>
        <td id="L1122" class="blob-num js-line-number" data-line-number="1122"></td>
        <td id="LC1122" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint32_t</span> m_nQueueFamilyIndex;</td>
      </tr>
      <tr>
        <td id="L1123" class="blob-num js-line-number" data-line-number="1123"></td>
        <td id="LC1123" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint32_t</span> m_nWidth;</td>
      </tr>
      <tr>
        <td id="L1124" class="blob-num js-line-number" data-line-number="1124"></td>
        <td id="LC1124" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint32_t</span> m_nHeight;</td>
      </tr>
      <tr>
        <td id="L1125" class="blob-num js-line-number" data-line-number="1125"></td>
        <td id="LC1125" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint32_t</span> m_nFormat;</td>
      </tr>
      <tr>
        <td id="L1126" class="blob-num js-line-number" data-line-number="1126"></td>
        <td id="LC1126" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint32_t</span> m_nSampleCount;</td>
      </tr>
      <tr>
        <td id="L1127" class="blob-num js-line-number" data-line-number="1127"></td>
        <td id="LC1127" class="blob-code blob-code-inner js-file-line">} VRVulkanTextureData_t;</td>
      </tr>
      <tr>
        <td id="L1128" class="blob-num js-line-number" data-line-number="1128"></td>
        <td id="LC1128" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L1129" class="blob-num js-line-number" data-line-number="1129"></td>
        <td id="LC1129" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">struct</span> VREvent_Controller_t</td>
      </tr>
      <tr>
        <td id="L1130" class="blob-num js-line-number" data-line-number="1130"></td>
        <td id="LC1130" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L1131" class="blob-num js-line-number" data-line-number="1131"></td>
        <td id="LC1131" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint32_t</span> button;</td>
      </tr>
      <tr>
        <td id="L1132" class="blob-num js-line-number" data-line-number="1132"></td>
        <td id="LC1132" class="blob-code blob-code-inner js-file-line">} VREvent_Controller_t;</td>
      </tr>
      <tr>
        <td id="L1133" class="blob-num js-line-number" data-line-number="1133"></td>
        <td id="LC1133" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L1134" class="blob-num js-line-number" data-line-number="1134"></td>
        <td id="LC1134" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">struct</span> VREvent_Mouse_t</td>
      </tr>
      <tr>
        <td id="L1135" class="blob-num js-line-number" data-line-number="1135"></td>
        <td id="LC1135" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L1136" class="blob-num js-line-number" data-line-number="1136"></td>
        <td id="LC1136" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">float</span> x;</td>
      </tr>
      <tr>
        <td id="L1137" class="blob-num js-line-number" data-line-number="1137"></td>
        <td id="LC1137" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">float</span> y;</td>
      </tr>
      <tr>
        <td id="L1138" class="blob-num js-line-number" data-line-number="1138"></td>
        <td id="LC1138" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint32_t</span> button;</td>
      </tr>
      <tr>
        <td id="L1139" class="blob-num js-line-number" data-line-number="1139"></td>
        <td id="LC1139" class="blob-code blob-code-inner js-file-line">} VREvent_Mouse_t;</td>
      </tr>
      <tr>
        <td id="L1140" class="blob-num js-line-number" data-line-number="1140"></td>
        <td id="LC1140" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L1141" class="blob-num js-line-number" data-line-number="1141"></td>
        <td id="LC1141" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">struct</span> VREvent_Scroll_t</td>
      </tr>
      <tr>
        <td id="L1142" class="blob-num js-line-number" data-line-number="1142"></td>
        <td id="LC1142" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L1143" class="blob-num js-line-number" data-line-number="1143"></td>
        <td id="LC1143" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">float</span> xdelta;</td>
      </tr>
      <tr>
        <td id="L1144" class="blob-num js-line-number" data-line-number="1144"></td>
        <td id="LC1144" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">float</span> ydelta;</td>
      </tr>
      <tr>
        <td id="L1145" class="blob-num js-line-number" data-line-number="1145"></td>
        <td id="LC1145" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint32_t</span> repeatCount;</td>
      </tr>
      <tr>
        <td id="L1146" class="blob-num js-line-number" data-line-number="1146"></td>
        <td id="LC1146" class="blob-code blob-code-inner js-file-line">} VREvent_Scroll_t;</td>
      </tr>
      <tr>
        <td id="L1147" class="blob-num js-line-number" data-line-number="1147"></td>
        <td id="LC1147" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L1148" class="blob-num js-line-number" data-line-number="1148"></td>
        <td id="LC1148" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">struct</span> VREvent_TouchPadMove_t</td>
      </tr>
      <tr>
        <td id="L1149" class="blob-num js-line-number" data-line-number="1149"></td>
        <td id="LC1149" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L1150" class="blob-num js-line-number" data-line-number="1150"></td>
        <td id="LC1150" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">bool</span> bFingerDown;</td>
      </tr>
      <tr>
        <td id="L1151" class="blob-num js-line-number" data-line-number="1151"></td>
        <td id="LC1151" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">float</span> flSecondsFingerDown;</td>
      </tr>
      <tr>
        <td id="L1152" class="blob-num js-line-number" data-line-number="1152"></td>
        <td id="LC1152" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">float</span> fValueXFirst;</td>
      </tr>
      <tr>
        <td id="L1153" class="blob-num js-line-number" data-line-number="1153"></td>
        <td id="LC1153" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">float</span> fValueYFirst;</td>
      </tr>
      <tr>
        <td id="L1154" class="blob-num js-line-number" data-line-number="1154"></td>
        <td id="LC1154" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">float</span> fValueXRaw;</td>
      </tr>
      <tr>
        <td id="L1155" class="blob-num js-line-number" data-line-number="1155"></td>
        <td id="LC1155" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">float</span> fValueYRaw;</td>
      </tr>
      <tr>
        <td id="L1156" class="blob-num js-line-number" data-line-number="1156"></td>
        <td id="LC1156" class="blob-code blob-code-inner js-file-line">} VREvent_TouchPadMove_t;</td>
      </tr>
      <tr>
        <td id="L1157" class="blob-num js-line-number" data-line-number="1157"></td>
        <td id="LC1157" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L1158" class="blob-num js-line-number" data-line-number="1158"></td>
        <td id="LC1158" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">struct</span> VREvent_Notification_t</td>
      </tr>
      <tr>
        <td id="L1159" class="blob-num js-line-number" data-line-number="1159"></td>
        <td id="LC1159" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L1160" class="blob-num js-line-number" data-line-number="1160"></td>
        <td id="LC1160" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint64_t</span> ulUserValue;</td>
      </tr>
      <tr>
        <td id="L1161" class="blob-num js-line-number" data-line-number="1161"></td>
        <td id="LC1161" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint32_t</span> notificationId;</td>
      </tr>
      <tr>
        <td id="L1162" class="blob-num js-line-number" data-line-number="1162"></td>
        <td id="LC1162" class="blob-code blob-code-inner js-file-line">} VREvent_Notification_t;</td>
      </tr>
      <tr>
        <td id="L1163" class="blob-num js-line-number" data-line-number="1163"></td>
        <td id="LC1163" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L1164" class="blob-num js-line-number" data-line-number="1164"></td>
        <td id="LC1164" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">struct</span> VREvent_Process_t</td>
      </tr>
      <tr>
        <td id="L1165" class="blob-num js-line-number" data-line-number="1165"></td>
        <td id="LC1165" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L1166" class="blob-num js-line-number" data-line-number="1166"></td>
        <td id="LC1166" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint32_t</span> pid;</td>
      </tr>
      <tr>
        <td id="L1167" class="blob-num js-line-number" data-line-number="1167"></td>
        <td id="LC1167" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint32_t</span> oldPid;</td>
      </tr>
      <tr>
        <td id="L1168" class="blob-num js-line-number" data-line-number="1168"></td>
        <td id="LC1168" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">bool</span> bForced;</td>
      </tr>
      <tr>
        <td id="L1169" class="blob-num js-line-number" data-line-number="1169"></td>
        <td id="LC1169" class="blob-code blob-code-inner js-file-line">} VREvent_Process_t;</td>
      </tr>
      <tr>
        <td id="L1170" class="blob-num js-line-number" data-line-number="1170"></td>
        <td id="LC1170" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L1171" class="blob-num js-line-number" data-line-number="1171"></td>
        <td id="LC1171" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">struct</span> VREvent_Overlay_t</td>
      </tr>
      <tr>
        <td id="L1172" class="blob-num js-line-number" data-line-number="1172"></td>
        <td id="LC1172" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L1173" class="blob-num js-line-number" data-line-number="1173"></td>
        <td id="LC1173" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint64_t</span> overlayHandle;</td>
      </tr>
      <tr>
        <td id="L1174" class="blob-num js-line-number" data-line-number="1174"></td>
        <td id="LC1174" class="blob-code blob-code-inner js-file-line">} VREvent_Overlay_t;</td>
      </tr>
      <tr>
        <td id="L1175" class="blob-num js-line-number" data-line-number="1175"></td>
        <td id="LC1175" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L1176" class="blob-num js-line-number" data-line-number="1176"></td>
        <td id="LC1176" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">struct</span> VREvent_Status_t</td>
      </tr>
      <tr>
        <td id="L1177" class="blob-num js-line-number" data-line-number="1177"></td>
        <td id="LC1177" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L1178" class="blob-num js-line-number" data-line-number="1178"></td>
        <td id="LC1178" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint32_t</span> statusState;</td>
      </tr>
      <tr>
        <td id="L1179" class="blob-num js-line-number" data-line-number="1179"></td>
        <td id="LC1179" class="blob-code blob-code-inner js-file-line">} VREvent_Status_t;</td>
      </tr>
      <tr>
        <td id="L1180" class="blob-num js-line-number" data-line-number="1180"></td>
        <td id="LC1180" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L1181" class="blob-num js-line-number" data-line-number="1181"></td>
        <td id="LC1181" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">struct</span> VREvent_Keyboard_t</td>
      </tr>
      <tr>
        <td id="L1182" class="blob-num js-line-number" data-line-number="1182"></td>
        <td id="LC1182" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L1183" class="blob-num js-line-number" data-line-number="1183"></td>
        <td id="LC1183" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">char</span> * cNewInput[<span class="pl-c1">8</span>]; <span class="pl-c"><span class="pl-c">//</span>char[8]</span></td>
      </tr>
      <tr>
        <td id="L1184" class="blob-num js-line-number" data-line-number="1184"></td>
        <td id="LC1184" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint64_t</span> uUserValue;</td>
      </tr>
      <tr>
        <td id="L1185" class="blob-num js-line-number" data-line-number="1185"></td>
        <td id="LC1185" class="blob-code blob-code-inner js-file-line">} VREvent_Keyboard_t;</td>
      </tr>
      <tr>
        <td id="L1186" class="blob-num js-line-number" data-line-number="1186"></td>
        <td id="LC1186" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L1187" class="blob-num js-line-number" data-line-number="1187"></td>
        <td id="LC1187" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">struct</span> VREvent_Ipd_t</td>
      </tr>
      <tr>
        <td id="L1188" class="blob-num js-line-number" data-line-number="1188"></td>
        <td id="LC1188" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L1189" class="blob-num js-line-number" data-line-number="1189"></td>
        <td id="LC1189" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">float</span> ipdMeters;</td>
      </tr>
      <tr>
        <td id="L1190" class="blob-num js-line-number" data-line-number="1190"></td>
        <td id="LC1190" class="blob-code blob-code-inner js-file-line">} VREvent_Ipd_t;</td>
      </tr>
      <tr>
        <td id="L1191" class="blob-num js-line-number" data-line-number="1191"></td>
        <td id="LC1191" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L1192" class="blob-num js-line-number" data-line-number="1192"></td>
        <td id="LC1192" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">struct</span> VREvent_Chaperone_t</td>
      </tr>
      <tr>
        <td id="L1193" class="blob-num js-line-number" data-line-number="1193"></td>
        <td id="LC1193" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L1194" class="blob-num js-line-number" data-line-number="1194"></td>
        <td id="LC1194" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint64_t</span> m_nPreviousUniverse;</td>
      </tr>
      <tr>
        <td id="L1195" class="blob-num js-line-number" data-line-number="1195"></td>
        <td id="LC1195" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint64_t</span> m_nCurrentUniverse;</td>
      </tr>
      <tr>
        <td id="L1196" class="blob-num js-line-number" data-line-number="1196"></td>
        <td id="LC1196" class="blob-code blob-code-inner js-file-line">} VREvent_Chaperone_t;</td>
      </tr>
      <tr>
        <td id="L1197" class="blob-num js-line-number" data-line-number="1197"></td>
        <td id="LC1197" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L1198" class="blob-num js-line-number" data-line-number="1198"></td>
        <td id="LC1198" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">struct</span> VREvent_Reserved_t</td>
      </tr>
      <tr>
        <td id="L1199" class="blob-num js-line-number" data-line-number="1199"></td>
        <td id="LC1199" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L1200" class="blob-num js-line-number" data-line-number="1200"></td>
        <td id="LC1200" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint64_t</span> reserved0;</td>
      </tr>
      <tr>
        <td id="L1201" class="blob-num js-line-number" data-line-number="1201"></td>
        <td id="LC1201" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint64_t</span> reserved1;</td>
      </tr>
      <tr>
        <td id="L1202" class="blob-num js-line-number" data-line-number="1202"></td>
        <td id="LC1202" class="blob-code blob-code-inner js-file-line">} VREvent_Reserved_t;</td>
      </tr>
      <tr>
        <td id="L1203" class="blob-num js-line-number" data-line-number="1203"></td>
        <td id="LC1203" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L1204" class="blob-num js-line-number" data-line-number="1204"></td>
        <td id="LC1204" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">struct</span> VREvent_PerformanceTest_t</td>
      </tr>
      <tr>
        <td id="L1205" class="blob-num js-line-number" data-line-number="1205"></td>
        <td id="LC1205" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L1206" class="blob-num js-line-number" data-line-number="1206"></td>
        <td id="LC1206" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint32_t</span> m_nFidelityLevel;</td>
      </tr>
      <tr>
        <td id="L1207" class="blob-num js-line-number" data-line-number="1207"></td>
        <td id="LC1207" class="blob-code blob-code-inner js-file-line">} VREvent_PerformanceTest_t;</td>
      </tr>
      <tr>
        <td id="L1208" class="blob-num js-line-number" data-line-number="1208"></td>
        <td id="LC1208" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L1209" class="blob-num js-line-number" data-line-number="1209"></td>
        <td id="LC1209" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">struct</span> VREvent_SeatedZeroPoseReset_t</td>
      </tr>
      <tr>
        <td id="L1210" class="blob-num js-line-number" data-line-number="1210"></td>
        <td id="LC1210" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L1211" class="blob-num js-line-number" data-line-number="1211"></td>
        <td id="LC1211" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">bool</span> bResetBySystemMenu;</td>
      </tr>
      <tr>
        <td id="L1212" class="blob-num js-line-number" data-line-number="1212"></td>
        <td id="LC1212" class="blob-code blob-code-inner js-file-line">} VREvent_SeatedZeroPoseReset_t;</td>
      </tr>
      <tr>
        <td id="L1213" class="blob-num js-line-number" data-line-number="1213"></td>
        <td id="LC1213" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L1214" class="blob-num js-line-number" data-line-number="1214"></td>
        <td id="LC1214" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">struct</span> VREvent_Screenshot_t</td>
      </tr>
      <tr>
        <td id="L1215" class="blob-num js-line-number" data-line-number="1215"></td>
        <td id="LC1215" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L1216" class="blob-num js-line-number" data-line-number="1216"></td>
        <td id="LC1216" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint32_t</span> handle;</td>
      </tr>
      <tr>
        <td id="L1217" class="blob-num js-line-number" data-line-number="1217"></td>
        <td id="LC1217" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint32_t</span> type;</td>
      </tr>
      <tr>
        <td id="L1218" class="blob-num js-line-number" data-line-number="1218"></td>
        <td id="LC1218" class="blob-code blob-code-inner js-file-line">} VREvent_Screenshot_t;</td>
      </tr>
      <tr>
        <td id="L1219" class="blob-num js-line-number" data-line-number="1219"></td>
        <td id="LC1219" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L1220" class="blob-num js-line-number" data-line-number="1220"></td>
        <td id="LC1220" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">struct</span> VREvent_ScreenshotProgress_t</td>
      </tr>
      <tr>
        <td id="L1221" class="blob-num js-line-number" data-line-number="1221"></td>
        <td id="LC1221" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L1222" class="blob-num js-line-number" data-line-number="1222"></td>
        <td id="LC1222" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">float</span> progress;</td>
      </tr>
      <tr>
        <td id="L1223" class="blob-num js-line-number" data-line-number="1223"></td>
        <td id="LC1223" class="blob-code blob-code-inner js-file-line">} VREvent_ScreenshotProgress_t;</td>
      </tr>
      <tr>
        <td id="L1224" class="blob-num js-line-number" data-line-number="1224"></td>
        <td id="LC1224" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L1225" class="blob-num js-line-number" data-line-number="1225"></td>
        <td id="LC1225" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">struct</span> VREvent_ApplicationLaunch_t</td>
      </tr>
      <tr>
        <td id="L1226" class="blob-num js-line-number" data-line-number="1226"></td>
        <td id="LC1226" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L1227" class="blob-num js-line-number" data-line-number="1227"></td>
        <td id="LC1227" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint32_t</span> pid;</td>
      </tr>
      <tr>
        <td id="L1228" class="blob-num js-line-number" data-line-number="1228"></td>
        <td id="LC1228" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint32_t</span> unArgsHandle;</td>
      </tr>
      <tr>
        <td id="L1229" class="blob-num js-line-number" data-line-number="1229"></td>
        <td id="LC1229" class="blob-code blob-code-inner js-file-line">} VREvent_ApplicationLaunch_t;</td>
      </tr>
      <tr>
        <td id="L1230" class="blob-num js-line-number" data-line-number="1230"></td>
        <td id="LC1230" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L1231" class="blob-num js-line-number" data-line-number="1231"></td>
        <td id="LC1231" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">struct</span> VREvent_EditingCameraSurface_t</td>
      </tr>
      <tr>
        <td id="L1232" class="blob-num js-line-number" data-line-number="1232"></td>
        <td id="LC1232" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L1233" class="blob-num js-line-number" data-line-number="1233"></td>
        <td id="LC1233" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint64_t</span> overlayHandle;</td>
      </tr>
      <tr>
        <td id="L1234" class="blob-num js-line-number" data-line-number="1234"></td>
        <td id="LC1234" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint32_t</span> nVisualMode;</td>
      </tr>
      <tr>
        <td id="L1235" class="blob-num js-line-number" data-line-number="1235"></td>
        <td id="LC1235" class="blob-code blob-code-inner js-file-line">} VREvent_EditingCameraSurface_t;</td>
      </tr>
      <tr>
        <td id="L1236" class="blob-num js-line-number" data-line-number="1236"></td>
        <td id="LC1236" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L1237" class="blob-num js-line-number" data-line-number="1237"></td>
        <td id="LC1237" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">struct</span> VREvent_MessageOverlay_t</td>
      </tr>
      <tr>
        <td id="L1238" class="blob-num js-line-number" data-line-number="1238"></td>
        <td id="LC1238" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L1239" class="blob-num js-line-number" data-line-number="1239"></td>
        <td id="LC1239" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint32_t</span> unVRMessageOverlayResponse;</td>
      </tr>
      <tr>
        <td id="L1240" class="blob-num js-line-number" data-line-number="1240"></td>
        <td id="LC1240" class="blob-code blob-code-inner js-file-line">} VREvent_MessageOverlay_t;</td>
      </tr>
      <tr>
        <td id="L1241" class="blob-num js-line-number" data-line-number="1241"></td>
        <td id="LC1241" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L1242" class="blob-num js-line-number" data-line-number="1242"></td>
        <td id="LC1242" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">struct</span> HiddenAreaMesh_t</td>
      </tr>
      <tr>
        <td id="L1243" class="blob-num js-line-number" data-line-number="1243"></td>
        <td id="LC1243" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L1244" class="blob-num js-line-number" data-line-number="1244"></td>
        <td id="LC1244" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">struct</span> HmdVector2_t * pVertexData; <span class="pl-c"><span class="pl-c">//</span> const struct vr::HmdVector2_t *</span></td>
      </tr>
      <tr>
        <td id="L1245" class="blob-num js-line-number" data-line-number="1245"></td>
        <td id="LC1245" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint32_t</span> unTriangleCount;</td>
      </tr>
      <tr>
        <td id="L1246" class="blob-num js-line-number" data-line-number="1246"></td>
        <td id="LC1246" class="blob-code blob-code-inner js-file-line">} HiddenAreaMesh_t;</td>
      </tr>
      <tr>
        <td id="L1247" class="blob-num js-line-number" data-line-number="1247"></td>
        <td id="LC1247" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L1248" class="blob-num js-line-number" data-line-number="1248"></td>
        <td id="LC1248" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">struct</span> VRControllerAxis_t</td>
      </tr>
      <tr>
        <td id="L1249" class="blob-num js-line-number" data-line-number="1249"></td>
        <td id="LC1249" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L1250" class="blob-num js-line-number" data-line-number="1250"></td>
        <td id="LC1250" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">float</span> x;</td>
      </tr>
      <tr>
        <td id="L1251" class="blob-num js-line-number" data-line-number="1251"></td>
        <td id="LC1251" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">float</span> y;</td>
      </tr>
      <tr>
        <td id="L1252" class="blob-num js-line-number" data-line-number="1252"></td>
        <td id="LC1252" class="blob-code blob-code-inner js-file-line">} VRControllerAxis_t;</td>
      </tr>
      <tr>
        <td id="L1253" class="blob-num js-line-number" data-line-number="1253"></td>
        <td id="LC1253" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L1254" class="blob-num js-line-number" data-line-number="1254"></td>
        <td id="LC1254" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">struct</span> VRControllerState_t</td>
      </tr>
      <tr>
        <td id="L1255" class="blob-num js-line-number" data-line-number="1255"></td>
        <td id="LC1255" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L1256" class="blob-num js-line-number" data-line-number="1256"></td>
        <td id="LC1256" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint32_t</span> unPacketNum;</td>
      </tr>
      <tr>
        <td id="L1257" class="blob-num js-line-number" data-line-number="1257"></td>
        <td id="LC1257" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint64_t</span> ulButtonPressed;</td>
      </tr>
      <tr>
        <td id="L1258" class="blob-num js-line-number" data-line-number="1258"></td>
        <td id="LC1258" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint64_t</span> ulButtonTouched;</td>
      </tr>
      <tr>
        <td id="L1259" class="blob-num js-line-number" data-line-number="1259"></td>
        <td id="LC1259" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">struct</span> VRControllerAxis_t rAxis[<span class="pl-c1">5</span>]; <span class="pl-c"><span class="pl-c">//</span>struct vr::VRControllerAxis_t[5]</span></td>
      </tr>
      <tr>
        <td id="L1260" class="blob-num js-line-number" data-line-number="1260"></td>
        <td id="LC1260" class="blob-code blob-code-inner js-file-line">} VRControllerState_t;</td>
      </tr>
      <tr>
        <td id="L1261" class="blob-num js-line-number" data-line-number="1261"></td>
        <td id="LC1261" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L1262" class="blob-num js-line-number" data-line-number="1262"></td>
        <td id="LC1262" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">struct</span> Compositor_OverlaySettings</td>
      </tr>
      <tr>
        <td id="L1263" class="blob-num js-line-number" data-line-number="1263"></td>
        <td id="LC1263" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L1264" class="blob-num js-line-number" data-line-number="1264"></td>
        <td id="LC1264" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint32_t</span> size;</td>
      </tr>
      <tr>
        <td id="L1265" class="blob-num js-line-number" data-line-number="1265"></td>
        <td id="LC1265" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">bool</span> curved;</td>
      </tr>
      <tr>
        <td id="L1266" class="blob-num js-line-number" data-line-number="1266"></td>
        <td id="LC1266" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">bool</span> antialias;</td>
      </tr>
      <tr>
        <td id="L1267" class="blob-num js-line-number" data-line-number="1267"></td>
        <td id="LC1267" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">float</span> scale;</td>
      </tr>
      <tr>
        <td id="L1268" class="blob-num js-line-number" data-line-number="1268"></td>
        <td id="LC1268" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">float</span> distance;</td>
      </tr>
      <tr>
        <td id="L1269" class="blob-num js-line-number" data-line-number="1269"></td>
        <td id="LC1269" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">float</span> alpha;</td>
      </tr>
      <tr>
        <td id="L1270" class="blob-num js-line-number" data-line-number="1270"></td>
        <td id="LC1270" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">float</span> uOffset;</td>
      </tr>
      <tr>
        <td id="L1271" class="blob-num js-line-number" data-line-number="1271"></td>
        <td id="LC1271" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">float</span> vOffset;</td>
      </tr>
      <tr>
        <td id="L1272" class="blob-num js-line-number" data-line-number="1272"></td>
        <td id="LC1272" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">float</span> uScale;</td>
      </tr>
      <tr>
        <td id="L1273" class="blob-num js-line-number" data-line-number="1273"></td>
        <td id="LC1273" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">float</span> vScale;</td>
      </tr>
      <tr>
        <td id="L1274" class="blob-num js-line-number" data-line-number="1274"></td>
        <td id="LC1274" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">float</span> gridDivs;</td>
      </tr>
      <tr>
        <td id="L1275" class="blob-num js-line-number" data-line-number="1275"></td>
        <td id="LC1275" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">float</span> gridWidth;</td>
      </tr>
      <tr>
        <td id="L1276" class="blob-num js-line-number" data-line-number="1276"></td>
        <td id="LC1276" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">float</span> gridScale;</td>
      </tr>
      <tr>
        <td id="L1277" class="blob-num js-line-number" data-line-number="1277"></td>
        <td id="LC1277" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">struct</span> HmdMatrix44_t transform;</td>
      </tr>
      <tr>
        <td id="L1278" class="blob-num js-line-number" data-line-number="1278"></td>
        <td id="LC1278" class="blob-code blob-code-inner js-file-line">} Compositor_OverlaySettings;</td>
      </tr>
      <tr>
        <td id="L1279" class="blob-num js-line-number" data-line-number="1279"></td>
        <td id="LC1279" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L1280" class="blob-num js-line-number" data-line-number="1280"></td>
        <td id="LC1280" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">struct</span> CameraVideoStreamFrameHeader_t</td>
      </tr>
      <tr>
        <td id="L1281" class="blob-num js-line-number" data-line-number="1281"></td>
        <td id="LC1281" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L1282" class="blob-num js-line-number" data-line-number="1282"></td>
        <td id="LC1282" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">enum</span> EVRTrackedCameraFrameType eFrameType;</td>
      </tr>
      <tr>
        <td id="L1283" class="blob-num js-line-number" data-line-number="1283"></td>
        <td id="LC1283" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint32_t</span> nWidth;</td>
      </tr>
      <tr>
        <td id="L1284" class="blob-num js-line-number" data-line-number="1284"></td>
        <td id="LC1284" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint32_t</span> nHeight;</td>
      </tr>
      <tr>
        <td id="L1285" class="blob-num js-line-number" data-line-number="1285"></td>
        <td id="LC1285" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint32_t</span> nBytesPerPixel;</td>
      </tr>
      <tr>
        <td id="L1286" class="blob-num js-line-number" data-line-number="1286"></td>
        <td id="LC1286" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint32_t</span> nFrameSequence;</td>
      </tr>
      <tr>
        <td id="L1287" class="blob-num js-line-number" data-line-number="1287"></td>
        <td id="LC1287" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">struct</span> TrackedDevicePose_t standingTrackedDevicePose;</td>
      </tr>
      <tr>
        <td id="L1288" class="blob-num js-line-number" data-line-number="1288"></td>
        <td id="LC1288" class="blob-code blob-code-inner js-file-line">} CameraVideoStreamFrameHeader_t;</td>
      </tr>
      <tr>
        <td id="L1289" class="blob-num js-line-number" data-line-number="1289"></td>
        <td id="LC1289" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L1290" class="blob-num js-line-number" data-line-number="1290"></td>
        <td id="LC1290" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">struct</span> AppOverrideKeys_t</td>
      </tr>
      <tr>
        <td id="L1291" class="blob-num js-line-number" data-line-number="1291"></td>
        <td id="LC1291" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L1292" class="blob-num js-line-number" data-line-number="1292"></td>
        <td id="LC1292" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">char</span> * pchKey; <span class="pl-c"><span class="pl-c">//</span> const char *</span></td>
      </tr>
      <tr>
        <td id="L1293" class="blob-num js-line-number" data-line-number="1293"></td>
        <td id="LC1293" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">char</span> * pchValue; <span class="pl-c"><span class="pl-c">//</span> const char *</span></td>
      </tr>
      <tr>
        <td id="L1294" class="blob-num js-line-number" data-line-number="1294"></td>
        <td id="LC1294" class="blob-code blob-code-inner js-file-line">} AppOverrideKeys_t;</td>
      </tr>
      <tr>
        <td id="L1295" class="blob-num js-line-number" data-line-number="1295"></td>
        <td id="LC1295" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L1296" class="blob-num js-line-number" data-line-number="1296"></td>
        <td id="LC1296" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">struct</span> Compositor_FrameTiming</td>
      </tr>
      <tr>
        <td id="L1297" class="blob-num js-line-number" data-line-number="1297"></td>
        <td id="LC1297" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L1298" class="blob-num js-line-number" data-line-number="1298"></td>
        <td id="LC1298" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint32_t</span> m_nSize;</td>
      </tr>
      <tr>
        <td id="L1299" class="blob-num js-line-number" data-line-number="1299"></td>
        <td id="LC1299" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint32_t</span> m_nFrameIndex;</td>
      </tr>
      <tr>
        <td id="L1300" class="blob-num js-line-number" data-line-number="1300"></td>
        <td id="LC1300" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint32_t</span> m_nNumFramePresents;</td>
      </tr>
      <tr>
        <td id="L1301" class="blob-num js-line-number" data-line-number="1301"></td>
        <td id="LC1301" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint32_t</span> m_nNumMisPresented;</td>
      </tr>
      <tr>
        <td id="L1302" class="blob-num js-line-number" data-line-number="1302"></td>
        <td id="LC1302" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint32_t</span> m_nNumDroppedFrames;</td>
      </tr>
      <tr>
        <td id="L1303" class="blob-num js-line-number" data-line-number="1303"></td>
        <td id="LC1303" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint32_t</span> m_nReprojectionFlags;</td>
      </tr>
      <tr>
        <td id="L1304" class="blob-num js-line-number" data-line-number="1304"></td>
        <td id="LC1304" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">double</span> m_flSystemTimeInSeconds;</td>
      </tr>
      <tr>
        <td id="L1305" class="blob-num js-line-number" data-line-number="1305"></td>
        <td id="LC1305" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">float</span> m_flPreSubmitGpuMs;</td>
      </tr>
      <tr>
        <td id="L1306" class="blob-num js-line-number" data-line-number="1306"></td>
        <td id="LC1306" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">float</span> m_flPostSubmitGpuMs;</td>
      </tr>
      <tr>
        <td id="L1307" class="blob-num js-line-number" data-line-number="1307"></td>
        <td id="LC1307" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">float</span> m_flTotalRenderGpuMs;</td>
      </tr>
      <tr>
        <td id="L1308" class="blob-num js-line-number" data-line-number="1308"></td>
        <td id="LC1308" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">float</span> m_flCompositorRenderGpuMs;</td>
      </tr>
      <tr>
        <td id="L1309" class="blob-num js-line-number" data-line-number="1309"></td>
        <td id="LC1309" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">float</span> m_flCompositorRenderCpuMs;</td>
      </tr>
      <tr>
        <td id="L1310" class="blob-num js-line-number" data-line-number="1310"></td>
        <td id="LC1310" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">float</span> m_flCompositorIdleCpuMs;</td>
      </tr>
      <tr>
        <td id="L1311" class="blob-num js-line-number" data-line-number="1311"></td>
        <td id="LC1311" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">float</span> m_flClientFrameIntervalMs;</td>
      </tr>
      <tr>
        <td id="L1312" class="blob-num js-line-number" data-line-number="1312"></td>
        <td id="LC1312" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">float</span> m_flPresentCallCpuMs;</td>
      </tr>
      <tr>
        <td id="L1313" class="blob-num js-line-number" data-line-number="1313"></td>
        <td id="LC1313" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">float</span> m_flWaitForPresentCpuMs;</td>
      </tr>
      <tr>
        <td id="L1314" class="blob-num js-line-number" data-line-number="1314"></td>
        <td id="LC1314" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">float</span> m_flSubmitFrameMs;</td>
      </tr>
      <tr>
        <td id="L1315" class="blob-num js-line-number" data-line-number="1315"></td>
        <td id="LC1315" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">float</span> m_flWaitGetPosesCalledMs;</td>
      </tr>
      <tr>
        <td id="L1316" class="blob-num js-line-number" data-line-number="1316"></td>
        <td id="LC1316" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">float</span> m_flNewPosesReadyMs;</td>
      </tr>
      <tr>
        <td id="L1317" class="blob-num js-line-number" data-line-number="1317"></td>
        <td id="LC1317" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">float</span> m_flNewFrameReadyMs;</td>
      </tr>
      <tr>
        <td id="L1318" class="blob-num js-line-number" data-line-number="1318"></td>
        <td id="LC1318" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">float</span> m_flCompositorUpdateStartMs;</td>
      </tr>
      <tr>
        <td id="L1319" class="blob-num js-line-number" data-line-number="1319"></td>
        <td id="LC1319" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">float</span> m_flCompositorUpdateEndMs;</td>
      </tr>
      <tr>
        <td id="L1320" class="blob-num js-line-number" data-line-number="1320"></td>
        <td id="LC1320" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">float</span> m_flCompositorRenderStartMs;</td>
      </tr>
      <tr>
        <td id="L1321" class="blob-num js-line-number" data-line-number="1321"></td>
        <td id="LC1321" class="blob-code blob-code-inner js-file-line">	TrackedDevicePose_t m_HmdPose;</td>
      </tr>
      <tr>
        <td id="L1322" class="blob-num js-line-number" data-line-number="1322"></td>
        <td id="LC1322" class="blob-code blob-code-inner js-file-line">} Compositor_FrameTiming;</td>
      </tr>
      <tr>
        <td id="L1323" class="blob-num js-line-number" data-line-number="1323"></td>
        <td id="LC1323" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L1324" class="blob-num js-line-number" data-line-number="1324"></td>
        <td id="LC1324" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">struct</span> Compositor_CumulativeStats</td>
      </tr>
      <tr>
        <td id="L1325" class="blob-num js-line-number" data-line-number="1325"></td>
        <td id="LC1325" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L1326" class="blob-num js-line-number" data-line-number="1326"></td>
        <td id="LC1326" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint32_t</span> m_nPid;</td>
      </tr>
      <tr>
        <td id="L1327" class="blob-num js-line-number" data-line-number="1327"></td>
        <td id="LC1327" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint32_t</span> m_nNumFramePresents;</td>
      </tr>
      <tr>
        <td id="L1328" class="blob-num js-line-number" data-line-number="1328"></td>
        <td id="LC1328" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint32_t</span> m_nNumDroppedFrames;</td>
      </tr>
      <tr>
        <td id="L1329" class="blob-num js-line-number" data-line-number="1329"></td>
        <td id="LC1329" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint32_t</span> m_nNumReprojectedFrames;</td>
      </tr>
      <tr>
        <td id="L1330" class="blob-num js-line-number" data-line-number="1330"></td>
        <td id="LC1330" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint32_t</span> m_nNumFramePresentsOnStartup;</td>
      </tr>
      <tr>
        <td id="L1331" class="blob-num js-line-number" data-line-number="1331"></td>
        <td id="LC1331" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint32_t</span> m_nNumDroppedFramesOnStartup;</td>
      </tr>
      <tr>
        <td id="L1332" class="blob-num js-line-number" data-line-number="1332"></td>
        <td id="LC1332" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint32_t</span> m_nNumReprojectedFramesOnStartup;</td>
      </tr>
      <tr>
        <td id="L1333" class="blob-num js-line-number" data-line-number="1333"></td>
        <td id="LC1333" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint32_t</span> m_nNumLoading;</td>
      </tr>
      <tr>
        <td id="L1334" class="blob-num js-line-number" data-line-number="1334"></td>
        <td id="LC1334" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint32_t</span> m_nNumFramePresentsLoading;</td>
      </tr>
      <tr>
        <td id="L1335" class="blob-num js-line-number" data-line-number="1335"></td>
        <td id="LC1335" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint32_t</span> m_nNumDroppedFramesLoading;</td>
      </tr>
      <tr>
        <td id="L1336" class="blob-num js-line-number" data-line-number="1336"></td>
        <td id="LC1336" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint32_t</span> m_nNumReprojectedFramesLoading;</td>
      </tr>
      <tr>
        <td id="L1337" class="blob-num js-line-number" data-line-number="1337"></td>
        <td id="LC1337" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint32_t</span> m_nNumTimedOut;</td>
      </tr>
      <tr>
        <td id="L1338" class="blob-num js-line-number" data-line-number="1338"></td>
        <td id="LC1338" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint32_t</span> m_nNumFramePresentsTimedOut;</td>
      </tr>
      <tr>
        <td id="L1339" class="blob-num js-line-number" data-line-number="1339"></td>
        <td id="LC1339" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint32_t</span> m_nNumDroppedFramesTimedOut;</td>
      </tr>
      <tr>
        <td id="L1340" class="blob-num js-line-number" data-line-number="1340"></td>
        <td id="LC1340" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint32_t</span> m_nNumReprojectedFramesTimedOut;</td>
      </tr>
      <tr>
        <td id="L1341" class="blob-num js-line-number" data-line-number="1341"></td>
        <td id="LC1341" class="blob-code blob-code-inner js-file-line">} Compositor_CumulativeStats;</td>
      </tr>
      <tr>
        <td id="L1342" class="blob-num js-line-number" data-line-number="1342"></td>
        <td id="LC1342" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L1343" class="blob-num js-line-number" data-line-number="1343"></td>
        <td id="LC1343" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">struct</span> VROverlayIntersectionParams_t</td>
      </tr>
      <tr>
        <td id="L1344" class="blob-num js-line-number" data-line-number="1344"></td>
        <td id="LC1344" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L1345" class="blob-num js-line-number" data-line-number="1345"></td>
        <td id="LC1345" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">struct</span> HmdVector3_t vSource;</td>
      </tr>
      <tr>
        <td id="L1346" class="blob-num js-line-number" data-line-number="1346"></td>
        <td id="LC1346" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">struct</span> HmdVector3_t vDirection;</td>
      </tr>
      <tr>
        <td id="L1347" class="blob-num js-line-number" data-line-number="1347"></td>
        <td id="LC1347" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">enum</span> ETrackingUniverseOrigin eOrigin;</td>
      </tr>
      <tr>
        <td id="L1348" class="blob-num js-line-number" data-line-number="1348"></td>
        <td id="LC1348" class="blob-code blob-code-inner js-file-line">} VROverlayIntersectionParams_t;</td>
      </tr>
      <tr>
        <td id="L1349" class="blob-num js-line-number" data-line-number="1349"></td>
        <td id="LC1349" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L1350" class="blob-num js-line-number" data-line-number="1350"></td>
        <td id="LC1350" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">struct</span> VROverlayIntersectionResults_t</td>
      </tr>
      <tr>
        <td id="L1351" class="blob-num js-line-number" data-line-number="1351"></td>
        <td id="LC1351" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L1352" class="blob-num js-line-number" data-line-number="1352"></td>
        <td id="LC1352" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">struct</span> HmdVector3_t vPoint;</td>
      </tr>
      <tr>
        <td id="L1353" class="blob-num js-line-number" data-line-number="1353"></td>
        <td id="LC1353" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">struct</span> HmdVector3_t vNormal;</td>
      </tr>
      <tr>
        <td id="L1354" class="blob-num js-line-number" data-line-number="1354"></td>
        <td id="LC1354" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">struct</span> HmdVector2_t vUVs;</td>
      </tr>
      <tr>
        <td id="L1355" class="blob-num js-line-number" data-line-number="1355"></td>
        <td id="LC1355" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">float</span> fDistance;</td>
      </tr>
      <tr>
        <td id="L1356" class="blob-num js-line-number" data-line-number="1356"></td>
        <td id="LC1356" class="blob-code blob-code-inner js-file-line">} VROverlayIntersectionResults_t;</td>
      </tr>
      <tr>
        <td id="L1357" class="blob-num js-line-number" data-line-number="1357"></td>
        <td id="LC1357" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L1358" class="blob-num js-line-number" data-line-number="1358"></td>
        <td id="LC1358" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">struct</span> IntersectionMaskRectangle_t</td>
      </tr>
      <tr>
        <td id="L1359" class="blob-num js-line-number" data-line-number="1359"></td>
        <td id="LC1359" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L1360" class="blob-num js-line-number" data-line-number="1360"></td>
        <td id="LC1360" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">float</span> m_flTopLeftX;</td>
      </tr>
      <tr>
        <td id="L1361" class="blob-num js-line-number" data-line-number="1361"></td>
        <td id="LC1361" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">float</span> m_flTopLeftY;</td>
      </tr>
      <tr>
        <td id="L1362" class="blob-num js-line-number" data-line-number="1362"></td>
        <td id="LC1362" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">float</span> m_flWidth;</td>
      </tr>
      <tr>
        <td id="L1363" class="blob-num js-line-number" data-line-number="1363"></td>
        <td id="LC1363" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">float</span> m_flHeight;</td>
      </tr>
      <tr>
        <td id="L1364" class="blob-num js-line-number" data-line-number="1364"></td>
        <td id="LC1364" class="blob-code blob-code-inner js-file-line">} IntersectionMaskRectangle_t;</td>
      </tr>
      <tr>
        <td id="L1365" class="blob-num js-line-number" data-line-number="1365"></td>
        <td id="LC1365" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L1366" class="blob-num js-line-number" data-line-number="1366"></td>
        <td id="LC1366" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">struct</span> IntersectionMaskCircle_t</td>
      </tr>
      <tr>
        <td id="L1367" class="blob-num js-line-number" data-line-number="1367"></td>
        <td id="LC1367" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L1368" class="blob-num js-line-number" data-line-number="1368"></td>
        <td id="LC1368" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">float</span> m_flCenterX;</td>
      </tr>
      <tr>
        <td id="L1369" class="blob-num js-line-number" data-line-number="1369"></td>
        <td id="LC1369" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">float</span> m_flCenterY;</td>
      </tr>
      <tr>
        <td id="L1370" class="blob-num js-line-number" data-line-number="1370"></td>
        <td id="LC1370" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">float</span> m_flRadius;</td>
      </tr>
      <tr>
        <td id="L1371" class="blob-num js-line-number" data-line-number="1371"></td>
        <td id="LC1371" class="blob-code blob-code-inner js-file-line">} IntersectionMaskCircle_t;</td>
      </tr>
      <tr>
        <td id="L1372" class="blob-num js-line-number" data-line-number="1372"></td>
        <td id="LC1372" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L1373" class="blob-num js-line-number" data-line-number="1373"></td>
        <td id="LC1373" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">struct</span> RenderModel_ComponentState_t</td>
      </tr>
      <tr>
        <td id="L1374" class="blob-num js-line-number" data-line-number="1374"></td>
        <td id="LC1374" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L1375" class="blob-num js-line-number" data-line-number="1375"></td>
        <td id="LC1375" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">struct</span> HmdMatrix34_t mTrackingToComponentRenderModel;</td>
      </tr>
      <tr>
        <td id="L1376" class="blob-num js-line-number" data-line-number="1376"></td>
        <td id="LC1376" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">struct</span> HmdMatrix34_t mTrackingToComponentLocal;</td>
      </tr>
      <tr>
        <td id="L1377" class="blob-num js-line-number" data-line-number="1377"></td>
        <td id="LC1377" class="blob-code blob-code-inner js-file-line">	VRComponentProperties uProperties;</td>
      </tr>
      <tr>
        <td id="L1378" class="blob-num js-line-number" data-line-number="1378"></td>
        <td id="LC1378" class="blob-code blob-code-inner js-file-line">} RenderModel_ComponentState_t;</td>
      </tr>
      <tr>
        <td id="L1379" class="blob-num js-line-number" data-line-number="1379"></td>
        <td id="LC1379" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L1380" class="blob-num js-line-number" data-line-number="1380"></td>
        <td id="LC1380" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">struct</span> RenderModel_Vertex_t</td>
      </tr>
      <tr>
        <td id="L1381" class="blob-num js-line-number" data-line-number="1381"></td>
        <td id="LC1381" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L1382" class="blob-num js-line-number" data-line-number="1382"></td>
        <td id="LC1382" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">struct</span> HmdVector3_t vPosition;</td>
      </tr>
      <tr>
        <td id="L1383" class="blob-num js-line-number" data-line-number="1383"></td>
        <td id="LC1383" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">struct</span> HmdVector3_t vNormal;</td>
      </tr>
      <tr>
        <td id="L1384" class="blob-num js-line-number" data-line-number="1384"></td>
        <td id="LC1384" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">float</span> rfTextureCoord[<span class="pl-c1">2</span>]; <span class="pl-c"><span class="pl-c">//</span>float[2]</span></td>
      </tr>
      <tr>
        <td id="L1385" class="blob-num js-line-number" data-line-number="1385"></td>
        <td id="LC1385" class="blob-code blob-code-inner js-file-line">} RenderModel_Vertex_t;</td>
      </tr>
      <tr>
        <td id="L1386" class="blob-num js-line-number" data-line-number="1386"></td>
        <td id="LC1386" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L1387" class="blob-num js-line-number" data-line-number="1387"></td>
        <td id="LC1387" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">struct</span> RenderModel_TextureMap_t</td>
      </tr>
      <tr>
        <td id="L1388" class="blob-num js-line-number" data-line-number="1388"></td>
        <td id="LC1388" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L1389" class="blob-num js-line-number" data-line-number="1389"></td>
        <td id="LC1389" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint16_t</span> unWidth;</td>
      </tr>
      <tr>
        <td id="L1390" class="blob-num js-line-number" data-line-number="1390"></td>
        <td id="LC1390" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint16_t</span> unHeight;</td>
      </tr>
      <tr>
        <td id="L1391" class="blob-num js-line-number" data-line-number="1391"></td>
        <td id="LC1391" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint8_t</span> * rubTextureMapData; <span class="pl-c"><span class="pl-c">//</span> const uint8_t *</span></td>
      </tr>
      <tr>
        <td id="L1392" class="blob-num js-line-number" data-line-number="1392"></td>
        <td id="LC1392" class="blob-code blob-code-inner js-file-line">} RenderModel_TextureMap_t;</td>
      </tr>
      <tr>
        <td id="L1393" class="blob-num js-line-number" data-line-number="1393"></td>
        <td id="LC1393" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L1394" class="blob-num js-line-number" data-line-number="1394"></td>
        <td id="LC1394" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">struct</span> RenderModel_t</td>
      </tr>
      <tr>
        <td id="L1395" class="blob-num js-line-number" data-line-number="1395"></td>
        <td id="LC1395" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L1396" class="blob-num js-line-number" data-line-number="1396"></td>
        <td id="LC1396" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">struct</span> RenderModel_Vertex_t * rVertexData; <span class="pl-c"><span class="pl-c">//</span> const struct vr::RenderModel_Vertex_t *</span></td>
      </tr>
      <tr>
        <td id="L1397" class="blob-num js-line-number" data-line-number="1397"></td>
        <td id="LC1397" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint32_t</span> unVertexCount;</td>
      </tr>
      <tr>
        <td id="L1398" class="blob-num js-line-number" data-line-number="1398"></td>
        <td id="LC1398" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint16_t</span> * rIndexData; <span class="pl-c"><span class="pl-c">//</span> const uint16_t *</span></td>
      </tr>
      <tr>
        <td id="L1399" class="blob-num js-line-number" data-line-number="1399"></td>
        <td id="LC1399" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint32_t</span> unTriangleCount;</td>
      </tr>
      <tr>
        <td id="L1400" class="blob-num js-line-number" data-line-number="1400"></td>
        <td id="LC1400" class="blob-code blob-code-inner js-file-line">	TextureID_t diffuseTextureId;</td>
      </tr>
      <tr>
        <td id="L1401" class="blob-num js-line-number" data-line-number="1401"></td>
        <td id="LC1401" class="blob-code blob-code-inner js-file-line">} RenderModel_t;</td>
      </tr>
      <tr>
        <td id="L1402" class="blob-num js-line-number" data-line-number="1402"></td>
        <td id="LC1402" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L1403" class="blob-num js-line-number" data-line-number="1403"></td>
        <td id="LC1403" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">struct</span> RenderModel_ControllerMode_State_t</td>
      </tr>
      <tr>
        <td id="L1404" class="blob-num js-line-number" data-line-number="1404"></td>
        <td id="LC1404" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L1405" class="blob-num js-line-number" data-line-number="1405"></td>
        <td id="LC1405" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">bool</span> bScrollWheelVisible;</td>
      </tr>
      <tr>
        <td id="L1406" class="blob-num js-line-number" data-line-number="1406"></td>
        <td id="LC1406" class="blob-code blob-code-inner js-file-line">} RenderModel_ControllerMode_State_t;</td>
      </tr>
      <tr>
        <td id="L1407" class="blob-num js-line-number" data-line-number="1407"></td>
        <td id="LC1407" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L1408" class="blob-num js-line-number" data-line-number="1408"></td>
        <td id="LC1408" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">struct</span> NotificationBitmap_t</td>
      </tr>
      <tr>
        <td id="L1409" class="blob-num js-line-number" data-line-number="1409"></td>
        <td id="LC1409" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L1410" class="blob-num js-line-number" data-line-number="1410"></td>
        <td id="LC1410" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">void</span> * m_pImageData; <span class="pl-c"><span class="pl-c">//</span> void *</span></td>
      </tr>
      <tr>
        <td id="L1411" class="blob-num js-line-number" data-line-number="1411"></td>
        <td id="LC1411" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">int32_t</span> m_nWidth;</td>
      </tr>
      <tr>
        <td id="L1412" class="blob-num js-line-number" data-line-number="1412"></td>
        <td id="LC1412" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">int32_t</span> m_nHeight;</td>
      </tr>
      <tr>
        <td id="L1413" class="blob-num js-line-number" data-line-number="1413"></td>
        <td id="LC1413" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">int32_t</span> m_nBytesPerPixel;</td>
      </tr>
      <tr>
        <td id="L1414" class="blob-num js-line-number" data-line-number="1414"></td>
        <td id="LC1414" class="blob-code blob-code-inner js-file-line">} NotificationBitmap_t;</td>
      </tr>
      <tr>
        <td id="L1415" class="blob-num js-line-number" data-line-number="1415"></td>
        <td id="LC1415" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L1416" class="blob-num js-line-number" data-line-number="1416"></td>
        <td id="LC1416" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">struct</span> COpenVRContext</td>
      </tr>
      <tr>
        <td id="L1417" class="blob-num js-line-number" data-line-number="1417"></td>
        <td id="LC1417" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L1418" class="blob-num js-line-number" data-line-number="1418"></td>
        <td id="LC1418" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">intptr_t</span> m_pVRSystem; <span class="pl-c"><span class="pl-c">//</span> class vr::IVRSystem *</span></td>
      </tr>
      <tr>
        <td id="L1419" class="blob-num js-line-number" data-line-number="1419"></td>
        <td id="LC1419" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">intptr_t</span> m_pVRChaperone; <span class="pl-c"><span class="pl-c">//</span> class vr::IVRChaperone *</span></td>
      </tr>
      <tr>
        <td id="L1420" class="blob-num js-line-number" data-line-number="1420"></td>
        <td id="LC1420" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">intptr_t</span> m_pVRChaperoneSetup; <span class="pl-c"><span class="pl-c">//</span> class vr::IVRChaperoneSetup *</span></td>
      </tr>
      <tr>
        <td id="L1421" class="blob-num js-line-number" data-line-number="1421"></td>
        <td id="LC1421" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">intptr_t</span> m_pVRCompositor; <span class="pl-c"><span class="pl-c">//</span> class vr::IVRCompositor *</span></td>
      </tr>
      <tr>
        <td id="L1422" class="blob-num js-line-number" data-line-number="1422"></td>
        <td id="LC1422" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">intptr_t</span> m_pVROverlay; <span class="pl-c"><span class="pl-c">//</span> class vr::IVROverlay *</span></td>
      </tr>
      <tr>
        <td id="L1423" class="blob-num js-line-number" data-line-number="1423"></td>
        <td id="LC1423" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">intptr_t</span> m_pVRResources; <span class="pl-c"><span class="pl-c">//</span> class vr::IVRResources *</span></td>
      </tr>
      <tr>
        <td id="L1424" class="blob-num js-line-number" data-line-number="1424"></td>
        <td id="LC1424" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">intptr_t</span> m_pVRRenderModels; <span class="pl-c"><span class="pl-c">//</span> class vr::IVRRenderModels *</span></td>
      </tr>
      <tr>
        <td id="L1425" class="blob-num js-line-number" data-line-number="1425"></td>
        <td id="LC1425" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">intptr_t</span> m_pVRExtendedDisplay; <span class="pl-c"><span class="pl-c">//</span> class vr::IVRExtendedDisplay *</span></td>
      </tr>
      <tr>
        <td id="L1426" class="blob-num js-line-number" data-line-number="1426"></td>
        <td id="LC1426" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">intptr_t</span> m_pVRSettings; <span class="pl-c"><span class="pl-c">//</span> class vr::IVRSettings *</span></td>
      </tr>
      <tr>
        <td id="L1427" class="blob-num js-line-number" data-line-number="1427"></td>
        <td id="LC1427" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">intptr_t</span> m_pVRApplications; <span class="pl-c"><span class="pl-c">//</span> class vr::IVRApplications *</span></td>
      </tr>
      <tr>
        <td id="L1428" class="blob-num js-line-number" data-line-number="1428"></td>
        <td id="LC1428" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">intptr_t</span> m_pVRTrackedCamera; <span class="pl-c"><span class="pl-c">//</span> class vr::IVRTrackedCamera *</span></td>
      </tr>
      <tr>
        <td id="L1429" class="blob-num js-line-number" data-line-number="1429"></td>
        <td id="LC1429" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">intptr_t</span> m_pVRScreenshots; <span class="pl-c"><span class="pl-c">//</span> class vr::IVRScreenshots *</span></td>
      </tr>
      <tr>
        <td id="L1430" class="blob-num js-line-number" data-line-number="1430"></td>
        <td id="LC1430" class="blob-code blob-code-inner js-file-line">} COpenVRContext;</td>
      </tr>
      <tr>
        <td id="L1431" class="blob-num js-line-number" data-line-number="1431"></td>
        <td id="LC1431" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L1432" class="blob-num js-line-number" data-line-number="1432"></td>
        <td id="LC1432" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L1433" class="blob-num js-line-number" data-line-number="1433"></td>
        <td id="LC1433" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">union</span></td>
      </tr>
      <tr>
        <td id="L1434" class="blob-num js-line-number" data-line-number="1434"></td>
        <td id="LC1434" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L1435" class="blob-num js-line-number" data-line-number="1435"></td>
        <td id="LC1435" class="blob-code blob-code-inner js-file-line">	VREvent_Reserved_t reserved;</td>
      </tr>
      <tr>
        <td id="L1436" class="blob-num js-line-number" data-line-number="1436"></td>
        <td id="LC1436" class="blob-code blob-code-inner js-file-line">	VREvent_Controller_t controller;</td>
      </tr>
      <tr>
        <td id="L1437" class="blob-num js-line-number" data-line-number="1437"></td>
        <td id="LC1437" class="blob-code blob-code-inner js-file-line">	VREvent_Mouse_t mouse;</td>
      </tr>
      <tr>
        <td id="L1438" class="blob-num js-line-number" data-line-number="1438"></td>
        <td id="LC1438" class="blob-code blob-code-inner js-file-line">	VREvent_Scroll_t scroll;</td>
      </tr>
      <tr>
        <td id="L1439" class="blob-num js-line-number" data-line-number="1439"></td>
        <td id="LC1439" class="blob-code blob-code-inner js-file-line">	VREvent_Process_t process;</td>
      </tr>
      <tr>
        <td id="L1440" class="blob-num js-line-number" data-line-number="1440"></td>
        <td id="LC1440" class="blob-code blob-code-inner js-file-line">	VREvent_Notification_t notification;</td>
      </tr>
      <tr>
        <td id="L1441" class="blob-num js-line-number" data-line-number="1441"></td>
        <td id="LC1441" class="blob-code blob-code-inner js-file-line">	VREvent_Overlay_t overlay;</td>
      </tr>
      <tr>
        <td id="L1442" class="blob-num js-line-number" data-line-number="1442"></td>
        <td id="LC1442" class="blob-code blob-code-inner js-file-line">	VREvent_Status_t status;</td>
      </tr>
      <tr>
        <td id="L1443" class="blob-num js-line-number" data-line-number="1443"></td>
        <td id="LC1443" class="blob-code blob-code-inner js-file-line">	VREvent_Keyboard_t keyboard;</td>
      </tr>
      <tr>
        <td id="L1444" class="blob-num js-line-number" data-line-number="1444"></td>
        <td id="LC1444" class="blob-code blob-code-inner js-file-line">	VREvent_Ipd_t ipd;</td>
      </tr>
      <tr>
        <td id="L1445" class="blob-num js-line-number" data-line-number="1445"></td>
        <td id="LC1445" class="blob-code blob-code-inner js-file-line">	VREvent_Chaperone_t chaperone;</td>
      </tr>
      <tr>
        <td id="L1446" class="blob-num js-line-number" data-line-number="1446"></td>
        <td id="LC1446" class="blob-code blob-code-inner js-file-line">	VREvent_PerformanceTest_t performanceTest;</td>
      </tr>
      <tr>
        <td id="L1447" class="blob-num js-line-number" data-line-number="1447"></td>
        <td id="LC1447" class="blob-code blob-code-inner js-file-line">	VREvent_TouchPadMove_t touchPadMove;</td>
      </tr>
      <tr>
        <td id="L1448" class="blob-num js-line-number" data-line-number="1448"></td>
        <td id="LC1448" class="blob-code blob-code-inner js-file-line">	VREvent_SeatedZeroPoseReset_t seatedZeroPoseReset;</td>
      </tr>
      <tr>
        <td id="L1449" class="blob-num js-line-number" data-line-number="1449"></td>
        <td id="LC1449" class="blob-code blob-code-inner js-file-line">} VREvent_Data_t;</td>
      </tr>
      <tr>
        <td id="L1450" class="blob-num js-line-number" data-line-number="1450"></td>
        <td id="LC1450" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L1451" class="blob-num js-line-number" data-line-number="1451"></td>
        <td id="LC1451" class="blob-code blob-code-inner js-file-line"><span class="pl-c"><span class="pl-c">/*</span>* An event posted by the server to all running applications <span class="pl-c">*/</span></span></td>
      </tr>
      <tr>
        <td id="L1452" class="blob-num js-line-number" data-line-number="1452"></td>
        <td id="LC1452" class="blob-code blob-code-inner js-file-line"><span class="pl-k">struct</span> VREvent_t</td>
      </tr>
      <tr>
        <td id="L1453" class="blob-num js-line-number" data-line-number="1453"></td>
        <td id="LC1453" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L1454" class="blob-num js-line-number" data-line-number="1454"></td>
        <td id="LC1454" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint32_t</span> eventType; <span class="pl-c"><span class="pl-c">//</span> EVREventType enum</span></td>
      </tr>
      <tr>
        <td id="L1455" class="blob-num js-line-number" data-line-number="1455"></td>
        <td id="LC1455" class="blob-code blob-code-inner js-file-line">	TrackedDeviceIndex_t trackedDeviceIndex;</td>
      </tr>
      <tr>
        <td id="L1456" class="blob-num js-line-number" data-line-number="1456"></td>
        <td id="LC1456" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">float</span> eventAgeSeconds;</td>
      </tr>
      <tr>
        <td id="L1457" class="blob-num js-line-number" data-line-number="1457"></td>
        <td id="LC1457" class="blob-code blob-code-inner js-file-line">	<span class="pl-c"><span class="pl-c">//</span> event data must be the end of the struct as its size is variable</span></td>
      </tr>
      <tr>
        <td id="L1458" class="blob-num js-line-number" data-line-number="1458"></td>
        <td id="LC1458" class="blob-code blob-code-inner js-file-line">	VREvent_Data_t data;</td>
      </tr>
      <tr>
        <td id="L1459" class="blob-num js-line-number" data-line-number="1459"></td>
        <td id="LC1459" class="blob-code blob-code-inner js-file-line">};</td>
      </tr>
      <tr>
        <td id="L1460" class="blob-num js-line-number" data-line-number="1460"></td>
        <td id="LC1460" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L1461" class="blob-num js-line-number" data-line-number="1461"></td>
        <td id="LC1461" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L1462" class="blob-num js-line-number" data-line-number="1462"></td>
        <td id="LC1462" class="blob-code blob-code-inner js-file-line"><span class="pl-k">typedef</span> <span class="pl-k">union</span></td>
      </tr>
      <tr>
        <td id="L1463" class="blob-num js-line-number" data-line-number="1463"></td>
        <td id="LC1463" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L1464" class="blob-num js-line-number" data-line-number="1464"></td>
        <td id="LC1464" class="blob-code blob-code-inner js-file-line">	IntersectionMaskRectangle_t m_Rectangle;</td>
      </tr>
      <tr>
        <td id="L1465" class="blob-num js-line-number" data-line-number="1465"></td>
        <td id="LC1465" class="blob-code blob-code-inner js-file-line">	IntersectionMaskCircle_t m_Circle;</td>
      </tr>
      <tr>
        <td id="L1466" class="blob-num js-line-number" data-line-number="1466"></td>
        <td id="LC1466" class="blob-code blob-code-inner js-file-line">} VROverlayIntersectionMaskPrimitive_Data_t;</td>
      </tr>
      <tr>
        <td id="L1467" class="blob-num js-line-number" data-line-number="1467"></td>
        <td id="LC1467" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L1468" class="blob-num js-line-number" data-line-number="1468"></td>
        <td id="LC1468" class="blob-code blob-code-inner js-file-line"><span class="pl-k">struct</span> VROverlayIntersectionMaskPrimitive_t</td>
      </tr>
      <tr>
        <td id="L1469" class="blob-num js-line-number" data-line-number="1469"></td>
        <td id="LC1469" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L1470" class="blob-num js-line-number" data-line-number="1470"></td>
        <td id="LC1470" class="blob-code blob-code-inner js-file-line">	EVROverlayIntersectionMaskPrimitiveType m_nPrimitiveType;</td>
      </tr>
      <tr>
        <td id="L1471" class="blob-num js-line-number" data-line-number="1471"></td>
        <td id="LC1471" class="blob-code blob-code-inner js-file-line">	VROverlayIntersectionMaskPrimitive_Data_t m_Primitive;</td>
      </tr>
      <tr>
        <td id="L1472" class="blob-num js-line-number" data-line-number="1472"></td>
        <td id="LC1472" class="blob-code blob-code-inner js-file-line">};</td>
      </tr>
      <tr>
        <td id="L1473" class="blob-num js-line-number" data-line-number="1473"></td>
        <td id="LC1473" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L1474" class="blob-num js-line-number" data-line-number="1474"></td>
        <td id="LC1474" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L1475" class="blob-num js-line-number" data-line-number="1475"></td>
        <td id="LC1475" class="blob-code blob-code-inner js-file-line"><span class="pl-c"><span class="pl-c">//</span> OpenVR Function Pointer Tables</span></td>
      </tr>
      <tr>
        <td id="L1476" class="blob-num js-line-number" data-line-number="1476"></td>
        <td id="LC1476" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L1477" class="blob-num js-line-number" data-line-number="1477"></td>
        <td id="LC1477" class="blob-code blob-code-inner js-file-line"><span class="pl-k">struct</span> VR_IVRSystem_FnTable</td>
      </tr>
      <tr>
        <td id="L1478" class="blob-num js-line-number" data-line-number="1478"></td>
        <td id="LC1478" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L1479" class="blob-num js-line-number" data-line-number="1479"></td>
        <td id="LC1479" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">void</span> (OPENVR_FNTABLE_CALLTYPE *GetRecommendedRenderTargetSize)(<span class="pl-c1">uint32_t</span> * pnWidth, <span class="pl-c1">uint32_t</span> * pnHeight);</td>
      </tr>
      <tr>
        <td id="L1480" class="blob-num js-line-number" data-line-number="1480"></td>
        <td id="LC1480" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">struct</span> <span class="pl-smi">HmdMatrix44_t</span> (OPENVR_FNTABLE_CALLTYPE *GetProjectionMatrix)(EVREye eEye, <span class="pl-k">float</span> fNearZ, <span class="pl-k">float</span> fFarZ);</td>
      </tr>
      <tr>
        <td id="L1481" class="blob-num js-line-number" data-line-number="1481"></td>
        <td id="LC1481" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">void</span> (OPENVR_FNTABLE_CALLTYPE *GetProjectionRaw)(EVREye eEye, <span class="pl-k">float</span> * pfLeft, <span class="pl-k">float</span> * pfRight, <span class="pl-k">float</span> * pfTop, <span class="pl-k">float</span> * pfBottom);</td>
      </tr>
      <tr>
        <td id="L1482" class="blob-num js-line-number" data-line-number="1482"></td>
        <td id="LC1482" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">bool</span> (OPENVR_FNTABLE_CALLTYPE *ComputeDistortion)(EVREye eEye, <span class="pl-k">float</span> fU, <span class="pl-k">float</span> fV, <span class="pl-k">struct</span> DistortionCoordinates_t * pDistortionCoordinates);</td>
      </tr>
      <tr>
        <td id="L1483" class="blob-num js-line-number" data-line-number="1483"></td>
        <td id="LC1483" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">struct</span> <span class="pl-smi">HmdMatrix34_t</span> (OPENVR_FNTABLE_CALLTYPE *GetEyeToHeadTransform)(EVREye eEye);</td>
      </tr>
      <tr>
        <td id="L1484" class="blob-num js-line-number" data-line-number="1484"></td>
        <td id="LC1484" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">bool</span> (OPENVR_FNTABLE_CALLTYPE *GetTimeSinceLastVsync)(<span class="pl-k">float</span> * pfSecondsSinceLastVsync, <span class="pl-c1">uint64_t</span> * pulFrameCounter);</td>
      </tr>
      <tr>
        <td id="L1485" class="blob-num js-line-number" data-line-number="1485"></td>
        <td id="LC1485" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">int32_t</span> (OPENVR_FNTABLE_CALLTYPE *GetD3D9AdapterIndex)();</td>
      </tr>
      <tr>
        <td id="L1486" class="blob-num js-line-number" data-line-number="1486"></td>
        <td id="LC1486" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">void</span> (OPENVR_FNTABLE_CALLTYPE *GetDXGIOutputInfo)(<span class="pl-c1">int32_t</span> * pnAdapterIndex);</td>
      </tr>
      <tr>
        <td id="L1487" class="blob-num js-line-number" data-line-number="1487"></td>
        <td id="LC1487" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">bool</span> (OPENVR_FNTABLE_CALLTYPE *IsDisplayOnDesktop)();</td>
      </tr>
      <tr>
        <td id="L1488" class="blob-num js-line-number" data-line-number="1488"></td>
        <td id="LC1488" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">bool</span> (OPENVR_FNTABLE_CALLTYPE *SetDisplayVisibility)(<span class="pl-k">bool</span> bIsVisibleOnDesktop);</td>
      </tr>
      <tr>
        <td id="L1489" class="blob-num js-line-number" data-line-number="1489"></td>
        <td id="LC1489" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">void</span> (OPENVR_FNTABLE_CALLTYPE *GetDeviceToAbsoluteTrackingPose)(ETrackingUniverseOrigin eOrigin, <span class="pl-k">float</span> fPredictedSecondsToPhotonsFromNow, <span class="pl-k">struct</span> TrackedDevicePose_t * pTrackedDevicePoseArray, <span class="pl-c1">uint32_t</span> unTrackedDevicePoseArrayCount);</td>
      </tr>
      <tr>
        <td id="L1490" class="blob-num js-line-number" data-line-number="1490"></td>
        <td id="LC1490" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">void</span> (OPENVR_FNTABLE_CALLTYPE *ResetSeatedZeroPose)();</td>
      </tr>
      <tr>
        <td id="L1491" class="blob-num js-line-number" data-line-number="1491"></td>
        <td id="LC1491" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">struct</span> <span class="pl-smi">HmdMatrix34_t</span> (OPENVR_FNTABLE_CALLTYPE *GetSeatedZeroPoseToStandingAbsoluteTrackingPose)();</td>
      </tr>
      <tr>
        <td id="L1492" class="blob-num js-line-number" data-line-number="1492"></td>
        <td id="LC1492" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">struct</span> <span class="pl-smi">HmdMatrix34_t</span> (OPENVR_FNTABLE_CALLTYPE *GetRawZeroPoseToStandingAbsoluteTrackingPose)();</td>
      </tr>
      <tr>
        <td id="L1493" class="blob-num js-line-number" data-line-number="1493"></td>
        <td id="LC1493" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint32_t</span> (OPENVR_FNTABLE_CALLTYPE *GetSortedTrackedDeviceIndicesOfClass)(ETrackedDeviceClass eTrackedDeviceClass, TrackedDeviceIndex_t * punTrackedDeviceIndexArray, <span class="pl-c1">uint32_t</span> unTrackedDeviceIndexArrayCount, TrackedDeviceIndex_t unRelativeToTrackedDeviceIndex);</td>
      </tr>
      <tr>
        <td id="L1494" class="blob-num js-line-number" data-line-number="1494"></td>
        <td id="LC1494" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EDeviceActivityLevel</span> (OPENVR_FNTABLE_CALLTYPE *GetTrackedDeviceActivityLevel)(TrackedDeviceIndex_t unDeviceId);</td>
      </tr>
      <tr>
        <td id="L1495" class="blob-num js-line-number" data-line-number="1495"></td>
        <td id="LC1495" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">void</span> (OPENVR_FNTABLE_CALLTYPE *ApplyTransform)(<span class="pl-k">struct</span> TrackedDevicePose_t * pOutputPose, <span class="pl-k">struct</span> TrackedDevicePose_t * pTrackedDevicePose, <span class="pl-k">struct</span> HmdMatrix34_t * pTransform);</td>
      </tr>
      <tr>
        <td id="L1496" class="blob-num js-line-number" data-line-number="1496"></td>
        <td id="LC1496" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">TrackedDeviceIndex_t</span> (OPENVR_FNTABLE_CALLTYPE *GetTrackedDeviceIndexForControllerRole)(ETrackedControllerRole unDeviceType);</td>
      </tr>
      <tr>
        <td id="L1497" class="blob-num js-line-number" data-line-number="1497"></td>
        <td id="LC1497" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">ETrackedControllerRole</span> (OPENVR_FNTABLE_CALLTYPE *GetControllerRoleForTrackedDeviceIndex)(TrackedDeviceIndex_t unDeviceIndex);</td>
      </tr>
      <tr>
        <td id="L1498" class="blob-num js-line-number" data-line-number="1498"></td>
        <td id="LC1498" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">ETrackedDeviceClass</span> (OPENVR_FNTABLE_CALLTYPE *GetTrackedDeviceClass)(TrackedDeviceIndex_t unDeviceIndex);</td>
      </tr>
      <tr>
        <td id="L1499" class="blob-num js-line-number" data-line-number="1499"></td>
        <td id="LC1499" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">bool</span> (OPENVR_FNTABLE_CALLTYPE *IsTrackedDeviceConnected)(TrackedDeviceIndex_t unDeviceIndex);</td>
      </tr>
      <tr>
        <td id="L1500" class="blob-num js-line-number" data-line-number="1500"></td>
        <td id="LC1500" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">bool</span> (OPENVR_FNTABLE_CALLTYPE *GetBoolTrackedDeviceProperty)(TrackedDeviceIndex_t unDeviceIndex, ETrackedDeviceProperty prop, ETrackedPropertyError * pError);</td>
      </tr>
      <tr>
        <td id="L1501" class="blob-num js-line-number" data-line-number="1501"></td>
        <td id="LC1501" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">float</span> (OPENVR_FNTABLE_CALLTYPE *GetFloatTrackedDeviceProperty)(TrackedDeviceIndex_t unDeviceIndex, ETrackedDeviceProperty prop, ETrackedPropertyError * pError);</td>
      </tr>
      <tr>
        <td id="L1502" class="blob-num js-line-number" data-line-number="1502"></td>
        <td id="LC1502" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">int32_t</span> (OPENVR_FNTABLE_CALLTYPE *GetInt32TrackedDeviceProperty)(TrackedDeviceIndex_t unDeviceIndex, ETrackedDeviceProperty prop, ETrackedPropertyError * pError);</td>
      </tr>
      <tr>
        <td id="L1503" class="blob-num js-line-number" data-line-number="1503"></td>
        <td id="LC1503" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint64_t</span> (OPENVR_FNTABLE_CALLTYPE *GetUint64TrackedDeviceProperty)(TrackedDeviceIndex_t unDeviceIndex, ETrackedDeviceProperty prop, ETrackedPropertyError * pError);</td>
      </tr>
      <tr>
        <td id="L1504" class="blob-num js-line-number" data-line-number="1504"></td>
        <td id="LC1504" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">struct</span> <span class="pl-smi">HmdMatrix34_t</span> (OPENVR_FNTABLE_CALLTYPE *GetMatrix34TrackedDeviceProperty)(TrackedDeviceIndex_t unDeviceIndex, ETrackedDeviceProperty prop, ETrackedPropertyError * pError);</td>
      </tr>
      <tr>
        <td id="L1505" class="blob-num js-line-number" data-line-number="1505"></td>
        <td id="LC1505" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint32_t</span> (OPENVR_FNTABLE_CALLTYPE *GetStringTrackedDeviceProperty)(TrackedDeviceIndex_t unDeviceIndex, ETrackedDeviceProperty prop, <span class="pl-k">char</span> * pchValue, <span class="pl-c1">uint32_t</span> unBufferSize, ETrackedPropertyError * pError);</td>
      </tr>
      <tr>
        <td id="L1506" class="blob-num js-line-number" data-line-number="1506"></td>
        <td id="LC1506" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">char</span> * (OPENVR_FNTABLE_CALLTYPE *GetPropErrorNameFromEnum)(ETrackedPropertyError error);</td>
      </tr>
      <tr>
        <td id="L1507" class="blob-num js-line-number" data-line-number="1507"></td>
        <td id="LC1507" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">bool</span> (OPENVR_FNTABLE_CALLTYPE *PollNextEvent)(<span class="pl-k">struct</span> VREvent_t * pEvent, <span class="pl-c1">uint32_t</span> uncbVREvent);</td>
      </tr>
      <tr>
        <td id="L1508" class="blob-num js-line-number" data-line-number="1508"></td>
        <td id="LC1508" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">bool</span> (OPENVR_FNTABLE_CALLTYPE *PollNextEventWithPose)(ETrackingUniverseOrigin eOrigin, <span class="pl-k">struct</span> VREvent_t * pEvent, <span class="pl-c1">uint32_t</span> uncbVREvent, TrackedDevicePose_t * pTrackedDevicePose);</td>
      </tr>
      <tr>
        <td id="L1509" class="blob-num js-line-number" data-line-number="1509"></td>
        <td id="LC1509" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">char</span> * (OPENVR_FNTABLE_CALLTYPE *GetEventTypeNameFromEnum)(EVREventType eType);</td>
      </tr>
      <tr>
        <td id="L1510" class="blob-num js-line-number" data-line-number="1510"></td>
        <td id="LC1510" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">struct</span> <span class="pl-smi">HiddenAreaMesh_t</span> (OPENVR_FNTABLE_CALLTYPE *GetHiddenAreaMesh)(EVREye eEye, EHiddenAreaMeshType type);</td>
      </tr>
      <tr>
        <td id="L1511" class="blob-num js-line-number" data-line-number="1511"></td>
        <td id="LC1511" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">bool</span> (OPENVR_FNTABLE_CALLTYPE *GetControllerState)(TrackedDeviceIndex_t unControllerDeviceIndex, VRControllerState_t * pControllerState, <span class="pl-c1">uint32_t</span> unControllerStateSize);</td>
      </tr>
      <tr>
        <td id="L1512" class="blob-num js-line-number" data-line-number="1512"></td>
        <td id="LC1512" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">bool</span> (OPENVR_FNTABLE_CALLTYPE *GetControllerStateWithPose)(ETrackingUniverseOrigin eOrigin, TrackedDeviceIndex_t unControllerDeviceIndex, VRControllerState_t * pControllerState, <span class="pl-c1">uint32_t</span> unControllerStateSize, <span class="pl-k">struct</span> TrackedDevicePose_t * pTrackedDevicePose);</td>
      </tr>
      <tr>
        <td id="L1513" class="blob-num js-line-number" data-line-number="1513"></td>
        <td id="LC1513" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">void</span> (OPENVR_FNTABLE_CALLTYPE *TriggerHapticPulse)(TrackedDeviceIndex_t unControllerDeviceIndex, <span class="pl-c1">uint32_t</span> unAxisId, <span class="pl-k">unsigned</span> <span class="pl-k">short</span> usDurationMicroSec);</td>
      </tr>
      <tr>
        <td id="L1514" class="blob-num js-line-number" data-line-number="1514"></td>
        <td id="LC1514" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">char</span> * (OPENVR_FNTABLE_CALLTYPE *GetButtonIdNameFromEnum)(EVRButtonId eButtonId);</td>
      </tr>
      <tr>
        <td id="L1515" class="blob-num js-line-number" data-line-number="1515"></td>
        <td id="LC1515" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">char</span> * (OPENVR_FNTABLE_CALLTYPE *GetControllerAxisTypeNameFromEnum)(EVRControllerAxisType eAxisType);</td>
      </tr>
      <tr>
        <td id="L1516" class="blob-num js-line-number" data-line-number="1516"></td>
        <td id="LC1516" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">bool</span> (OPENVR_FNTABLE_CALLTYPE *CaptureInputFocus)();</td>
      </tr>
      <tr>
        <td id="L1517" class="blob-num js-line-number" data-line-number="1517"></td>
        <td id="LC1517" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">void</span> (OPENVR_FNTABLE_CALLTYPE *ReleaseInputFocus)();</td>
      </tr>
      <tr>
        <td id="L1518" class="blob-num js-line-number" data-line-number="1518"></td>
        <td id="LC1518" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">bool</span> (OPENVR_FNTABLE_CALLTYPE *IsInputFocusCapturedByAnotherProcess)();</td>
      </tr>
      <tr>
        <td id="L1519" class="blob-num js-line-number" data-line-number="1519"></td>
        <td id="LC1519" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint32_t</span> (OPENVR_FNTABLE_CALLTYPE *DriverDebugRequest)(TrackedDeviceIndex_t unDeviceIndex, <span class="pl-k">char</span> * pchRequest, <span class="pl-k">char</span> * pchResponseBuffer, <span class="pl-c1">uint32_t</span> unResponseBufferSize);</td>
      </tr>
      <tr>
        <td id="L1520" class="blob-num js-line-number" data-line-number="1520"></td>
        <td id="LC1520" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVRFirmwareError</span> (OPENVR_FNTABLE_CALLTYPE *PerformFirmwareUpdate)(TrackedDeviceIndex_t unDeviceIndex);</td>
      </tr>
      <tr>
        <td id="L1521" class="blob-num js-line-number" data-line-number="1521"></td>
        <td id="LC1521" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">void</span> (OPENVR_FNTABLE_CALLTYPE *AcknowledgeQuit_Exiting)();</td>
      </tr>
      <tr>
        <td id="L1522" class="blob-num js-line-number" data-line-number="1522"></td>
        <td id="LC1522" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">void</span> (OPENVR_FNTABLE_CALLTYPE *AcknowledgeQuit_UserPrompt)();</td>
      </tr>
      <tr>
        <td id="L1523" class="blob-num js-line-number" data-line-number="1523"></td>
        <td id="LC1523" class="blob-code blob-code-inner js-file-line">};</td>
      </tr>
      <tr>
        <td id="L1524" class="blob-num js-line-number" data-line-number="1524"></td>
        <td id="LC1524" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L1525" class="blob-num js-line-number" data-line-number="1525"></td>
        <td id="LC1525" class="blob-code blob-code-inner js-file-line"><span class="pl-k">struct</span> VR_IVRExtendedDisplay_FnTable</td>
      </tr>
      <tr>
        <td id="L1526" class="blob-num js-line-number" data-line-number="1526"></td>
        <td id="LC1526" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L1527" class="blob-num js-line-number" data-line-number="1527"></td>
        <td id="LC1527" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">void</span> (OPENVR_FNTABLE_CALLTYPE *GetWindowBounds)(<span class="pl-c1">int32_t</span> * pnX, <span class="pl-c1">int32_t</span> * pnY, <span class="pl-c1">uint32_t</span> * pnWidth, <span class="pl-c1">uint32_t</span> * pnHeight);</td>
      </tr>
      <tr>
        <td id="L1528" class="blob-num js-line-number" data-line-number="1528"></td>
        <td id="LC1528" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">void</span> (OPENVR_FNTABLE_CALLTYPE *GetEyeOutputViewport)(EVREye eEye, <span class="pl-c1">uint32_t</span> * pnX, <span class="pl-c1">uint32_t</span> * pnY, <span class="pl-c1">uint32_t</span> * pnWidth, <span class="pl-c1">uint32_t</span> * pnHeight);</td>
      </tr>
      <tr>
        <td id="L1529" class="blob-num js-line-number" data-line-number="1529"></td>
        <td id="LC1529" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">void</span> (OPENVR_FNTABLE_CALLTYPE *GetDXGIOutputInfo)(<span class="pl-c1">int32_t</span> * pnAdapterIndex, <span class="pl-c1">int32_t</span> * pnAdapterOutputIndex);</td>
      </tr>
      <tr>
        <td id="L1530" class="blob-num js-line-number" data-line-number="1530"></td>
        <td id="LC1530" class="blob-code blob-code-inner js-file-line">};</td>
      </tr>
      <tr>
        <td id="L1531" class="blob-num js-line-number" data-line-number="1531"></td>
        <td id="LC1531" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L1532" class="blob-num js-line-number" data-line-number="1532"></td>
        <td id="LC1532" class="blob-code blob-code-inner js-file-line"><span class="pl-k">struct</span> VR_IVRTrackedCamera_FnTable</td>
      </tr>
      <tr>
        <td id="L1533" class="blob-num js-line-number" data-line-number="1533"></td>
        <td id="LC1533" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L1534" class="blob-num js-line-number" data-line-number="1534"></td>
        <td id="LC1534" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">char</span> * (OPENVR_FNTABLE_CALLTYPE *GetCameraErrorNameFromEnum)(EVRTrackedCameraError eCameraError);</td>
      </tr>
      <tr>
        <td id="L1535" class="blob-num js-line-number" data-line-number="1535"></td>
        <td id="LC1535" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVRTrackedCameraError</span> (OPENVR_FNTABLE_CALLTYPE *HasCamera)(TrackedDeviceIndex_t nDeviceIndex, <span class="pl-k">bool</span> * pHasCamera);</td>
      </tr>
      <tr>
        <td id="L1536" class="blob-num js-line-number" data-line-number="1536"></td>
        <td id="LC1536" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVRTrackedCameraError</span> (OPENVR_FNTABLE_CALLTYPE *GetCameraFrameSize)(TrackedDeviceIndex_t nDeviceIndex, EVRTrackedCameraFrameType eFrameType, <span class="pl-c1">uint32_t</span> * pnWidth, <span class="pl-c1">uint32_t</span> * pnHeight, <span class="pl-c1">uint32_t</span> * pnFrameBufferSize);</td>
      </tr>
      <tr>
        <td id="L1537" class="blob-num js-line-number" data-line-number="1537"></td>
        <td id="LC1537" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVRTrackedCameraError</span> (OPENVR_FNTABLE_CALLTYPE *GetCameraIntrinsics)(TrackedDeviceIndex_t nDeviceIndex, EVRTrackedCameraFrameType eFrameType, HmdVector2_t * pFocalLength, HmdVector2_t * pCenter);</td>
      </tr>
      <tr>
        <td id="L1538" class="blob-num js-line-number" data-line-number="1538"></td>
        <td id="LC1538" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVRTrackedCameraError</span> (OPENVR_FNTABLE_CALLTYPE *GetCameraProjection)(TrackedDeviceIndex_t nDeviceIndex, EVRTrackedCameraFrameType eFrameType, <span class="pl-k">float</span> flZNear, <span class="pl-k">float</span> flZFar, HmdMatrix44_t * pProjection);</td>
      </tr>
      <tr>
        <td id="L1539" class="blob-num js-line-number" data-line-number="1539"></td>
        <td id="LC1539" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVRTrackedCameraError</span> (OPENVR_FNTABLE_CALLTYPE *AcquireVideoStreamingService)(TrackedDeviceIndex_t nDeviceIndex, TrackedCameraHandle_t * pHandle);</td>
      </tr>
      <tr>
        <td id="L1540" class="blob-num js-line-number" data-line-number="1540"></td>
        <td id="LC1540" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVRTrackedCameraError</span> (OPENVR_FNTABLE_CALLTYPE *ReleaseVideoStreamingService)(TrackedCameraHandle_t hTrackedCamera);</td>
      </tr>
      <tr>
        <td id="L1541" class="blob-num js-line-number" data-line-number="1541"></td>
        <td id="LC1541" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVRTrackedCameraError</span> (OPENVR_FNTABLE_CALLTYPE *GetVideoStreamFrameBuffer)(TrackedCameraHandle_t hTrackedCamera, EVRTrackedCameraFrameType eFrameType, <span class="pl-k">void</span> * pFrameBuffer, <span class="pl-c1">uint32_t</span> nFrameBufferSize, CameraVideoStreamFrameHeader_t * pFrameHeader, <span class="pl-c1">uint32_t</span> nFrameHeaderSize);</td>
      </tr>
      <tr>
        <td id="L1542" class="blob-num js-line-number" data-line-number="1542"></td>
        <td id="LC1542" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVRTrackedCameraError</span> (OPENVR_FNTABLE_CALLTYPE *GetVideoStreamTextureSize)(TrackedDeviceIndex_t nDeviceIndex, EVRTrackedCameraFrameType eFrameType, VRTextureBounds_t * pTextureBounds, <span class="pl-c1">uint32_t</span> * pnWidth, <span class="pl-c1">uint32_t</span> * pnHeight);</td>
      </tr>
      <tr>
        <td id="L1543" class="blob-num js-line-number" data-line-number="1543"></td>
        <td id="LC1543" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVRTrackedCameraError</span> (OPENVR_FNTABLE_CALLTYPE *GetVideoStreamTextureD3D11)(TrackedCameraHandle_t hTrackedCamera, EVRTrackedCameraFrameType eFrameType, <span class="pl-k">void</span> * pD3D11DeviceOrResource, <span class="pl-k">void</span> ** ppD3D11ShaderResourceView, CameraVideoStreamFrameHeader_t * pFrameHeader, <span class="pl-c1">uint32_t</span> nFrameHeaderSize);</td>
      </tr>
      <tr>
        <td id="L1544" class="blob-num js-line-number" data-line-number="1544"></td>
        <td id="LC1544" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVRTrackedCameraError</span> (OPENVR_FNTABLE_CALLTYPE *GetVideoStreamTextureGL)(TrackedCameraHandle_t hTrackedCamera, EVRTrackedCameraFrameType eFrameType, glUInt_t * pglTextureId, CameraVideoStreamFrameHeader_t * pFrameHeader, <span class="pl-c1">uint32_t</span> nFrameHeaderSize);</td>
      </tr>
      <tr>
        <td id="L1545" class="blob-num js-line-number" data-line-number="1545"></td>
        <td id="LC1545" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVRTrackedCameraError</span> (OPENVR_FNTABLE_CALLTYPE *ReleaseVideoStreamTextureGL)(TrackedCameraHandle_t hTrackedCamera, glUInt_t glTextureId);</td>
      </tr>
      <tr>
        <td id="L1546" class="blob-num js-line-number" data-line-number="1546"></td>
        <td id="LC1546" class="blob-code blob-code-inner js-file-line">};</td>
      </tr>
      <tr>
        <td id="L1547" class="blob-num js-line-number" data-line-number="1547"></td>
        <td id="LC1547" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L1548" class="blob-num js-line-number" data-line-number="1548"></td>
        <td id="LC1548" class="blob-code blob-code-inner js-file-line"><span class="pl-k">struct</span> VR_IVRApplications_FnTable</td>
      </tr>
      <tr>
        <td id="L1549" class="blob-num js-line-number" data-line-number="1549"></td>
        <td id="LC1549" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L1550" class="blob-num js-line-number" data-line-number="1550"></td>
        <td id="LC1550" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVRApplicationError</span> (OPENVR_FNTABLE_CALLTYPE *AddApplicationManifest)(<span class="pl-k">char</span> * pchApplicationManifestFullPath, <span class="pl-k">bool</span> bTemporary);</td>
      </tr>
      <tr>
        <td id="L1551" class="blob-num js-line-number" data-line-number="1551"></td>
        <td id="LC1551" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVRApplicationError</span> (OPENVR_FNTABLE_CALLTYPE *RemoveApplicationManifest)(<span class="pl-k">char</span> * pchApplicationManifestFullPath);</td>
      </tr>
      <tr>
        <td id="L1552" class="blob-num js-line-number" data-line-number="1552"></td>
        <td id="LC1552" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">bool</span> (OPENVR_FNTABLE_CALLTYPE *IsApplicationInstalled)(<span class="pl-k">char</span> * pchAppKey);</td>
      </tr>
      <tr>
        <td id="L1553" class="blob-num js-line-number" data-line-number="1553"></td>
        <td id="LC1553" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint32_t</span> (OPENVR_FNTABLE_CALLTYPE *GetApplicationCount)();</td>
      </tr>
      <tr>
        <td id="L1554" class="blob-num js-line-number" data-line-number="1554"></td>
        <td id="LC1554" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVRApplicationError</span> (OPENVR_FNTABLE_CALLTYPE *GetApplicationKeyByIndex)(<span class="pl-c1">uint32_t</span> unApplicationIndex, <span class="pl-k">char</span> * pchAppKeyBuffer, <span class="pl-c1">uint32_t</span> unAppKeyBufferLen);</td>
      </tr>
      <tr>
        <td id="L1555" class="blob-num js-line-number" data-line-number="1555"></td>
        <td id="LC1555" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVRApplicationError</span> (OPENVR_FNTABLE_CALLTYPE *GetApplicationKeyByProcessId)(<span class="pl-c1">uint32_t</span> unProcessId, <span class="pl-k">char</span> * pchAppKeyBuffer, <span class="pl-c1">uint32_t</span> unAppKeyBufferLen);</td>
      </tr>
      <tr>
        <td id="L1556" class="blob-num js-line-number" data-line-number="1556"></td>
        <td id="LC1556" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVRApplicationError</span> (OPENVR_FNTABLE_CALLTYPE *LaunchApplication)(<span class="pl-k">char</span> * pchAppKey);</td>
      </tr>
      <tr>
        <td id="L1557" class="blob-num js-line-number" data-line-number="1557"></td>
        <td id="LC1557" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVRApplicationError</span> (OPENVR_FNTABLE_CALLTYPE *LaunchTemplateApplication)(<span class="pl-k">char</span> * pchTemplateAppKey, <span class="pl-k">char</span> * pchNewAppKey, <span class="pl-k">struct</span> AppOverrideKeys_t * pKeys, <span class="pl-c1">uint32_t</span> unKeys);</td>
      </tr>
      <tr>
        <td id="L1558" class="blob-num js-line-number" data-line-number="1558"></td>
        <td id="LC1558" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVRApplicationError</span> (OPENVR_FNTABLE_CALLTYPE *LaunchApplicationFromMimeType)(<span class="pl-k">char</span> * pchMimeType, <span class="pl-k">char</span> * pchArgs);</td>
      </tr>
      <tr>
        <td id="L1559" class="blob-num js-line-number" data-line-number="1559"></td>
        <td id="LC1559" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVRApplicationError</span> (OPENVR_FNTABLE_CALLTYPE *LaunchDashboardOverlay)(<span class="pl-k">char</span> * pchAppKey);</td>
      </tr>
      <tr>
        <td id="L1560" class="blob-num js-line-number" data-line-number="1560"></td>
        <td id="LC1560" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">bool</span> (OPENVR_FNTABLE_CALLTYPE *CancelApplicationLaunch)(<span class="pl-k">char</span> * pchAppKey);</td>
      </tr>
      <tr>
        <td id="L1561" class="blob-num js-line-number" data-line-number="1561"></td>
        <td id="LC1561" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVRApplicationError</span> (OPENVR_FNTABLE_CALLTYPE *IdentifyApplication)(<span class="pl-c1">uint32_t</span> unProcessId, <span class="pl-k">char</span> * pchAppKey);</td>
      </tr>
      <tr>
        <td id="L1562" class="blob-num js-line-number" data-line-number="1562"></td>
        <td id="LC1562" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint32_t</span> (OPENVR_FNTABLE_CALLTYPE *GetApplicationProcessId)(<span class="pl-k">char</span> * pchAppKey);</td>
      </tr>
      <tr>
        <td id="L1563" class="blob-num js-line-number" data-line-number="1563"></td>
        <td id="LC1563" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">char</span> * (OPENVR_FNTABLE_CALLTYPE *GetApplicationsErrorNameFromEnum)(EVRApplicationError error);</td>
      </tr>
      <tr>
        <td id="L1564" class="blob-num js-line-number" data-line-number="1564"></td>
        <td id="LC1564" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint32_t</span> (OPENVR_FNTABLE_CALLTYPE *GetApplicationPropertyString)(<span class="pl-k">char</span> * pchAppKey, EVRApplicationProperty eProperty, <span class="pl-k">char</span> * pchPropertyValueBuffer, <span class="pl-c1">uint32_t</span> unPropertyValueBufferLen, EVRApplicationError * peError);</td>
      </tr>
      <tr>
        <td id="L1565" class="blob-num js-line-number" data-line-number="1565"></td>
        <td id="LC1565" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">bool</span> (OPENVR_FNTABLE_CALLTYPE *GetApplicationPropertyBool)(<span class="pl-k">char</span> * pchAppKey, EVRApplicationProperty eProperty, EVRApplicationError * peError);</td>
      </tr>
      <tr>
        <td id="L1566" class="blob-num js-line-number" data-line-number="1566"></td>
        <td id="LC1566" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint64_t</span> (OPENVR_FNTABLE_CALLTYPE *GetApplicationPropertyUint64)(<span class="pl-k">char</span> * pchAppKey, EVRApplicationProperty eProperty, EVRApplicationError * peError);</td>
      </tr>
      <tr>
        <td id="L1567" class="blob-num js-line-number" data-line-number="1567"></td>
        <td id="LC1567" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVRApplicationError</span> (OPENVR_FNTABLE_CALLTYPE *SetApplicationAutoLaunch)(<span class="pl-k">char</span> * pchAppKey, <span class="pl-k">bool</span> bAutoLaunch);</td>
      </tr>
      <tr>
        <td id="L1568" class="blob-num js-line-number" data-line-number="1568"></td>
        <td id="LC1568" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">bool</span> (OPENVR_FNTABLE_CALLTYPE *GetApplicationAutoLaunch)(<span class="pl-k">char</span> * pchAppKey);</td>
      </tr>
      <tr>
        <td id="L1569" class="blob-num js-line-number" data-line-number="1569"></td>
        <td id="LC1569" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVRApplicationError</span> (OPENVR_FNTABLE_CALLTYPE *SetDefaultApplicationForMimeType)(<span class="pl-k">char</span> * pchAppKey, <span class="pl-k">char</span> * pchMimeType);</td>
      </tr>
      <tr>
        <td id="L1570" class="blob-num js-line-number" data-line-number="1570"></td>
        <td id="LC1570" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">bool</span> (OPENVR_FNTABLE_CALLTYPE *GetDefaultApplicationForMimeType)(<span class="pl-k">char</span> * pchMimeType, <span class="pl-k">char</span> * pchAppKeyBuffer, <span class="pl-c1">uint32_t</span> unAppKeyBufferLen);</td>
      </tr>
      <tr>
        <td id="L1571" class="blob-num js-line-number" data-line-number="1571"></td>
        <td id="LC1571" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">bool</span> (OPENVR_FNTABLE_CALLTYPE *GetApplicationSupportedMimeTypes)(<span class="pl-k">char</span> * pchAppKey, <span class="pl-k">char</span> * pchMimeTypesBuffer, <span class="pl-c1">uint32_t</span> unMimeTypesBuffer);</td>
      </tr>
      <tr>
        <td id="L1572" class="blob-num js-line-number" data-line-number="1572"></td>
        <td id="LC1572" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint32_t</span> (OPENVR_FNTABLE_CALLTYPE *GetApplicationsThatSupportMimeType)(<span class="pl-k">char</span> * pchMimeType, <span class="pl-k">char</span> * pchAppKeysThatSupportBuffer, <span class="pl-c1">uint32_t</span> unAppKeysThatSupportBuffer);</td>
      </tr>
      <tr>
        <td id="L1573" class="blob-num js-line-number" data-line-number="1573"></td>
        <td id="LC1573" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint32_t</span> (OPENVR_FNTABLE_CALLTYPE *GetApplicationLaunchArguments)(<span class="pl-c1">uint32_t</span> unHandle, <span class="pl-k">char</span> * pchArgs, <span class="pl-c1">uint32_t</span> unArgs);</td>
      </tr>
      <tr>
        <td id="L1574" class="blob-num js-line-number" data-line-number="1574"></td>
        <td id="LC1574" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVRApplicationError</span> (OPENVR_FNTABLE_CALLTYPE *GetStartingApplication)(<span class="pl-k">char</span> * pchAppKeyBuffer, <span class="pl-c1">uint32_t</span> unAppKeyBufferLen);</td>
      </tr>
      <tr>
        <td id="L1575" class="blob-num js-line-number" data-line-number="1575"></td>
        <td id="LC1575" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVRApplicationTransitionState</span> (OPENVR_FNTABLE_CALLTYPE *GetTransitionState)();</td>
      </tr>
      <tr>
        <td id="L1576" class="blob-num js-line-number" data-line-number="1576"></td>
        <td id="LC1576" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVRApplicationError</span> (OPENVR_FNTABLE_CALLTYPE *PerformApplicationPrelaunchCheck)(<span class="pl-k">char</span> * pchAppKey);</td>
      </tr>
      <tr>
        <td id="L1577" class="blob-num js-line-number" data-line-number="1577"></td>
        <td id="LC1577" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">char</span> * (OPENVR_FNTABLE_CALLTYPE *GetApplicationsTransitionStateNameFromEnum)(EVRApplicationTransitionState state);</td>
      </tr>
      <tr>
        <td id="L1578" class="blob-num js-line-number" data-line-number="1578"></td>
        <td id="LC1578" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">bool</span> (OPENVR_FNTABLE_CALLTYPE *IsQuitUserPromptRequested)();</td>
      </tr>
      <tr>
        <td id="L1579" class="blob-num js-line-number" data-line-number="1579"></td>
        <td id="LC1579" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVRApplicationError</span> (OPENVR_FNTABLE_CALLTYPE *LaunchInternalProcess)(<span class="pl-k">char</span> * pchBinaryPath, <span class="pl-k">char</span> * pchArguments, <span class="pl-k">char</span> * pchWorkingDirectory);</td>
      </tr>
      <tr>
        <td id="L1580" class="blob-num js-line-number" data-line-number="1580"></td>
        <td id="LC1580" class="blob-code blob-code-inner js-file-line">};</td>
      </tr>
      <tr>
        <td id="L1581" class="blob-num js-line-number" data-line-number="1581"></td>
        <td id="LC1581" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L1582" class="blob-num js-line-number" data-line-number="1582"></td>
        <td id="LC1582" class="blob-code blob-code-inner js-file-line"><span class="pl-k">struct</span> VR_IVRChaperone_FnTable</td>
      </tr>
      <tr>
        <td id="L1583" class="blob-num js-line-number" data-line-number="1583"></td>
        <td id="LC1583" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L1584" class="blob-num js-line-number" data-line-number="1584"></td>
        <td id="LC1584" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">ChaperoneCalibrationState</span> (OPENVR_FNTABLE_CALLTYPE *GetCalibrationState)();</td>
      </tr>
      <tr>
        <td id="L1585" class="blob-num js-line-number" data-line-number="1585"></td>
        <td id="LC1585" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">bool</span> (OPENVR_FNTABLE_CALLTYPE *GetPlayAreaSize)(<span class="pl-k">float</span> * pSizeX, <span class="pl-k">float</span> * pSizeZ);</td>
      </tr>
      <tr>
        <td id="L1586" class="blob-num js-line-number" data-line-number="1586"></td>
        <td id="LC1586" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">bool</span> (OPENVR_FNTABLE_CALLTYPE *GetPlayAreaRect)(<span class="pl-k">struct</span> HmdQuad_t * rect);</td>
      </tr>
      <tr>
        <td id="L1587" class="blob-num js-line-number" data-line-number="1587"></td>
        <td id="LC1587" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">void</span> (OPENVR_FNTABLE_CALLTYPE *ReloadInfo)();</td>
      </tr>
      <tr>
        <td id="L1588" class="blob-num js-line-number" data-line-number="1588"></td>
        <td id="LC1588" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">void</span> (OPENVR_FNTABLE_CALLTYPE *SetSceneColor)(<span class="pl-k">struct</span> HmdColor_t color);</td>
      </tr>
      <tr>
        <td id="L1589" class="blob-num js-line-number" data-line-number="1589"></td>
        <td id="LC1589" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">void</span> (OPENVR_FNTABLE_CALLTYPE *GetBoundsColor)(<span class="pl-k">struct</span> HmdColor_t * pOutputColorArray, <span class="pl-k">int</span> nNumOutputColors, <span class="pl-k">float</span> flCollisionBoundsFadeDistance, <span class="pl-k">struct</span> HmdColor_t * pOutputCameraColor);</td>
      </tr>
      <tr>
        <td id="L1590" class="blob-num js-line-number" data-line-number="1590"></td>
        <td id="LC1590" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">bool</span> (OPENVR_FNTABLE_CALLTYPE *AreBoundsVisible)();</td>
      </tr>
      <tr>
        <td id="L1591" class="blob-num js-line-number" data-line-number="1591"></td>
        <td id="LC1591" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">void</span> (OPENVR_FNTABLE_CALLTYPE *ForceBoundsVisible)(<span class="pl-k">bool</span> bForce);</td>
      </tr>
      <tr>
        <td id="L1592" class="blob-num js-line-number" data-line-number="1592"></td>
        <td id="LC1592" class="blob-code blob-code-inner js-file-line">};</td>
      </tr>
      <tr>
        <td id="L1593" class="blob-num js-line-number" data-line-number="1593"></td>
        <td id="LC1593" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L1594" class="blob-num js-line-number" data-line-number="1594"></td>
        <td id="LC1594" class="blob-code blob-code-inner js-file-line"><span class="pl-k">struct</span> VR_IVRChaperoneSetup_FnTable</td>
      </tr>
      <tr>
        <td id="L1595" class="blob-num js-line-number" data-line-number="1595"></td>
        <td id="LC1595" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L1596" class="blob-num js-line-number" data-line-number="1596"></td>
        <td id="LC1596" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">bool</span> (OPENVR_FNTABLE_CALLTYPE *CommitWorkingCopy)(EChaperoneConfigFile configFile);</td>
      </tr>
      <tr>
        <td id="L1597" class="blob-num js-line-number" data-line-number="1597"></td>
        <td id="LC1597" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">void</span> (OPENVR_FNTABLE_CALLTYPE *RevertWorkingCopy)();</td>
      </tr>
      <tr>
        <td id="L1598" class="blob-num js-line-number" data-line-number="1598"></td>
        <td id="LC1598" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">bool</span> (OPENVR_FNTABLE_CALLTYPE *GetWorkingPlayAreaSize)(<span class="pl-k">float</span> * pSizeX, <span class="pl-k">float</span> * pSizeZ);</td>
      </tr>
      <tr>
        <td id="L1599" class="blob-num js-line-number" data-line-number="1599"></td>
        <td id="LC1599" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">bool</span> (OPENVR_FNTABLE_CALLTYPE *GetWorkingPlayAreaRect)(<span class="pl-k">struct</span> HmdQuad_t * rect);</td>
      </tr>
      <tr>
        <td id="L1600" class="blob-num js-line-number" data-line-number="1600"></td>
        <td id="LC1600" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">bool</span> (OPENVR_FNTABLE_CALLTYPE *GetWorkingCollisionBoundsInfo)(<span class="pl-k">struct</span> HmdQuad_t * pQuadsBuffer, <span class="pl-c1">uint32_t</span> * punQuadsCount);</td>
      </tr>
      <tr>
        <td id="L1601" class="blob-num js-line-number" data-line-number="1601"></td>
        <td id="LC1601" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">bool</span> (OPENVR_FNTABLE_CALLTYPE *GetLiveCollisionBoundsInfo)(<span class="pl-k">struct</span> HmdQuad_t * pQuadsBuffer, <span class="pl-c1">uint32_t</span> * punQuadsCount);</td>
      </tr>
      <tr>
        <td id="L1602" class="blob-num js-line-number" data-line-number="1602"></td>
        <td id="LC1602" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">bool</span> (OPENVR_FNTABLE_CALLTYPE *GetWorkingSeatedZeroPoseToRawTrackingPose)(<span class="pl-k">struct</span> HmdMatrix34_t * pmatSeatedZeroPoseToRawTrackingPose);</td>
      </tr>
      <tr>
        <td id="L1603" class="blob-num js-line-number" data-line-number="1603"></td>
        <td id="LC1603" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">bool</span> (OPENVR_FNTABLE_CALLTYPE *GetWorkingStandingZeroPoseToRawTrackingPose)(<span class="pl-k">struct</span> HmdMatrix34_t * pmatStandingZeroPoseToRawTrackingPose);</td>
      </tr>
      <tr>
        <td id="L1604" class="blob-num js-line-number" data-line-number="1604"></td>
        <td id="LC1604" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">void</span> (OPENVR_FNTABLE_CALLTYPE *SetWorkingPlayAreaSize)(<span class="pl-k">float</span> sizeX, <span class="pl-k">float</span> sizeZ);</td>
      </tr>
      <tr>
        <td id="L1605" class="blob-num js-line-number" data-line-number="1605"></td>
        <td id="LC1605" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">void</span> (OPENVR_FNTABLE_CALLTYPE *SetWorkingCollisionBoundsInfo)(<span class="pl-k">struct</span> HmdQuad_t * pQuadsBuffer, <span class="pl-c1">uint32_t</span> unQuadsCount);</td>
      </tr>
      <tr>
        <td id="L1606" class="blob-num js-line-number" data-line-number="1606"></td>
        <td id="LC1606" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">void</span> (OPENVR_FNTABLE_CALLTYPE *SetWorkingSeatedZeroPoseToRawTrackingPose)(<span class="pl-k">struct</span> HmdMatrix34_t * pMatSeatedZeroPoseToRawTrackingPose);</td>
      </tr>
      <tr>
        <td id="L1607" class="blob-num js-line-number" data-line-number="1607"></td>
        <td id="LC1607" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">void</span> (OPENVR_FNTABLE_CALLTYPE *SetWorkingStandingZeroPoseToRawTrackingPose)(<span class="pl-k">struct</span> HmdMatrix34_t * pMatStandingZeroPoseToRawTrackingPose);</td>
      </tr>
      <tr>
        <td id="L1608" class="blob-num js-line-number" data-line-number="1608"></td>
        <td id="LC1608" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">void</span> (OPENVR_FNTABLE_CALLTYPE *ReloadFromDisk)(EChaperoneConfigFile configFile);</td>
      </tr>
      <tr>
        <td id="L1609" class="blob-num js-line-number" data-line-number="1609"></td>
        <td id="LC1609" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">bool</span> (OPENVR_FNTABLE_CALLTYPE *GetLiveSeatedZeroPoseToRawTrackingPose)(<span class="pl-k">struct</span> HmdMatrix34_t * pmatSeatedZeroPoseToRawTrackingPose);</td>
      </tr>
      <tr>
        <td id="L1610" class="blob-num js-line-number" data-line-number="1610"></td>
        <td id="LC1610" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">void</span> (OPENVR_FNTABLE_CALLTYPE *SetWorkingCollisionBoundsTagsInfo)(<span class="pl-c1">uint8_t</span> * pTagsBuffer, <span class="pl-c1">uint32_t</span> unTagCount);</td>
      </tr>
      <tr>
        <td id="L1611" class="blob-num js-line-number" data-line-number="1611"></td>
        <td id="LC1611" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">bool</span> (OPENVR_FNTABLE_CALLTYPE *GetLiveCollisionBoundsTagsInfo)(<span class="pl-c1">uint8_t</span> * pTagsBuffer, <span class="pl-c1">uint32_t</span> * punTagCount);</td>
      </tr>
      <tr>
        <td id="L1612" class="blob-num js-line-number" data-line-number="1612"></td>
        <td id="LC1612" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">bool</span> (OPENVR_FNTABLE_CALLTYPE *SetWorkingPhysicalBoundsInfo)(<span class="pl-k">struct</span> HmdQuad_t * pQuadsBuffer, <span class="pl-c1">uint32_t</span> unQuadsCount);</td>
      </tr>
      <tr>
        <td id="L1613" class="blob-num js-line-number" data-line-number="1613"></td>
        <td id="LC1613" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">bool</span> (OPENVR_FNTABLE_CALLTYPE *GetLivePhysicalBoundsInfo)(<span class="pl-k">struct</span> HmdQuad_t * pQuadsBuffer, <span class="pl-c1">uint32_t</span> * punQuadsCount);</td>
      </tr>
      <tr>
        <td id="L1614" class="blob-num js-line-number" data-line-number="1614"></td>
        <td id="LC1614" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">bool</span> (OPENVR_FNTABLE_CALLTYPE *ExportLiveToBuffer)(<span class="pl-k">char</span> * pBuffer, <span class="pl-c1">uint32_t</span> * pnBufferLength);</td>
      </tr>
      <tr>
        <td id="L1615" class="blob-num js-line-number" data-line-number="1615"></td>
        <td id="LC1615" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">bool</span> (OPENVR_FNTABLE_CALLTYPE *ImportFromBufferToWorking)(<span class="pl-k">char</span> * pBuffer, <span class="pl-c1">uint32_t</span> nImportFlags);</td>
      </tr>
      <tr>
        <td id="L1616" class="blob-num js-line-number" data-line-number="1616"></td>
        <td id="LC1616" class="blob-code blob-code-inner js-file-line">};</td>
      </tr>
      <tr>
        <td id="L1617" class="blob-num js-line-number" data-line-number="1617"></td>
        <td id="LC1617" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L1618" class="blob-num js-line-number" data-line-number="1618"></td>
        <td id="LC1618" class="blob-code blob-code-inner js-file-line"><span class="pl-k">struct</span> VR_IVRCompositor_FnTable</td>
      </tr>
      <tr>
        <td id="L1619" class="blob-num js-line-number" data-line-number="1619"></td>
        <td id="LC1619" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L1620" class="blob-num js-line-number" data-line-number="1620"></td>
        <td id="LC1620" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">void</span> (OPENVR_FNTABLE_CALLTYPE *SetTrackingSpace)(ETrackingUniverseOrigin eOrigin);</td>
      </tr>
      <tr>
        <td id="L1621" class="blob-num js-line-number" data-line-number="1621"></td>
        <td id="LC1621" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">ETrackingUniverseOrigin</span> (OPENVR_FNTABLE_CALLTYPE *GetTrackingSpace)();</td>
      </tr>
      <tr>
        <td id="L1622" class="blob-num js-line-number" data-line-number="1622"></td>
        <td id="LC1622" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVRCompositorError</span> (OPENVR_FNTABLE_CALLTYPE *WaitGetPoses)(<span class="pl-k">struct</span> TrackedDevicePose_t * pRenderPoseArray, <span class="pl-c1">uint32_t</span> unRenderPoseArrayCount, <span class="pl-k">struct</span> TrackedDevicePose_t * pGamePoseArray, <span class="pl-c1">uint32_t</span> unGamePoseArrayCount);</td>
      </tr>
      <tr>
        <td id="L1623" class="blob-num js-line-number" data-line-number="1623"></td>
        <td id="LC1623" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVRCompositorError</span> (OPENVR_FNTABLE_CALLTYPE *GetLastPoses)(<span class="pl-k">struct</span> TrackedDevicePose_t * pRenderPoseArray, <span class="pl-c1">uint32_t</span> unRenderPoseArrayCount, <span class="pl-k">struct</span> TrackedDevicePose_t * pGamePoseArray, <span class="pl-c1">uint32_t</span> unGamePoseArrayCount);</td>
      </tr>
      <tr>
        <td id="L1624" class="blob-num js-line-number" data-line-number="1624"></td>
        <td id="LC1624" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVRCompositorError</span> (OPENVR_FNTABLE_CALLTYPE *GetLastPoseForTrackedDeviceIndex)(TrackedDeviceIndex_t unDeviceIndex, <span class="pl-k">struct</span> TrackedDevicePose_t * pOutputPose, <span class="pl-k">struct</span> TrackedDevicePose_t * pOutputGamePose);</td>
      </tr>
      <tr>
        <td id="L1625" class="blob-num js-line-number" data-line-number="1625"></td>
        <td id="LC1625" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVRCompositorError</span> (OPENVR_FNTABLE_CALLTYPE *Submit)(EVREye eEye, <span class="pl-k">struct</span> Texture_t * pTexture, <span class="pl-k">struct</span> VRTextureBounds_t * pBounds, EVRSubmitFlags nSubmitFlags);</td>
      </tr>
      <tr>
        <td id="L1626" class="blob-num js-line-number" data-line-number="1626"></td>
        <td id="LC1626" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">void</span> (OPENVR_FNTABLE_CALLTYPE *ClearLastSubmittedFrame)();</td>
      </tr>
      <tr>
        <td id="L1627" class="blob-num js-line-number" data-line-number="1627"></td>
        <td id="LC1627" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">void</span> (OPENVR_FNTABLE_CALLTYPE *PostPresentHandoff)();</td>
      </tr>
      <tr>
        <td id="L1628" class="blob-num js-line-number" data-line-number="1628"></td>
        <td id="LC1628" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">bool</span> (OPENVR_FNTABLE_CALLTYPE *GetFrameTiming)(<span class="pl-k">struct</span> Compositor_FrameTiming * pTiming, <span class="pl-c1">uint32_t</span> unFramesAgo);</td>
      </tr>
      <tr>
        <td id="L1629" class="blob-num js-line-number" data-line-number="1629"></td>
        <td id="LC1629" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint32_t</span> (OPENVR_FNTABLE_CALLTYPE *GetFrameTimings)(<span class="pl-k">struct</span> Compositor_FrameTiming * pTiming, <span class="pl-c1">uint32_t</span> nFrames);</td>
      </tr>
      <tr>
        <td id="L1630" class="blob-num js-line-number" data-line-number="1630"></td>
        <td id="LC1630" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">float</span> (OPENVR_FNTABLE_CALLTYPE *GetFrameTimeRemaining)();</td>
      </tr>
      <tr>
        <td id="L1631" class="blob-num js-line-number" data-line-number="1631"></td>
        <td id="LC1631" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">void</span> (OPENVR_FNTABLE_CALLTYPE *GetCumulativeStats)(<span class="pl-k">struct</span> Compositor_CumulativeStats * pStats, <span class="pl-c1">uint32_t</span> nStatsSizeInBytes);</td>
      </tr>
      <tr>
        <td id="L1632" class="blob-num js-line-number" data-line-number="1632"></td>
        <td id="LC1632" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">void</span> (OPENVR_FNTABLE_CALLTYPE *FadeToColor)(<span class="pl-k">float</span> fSeconds, <span class="pl-k">float</span> fRed, <span class="pl-k">float</span> fGreen, <span class="pl-k">float</span> fBlue, <span class="pl-k">float</span> fAlpha, <span class="pl-k">bool</span> bBackground);</td>
      </tr>
      <tr>
        <td id="L1633" class="blob-num js-line-number" data-line-number="1633"></td>
        <td id="LC1633" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">struct</span> <span class="pl-smi">HmdColor_t</span> (OPENVR_FNTABLE_CALLTYPE *GetCurrentFadeColor)(<span class="pl-k">bool</span> bBackground);</td>
      </tr>
      <tr>
        <td id="L1634" class="blob-num js-line-number" data-line-number="1634"></td>
        <td id="LC1634" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">void</span> (OPENVR_FNTABLE_CALLTYPE *FadeGrid)(<span class="pl-k">float</span> fSeconds, <span class="pl-k">bool</span> bFadeIn);</td>
      </tr>
      <tr>
        <td id="L1635" class="blob-num js-line-number" data-line-number="1635"></td>
        <td id="LC1635" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">float</span> (OPENVR_FNTABLE_CALLTYPE *GetCurrentGridAlpha)();</td>
      </tr>
      <tr>
        <td id="L1636" class="blob-num js-line-number" data-line-number="1636"></td>
        <td id="LC1636" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVRCompositorError</span> (OPENVR_FNTABLE_CALLTYPE *SetSkyboxOverride)(<span class="pl-k">struct</span> Texture_t * pTextures, <span class="pl-c1">uint32_t</span> unTextureCount);</td>
      </tr>
      <tr>
        <td id="L1637" class="blob-num js-line-number" data-line-number="1637"></td>
        <td id="LC1637" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">void</span> (OPENVR_FNTABLE_CALLTYPE *ClearSkyboxOverride)();</td>
      </tr>
      <tr>
        <td id="L1638" class="blob-num js-line-number" data-line-number="1638"></td>
        <td id="LC1638" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">void</span> (OPENVR_FNTABLE_CALLTYPE *CompositorBringToFront)();</td>
      </tr>
      <tr>
        <td id="L1639" class="blob-num js-line-number" data-line-number="1639"></td>
        <td id="LC1639" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">void</span> (OPENVR_FNTABLE_CALLTYPE *CompositorGoToBack)();</td>
      </tr>
      <tr>
        <td id="L1640" class="blob-num js-line-number" data-line-number="1640"></td>
        <td id="LC1640" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">void</span> (OPENVR_FNTABLE_CALLTYPE *CompositorQuit)();</td>
      </tr>
      <tr>
        <td id="L1641" class="blob-num js-line-number" data-line-number="1641"></td>
        <td id="LC1641" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">bool</span> (OPENVR_FNTABLE_CALLTYPE *IsFullscreen)();</td>
      </tr>
      <tr>
        <td id="L1642" class="blob-num js-line-number" data-line-number="1642"></td>
        <td id="LC1642" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint32_t</span> (OPENVR_FNTABLE_CALLTYPE *GetCurrentSceneFocusProcess)();</td>
      </tr>
      <tr>
        <td id="L1643" class="blob-num js-line-number" data-line-number="1643"></td>
        <td id="LC1643" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint32_t</span> (OPENVR_FNTABLE_CALLTYPE *GetLastFrameRenderer)();</td>
      </tr>
      <tr>
        <td id="L1644" class="blob-num js-line-number" data-line-number="1644"></td>
        <td id="LC1644" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">bool</span> (OPENVR_FNTABLE_CALLTYPE *CanRenderScene)();</td>
      </tr>
      <tr>
        <td id="L1645" class="blob-num js-line-number" data-line-number="1645"></td>
        <td id="LC1645" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">void</span> (OPENVR_FNTABLE_CALLTYPE *ShowMirrorWindow)();</td>
      </tr>
      <tr>
        <td id="L1646" class="blob-num js-line-number" data-line-number="1646"></td>
        <td id="LC1646" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">void</span> (OPENVR_FNTABLE_CALLTYPE *HideMirrorWindow)();</td>
      </tr>
      <tr>
        <td id="L1647" class="blob-num js-line-number" data-line-number="1647"></td>
        <td id="LC1647" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">bool</span> (OPENVR_FNTABLE_CALLTYPE *IsMirrorWindowVisible)();</td>
      </tr>
      <tr>
        <td id="L1648" class="blob-num js-line-number" data-line-number="1648"></td>
        <td id="LC1648" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">void</span> (OPENVR_FNTABLE_CALLTYPE *CompositorDumpImages)();</td>
      </tr>
      <tr>
        <td id="L1649" class="blob-num js-line-number" data-line-number="1649"></td>
        <td id="LC1649" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">bool</span> (OPENVR_FNTABLE_CALLTYPE *ShouldAppRenderWithLowResources)();</td>
      </tr>
      <tr>
        <td id="L1650" class="blob-num js-line-number" data-line-number="1650"></td>
        <td id="LC1650" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">void</span> (OPENVR_FNTABLE_CALLTYPE *ForceInterleavedReprojectionOn)(<span class="pl-k">bool</span> bOverride);</td>
      </tr>
      <tr>
        <td id="L1651" class="blob-num js-line-number" data-line-number="1651"></td>
        <td id="LC1651" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">void</span> (OPENVR_FNTABLE_CALLTYPE *ForceReconnectProcess)();</td>
      </tr>
      <tr>
        <td id="L1652" class="blob-num js-line-number" data-line-number="1652"></td>
        <td id="LC1652" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">void</span> (OPENVR_FNTABLE_CALLTYPE *SuspendRendering)(<span class="pl-k">bool</span> bSuspend);</td>
      </tr>
      <tr>
        <td id="L1653" class="blob-num js-line-number" data-line-number="1653"></td>
        <td id="LC1653" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVRCompositorError</span> (OPENVR_FNTABLE_CALLTYPE *GetMirrorTextureD3D11)(EVREye eEye, <span class="pl-k">void</span> * pD3D11DeviceOrResource, <span class="pl-k">void</span> ** ppD3D11ShaderResourceView);</td>
      </tr>
      <tr>
        <td id="L1654" class="blob-num js-line-number" data-line-number="1654"></td>
        <td id="LC1654" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVRCompositorError</span> (OPENVR_FNTABLE_CALLTYPE *GetMirrorTextureGL)(EVREye eEye, glUInt_t * pglTextureId, glSharedTextureHandle_t * pglSharedTextureHandle);</td>
      </tr>
      <tr>
        <td id="L1655" class="blob-num js-line-number" data-line-number="1655"></td>
        <td id="LC1655" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">bool</span> (OPENVR_FNTABLE_CALLTYPE *ReleaseSharedGLTexture)(glUInt_t glTextureId, glSharedTextureHandle_t glSharedTextureHandle);</td>
      </tr>
      <tr>
        <td id="L1656" class="blob-num js-line-number" data-line-number="1656"></td>
        <td id="LC1656" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">void</span> (OPENVR_FNTABLE_CALLTYPE *LockGLSharedTextureForAccess)(glSharedTextureHandle_t glSharedTextureHandle);</td>
      </tr>
      <tr>
        <td id="L1657" class="blob-num js-line-number" data-line-number="1657"></td>
        <td id="LC1657" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">void</span> (OPENVR_FNTABLE_CALLTYPE *UnlockGLSharedTextureForAccess)(glSharedTextureHandle_t glSharedTextureHandle);</td>
      </tr>
      <tr>
        <td id="L1658" class="blob-num js-line-number" data-line-number="1658"></td>
        <td id="LC1658" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint32_t</span> (OPENVR_FNTABLE_CALLTYPE *GetVulkanInstanceExtensionsRequired)(<span class="pl-k">char</span> * pchValue, <span class="pl-c1">uint32_t</span> unBufferSize);</td>
      </tr>
      <tr>
        <td id="L1659" class="blob-num js-line-number" data-line-number="1659"></td>
        <td id="LC1659" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint32_t</span> (OPENVR_FNTABLE_CALLTYPE *GetVulkanDeviceExtensionsRequired)(<span class="pl-k">struct</span> VkPhysicalDevice_T * pPhysicalDevice, <span class="pl-k">char</span> * pchValue, <span class="pl-c1">uint32_t</span> unBufferSize);</td>
      </tr>
      <tr>
        <td id="L1660" class="blob-num js-line-number" data-line-number="1660"></td>
        <td id="LC1660" class="blob-code blob-code-inner js-file-line">};</td>
      </tr>
      <tr>
        <td id="L1661" class="blob-num js-line-number" data-line-number="1661"></td>
        <td id="LC1661" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L1662" class="blob-num js-line-number" data-line-number="1662"></td>
        <td id="LC1662" class="blob-code blob-code-inner js-file-line"><span class="pl-k">struct</span> VR_IVROverlay_FnTable</td>
      </tr>
      <tr>
        <td id="L1663" class="blob-num js-line-number" data-line-number="1663"></td>
        <td id="LC1663" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L1664" class="blob-num js-line-number" data-line-number="1664"></td>
        <td id="LC1664" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVROverlayError</span> (OPENVR_FNTABLE_CALLTYPE *FindOverlay)(<span class="pl-k">char</span> * pchOverlayKey, VROverlayHandle_t * pOverlayHandle);</td>
      </tr>
      <tr>
        <td id="L1665" class="blob-num js-line-number" data-line-number="1665"></td>
        <td id="LC1665" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVROverlayError</span> (OPENVR_FNTABLE_CALLTYPE *CreateOverlay)(<span class="pl-k">char</span> * pchOverlayKey, <span class="pl-k">char</span> * pchOverlayFriendlyName, VROverlayHandle_t * pOverlayHandle);</td>
      </tr>
      <tr>
        <td id="L1666" class="blob-num js-line-number" data-line-number="1666"></td>
        <td id="LC1666" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVROverlayError</span> (OPENVR_FNTABLE_CALLTYPE *DestroyOverlay)(VROverlayHandle_t ulOverlayHandle);</td>
      </tr>
      <tr>
        <td id="L1667" class="blob-num js-line-number" data-line-number="1667"></td>
        <td id="LC1667" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVROverlayError</span> (OPENVR_FNTABLE_CALLTYPE *SetHighQualityOverlay)(VROverlayHandle_t ulOverlayHandle);</td>
      </tr>
      <tr>
        <td id="L1668" class="blob-num js-line-number" data-line-number="1668"></td>
        <td id="LC1668" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">VROverlayHandle_t</span> (OPENVR_FNTABLE_CALLTYPE *GetHighQualityOverlay)();</td>
      </tr>
      <tr>
        <td id="L1669" class="blob-num js-line-number" data-line-number="1669"></td>
        <td id="LC1669" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint32_t</span> (OPENVR_FNTABLE_CALLTYPE *GetOverlayKey)(VROverlayHandle_t ulOverlayHandle, <span class="pl-k">char</span> * pchValue, <span class="pl-c1">uint32_t</span> unBufferSize, EVROverlayError * pError);</td>
      </tr>
      <tr>
        <td id="L1670" class="blob-num js-line-number" data-line-number="1670"></td>
        <td id="LC1670" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint32_t</span> (OPENVR_FNTABLE_CALLTYPE *GetOverlayName)(VROverlayHandle_t ulOverlayHandle, <span class="pl-k">char</span> * pchValue, <span class="pl-c1">uint32_t</span> unBufferSize, EVROverlayError * pError);</td>
      </tr>
      <tr>
        <td id="L1671" class="blob-num js-line-number" data-line-number="1671"></td>
        <td id="LC1671" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVROverlayError</span> (OPENVR_FNTABLE_CALLTYPE *GetOverlayImageData)(VROverlayHandle_t ulOverlayHandle, <span class="pl-k">void</span> * pvBuffer, <span class="pl-c1">uint32_t</span> unBufferSize, <span class="pl-c1">uint32_t</span> * punWidth, <span class="pl-c1">uint32_t</span> * punHeight);</td>
      </tr>
      <tr>
        <td id="L1672" class="blob-num js-line-number" data-line-number="1672"></td>
        <td id="LC1672" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">char</span> * (OPENVR_FNTABLE_CALLTYPE *GetOverlayErrorNameFromEnum)(EVROverlayError error);</td>
      </tr>
      <tr>
        <td id="L1673" class="blob-num js-line-number" data-line-number="1673"></td>
        <td id="LC1673" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVROverlayError</span> (OPENVR_FNTABLE_CALLTYPE *SetOverlayRenderingPid)(VROverlayHandle_t ulOverlayHandle, <span class="pl-c1">uint32_t</span> unPID);</td>
      </tr>
      <tr>
        <td id="L1674" class="blob-num js-line-number" data-line-number="1674"></td>
        <td id="LC1674" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint32_t</span> (OPENVR_FNTABLE_CALLTYPE *GetOverlayRenderingPid)(VROverlayHandle_t ulOverlayHandle);</td>
      </tr>
      <tr>
        <td id="L1675" class="blob-num js-line-number" data-line-number="1675"></td>
        <td id="LC1675" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVROverlayError</span> (OPENVR_FNTABLE_CALLTYPE *SetOverlayFlag)(VROverlayHandle_t ulOverlayHandle, VROverlayFlags eOverlayFlag, <span class="pl-k">bool</span> bEnabled);</td>
      </tr>
      <tr>
        <td id="L1676" class="blob-num js-line-number" data-line-number="1676"></td>
        <td id="LC1676" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVROverlayError</span> (OPENVR_FNTABLE_CALLTYPE *GetOverlayFlag)(VROverlayHandle_t ulOverlayHandle, VROverlayFlags eOverlayFlag, <span class="pl-k">bool</span> * pbEnabled);</td>
      </tr>
      <tr>
        <td id="L1677" class="blob-num js-line-number" data-line-number="1677"></td>
        <td id="LC1677" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVROverlayError</span> (OPENVR_FNTABLE_CALLTYPE *SetOverlayColor)(VROverlayHandle_t ulOverlayHandle, <span class="pl-k">float</span> fRed, <span class="pl-k">float</span> fGreen, <span class="pl-k">float</span> fBlue);</td>
      </tr>
      <tr>
        <td id="L1678" class="blob-num js-line-number" data-line-number="1678"></td>
        <td id="LC1678" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVROverlayError</span> (OPENVR_FNTABLE_CALLTYPE *GetOverlayColor)(VROverlayHandle_t ulOverlayHandle, <span class="pl-k">float</span> * pfRed, <span class="pl-k">float</span> * pfGreen, <span class="pl-k">float</span> * pfBlue);</td>
      </tr>
      <tr>
        <td id="L1679" class="blob-num js-line-number" data-line-number="1679"></td>
        <td id="LC1679" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVROverlayError</span> (OPENVR_FNTABLE_CALLTYPE *SetOverlayAlpha)(VROverlayHandle_t ulOverlayHandle, <span class="pl-k">float</span> fAlpha);</td>
      </tr>
      <tr>
        <td id="L1680" class="blob-num js-line-number" data-line-number="1680"></td>
        <td id="LC1680" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVROverlayError</span> (OPENVR_FNTABLE_CALLTYPE *GetOverlayAlpha)(VROverlayHandle_t ulOverlayHandle, <span class="pl-k">float</span> * pfAlpha);</td>
      </tr>
      <tr>
        <td id="L1681" class="blob-num js-line-number" data-line-number="1681"></td>
        <td id="LC1681" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVROverlayError</span> (OPENVR_FNTABLE_CALLTYPE *SetOverlayTexelAspect)(VROverlayHandle_t ulOverlayHandle, <span class="pl-k">float</span> fTexelAspect);</td>
      </tr>
      <tr>
        <td id="L1682" class="blob-num js-line-number" data-line-number="1682"></td>
        <td id="LC1682" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVROverlayError</span> (OPENVR_FNTABLE_CALLTYPE *GetOverlayTexelAspect)(VROverlayHandle_t ulOverlayHandle, <span class="pl-k">float</span> * pfTexelAspect);</td>
      </tr>
      <tr>
        <td id="L1683" class="blob-num js-line-number" data-line-number="1683"></td>
        <td id="LC1683" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVROverlayError</span> (OPENVR_FNTABLE_CALLTYPE *SetOverlaySortOrder)(VROverlayHandle_t ulOverlayHandle, <span class="pl-c1">uint32_t</span> unSortOrder);</td>
      </tr>
      <tr>
        <td id="L1684" class="blob-num js-line-number" data-line-number="1684"></td>
        <td id="LC1684" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVROverlayError</span> (OPENVR_FNTABLE_CALLTYPE *GetOverlaySortOrder)(VROverlayHandle_t ulOverlayHandle, <span class="pl-c1">uint32_t</span> * punSortOrder);</td>
      </tr>
      <tr>
        <td id="L1685" class="blob-num js-line-number" data-line-number="1685"></td>
        <td id="LC1685" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVROverlayError</span> (OPENVR_FNTABLE_CALLTYPE *SetOverlayWidthInMeters)(VROverlayHandle_t ulOverlayHandle, <span class="pl-k">float</span> fWidthInMeters);</td>
      </tr>
      <tr>
        <td id="L1686" class="blob-num js-line-number" data-line-number="1686"></td>
        <td id="LC1686" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVROverlayError</span> (OPENVR_FNTABLE_CALLTYPE *GetOverlayWidthInMeters)(VROverlayHandle_t ulOverlayHandle, <span class="pl-k">float</span> * pfWidthInMeters);</td>
      </tr>
      <tr>
        <td id="L1687" class="blob-num js-line-number" data-line-number="1687"></td>
        <td id="LC1687" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVROverlayError</span> (OPENVR_FNTABLE_CALLTYPE *SetOverlayAutoCurveDistanceRangeInMeters)(VROverlayHandle_t ulOverlayHandle, <span class="pl-k">float</span> fMinDistanceInMeters, <span class="pl-k">float</span> fMaxDistanceInMeters);</td>
      </tr>
      <tr>
        <td id="L1688" class="blob-num js-line-number" data-line-number="1688"></td>
        <td id="LC1688" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVROverlayError</span> (OPENVR_FNTABLE_CALLTYPE *GetOverlayAutoCurveDistanceRangeInMeters)(VROverlayHandle_t ulOverlayHandle, <span class="pl-k">float</span> * pfMinDistanceInMeters, <span class="pl-k">float</span> * pfMaxDistanceInMeters);</td>
      </tr>
      <tr>
        <td id="L1689" class="blob-num js-line-number" data-line-number="1689"></td>
        <td id="LC1689" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVROverlayError</span> (OPENVR_FNTABLE_CALLTYPE *SetOverlayTextureColorSpace)(VROverlayHandle_t ulOverlayHandle, EColorSpace eTextureColorSpace);</td>
      </tr>
      <tr>
        <td id="L1690" class="blob-num js-line-number" data-line-number="1690"></td>
        <td id="LC1690" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVROverlayError</span> (OPENVR_FNTABLE_CALLTYPE *GetOverlayTextureColorSpace)(VROverlayHandle_t ulOverlayHandle, EColorSpace * peTextureColorSpace);</td>
      </tr>
      <tr>
        <td id="L1691" class="blob-num js-line-number" data-line-number="1691"></td>
        <td id="LC1691" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVROverlayError</span> (OPENVR_FNTABLE_CALLTYPE *SetOverlayTextureBounds)(VROverlayHandle_t ulOverlayHandle, <span class="pl-k">struct</span> VRTextureBounds_t * pOverlayTextureBounds);</td>
      </tr>
      <tr>
        <td id="L1692" class="blob-num js-line-number" data-line-number="1692"></td>
        <td id="LC1692" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVROverlayError</span> (OPENVR_FNTABLE_CALLTYPE *GetOverlayTextureBounds)(VROverlayHandle_t ulOverlayHandle, <span class="pl-k">struct</span> VRTextureBounds_t * pOverlayTextureBounds);</td>
      </tr>
      <tr>
        <td id="L1693" class="blob-num js-line-number" data-line-number="1693"></td>
        <td id="LC1693" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVROverlayError</span> (OPENVR_FNTABLE_CALLTYPE *GetOverlayTransformType)(VROverlayHandle_t ulOverlayHandle, VROverlayTransformType * peTransformType);</td>
      </tr>
      <tr>
        <td id="L1694" class="blob-num js-line-number" data-line-number="1694"></td>
        <td id="LC1694" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVROverlayError</span> (OPENVR_FNTABLE_CALLTYPE *SetOverlayTransformAbsolute)(VROverlayHandle_t ulOverlayHandle, ETrackingUniverseOrigin eTrackingOrigin, <span class="pl-k">struct</span> HmdMatrix34_t * pmatTrackingOriginToOverlayTransform);</td>
      </tr>
      <tr>
        <td id="L1695" class="blob-num js-line-number" data-line-number="1695"></td>
        <td id="LC1695" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVROverlayError</span> (OPENVR_FNTABLE_CALLTYPE *GetOverlayTransformAbsolute)(VROverlayHandle_t ulOverlayHandle, ETrackingUniverseOrigin * peTrackingOrigin, <span class="pl-k">struct</span> HmdMatrix34_t * pmatTrackingOriginToOverlayTransform);</td>
      </tr>
      <tr>
        <td id="L1696" class="blob-num js-line-number" data-line-number="1696"></td>
        <td id="LC1696" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVROverlayError</span> (OPENVR_FNTABLE_CALLTYPE *SetOverlayTransformTrackedDeviceRelative)(VROverlayHandle_t ulOverlayHandle, TrackedDeviceIndex_t unTrackedDevice, <span class="pl-k">struct</span> HmdMatrix34_t * pmatTrackedDeviceToOverlayTransform);</td>
      </tr>
      <tr>
        <td id="L1697" class="blob-num js-line-number" data-line-number="1697"></td>
        <td id="LC1697" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVROverlayError</span> (OPENVR_FNTABLE_CALLTYPE *GetOverlayTransformTrackedDeviceRelative)(VROverlayHandle_t ulOverlayHandle, TrackedDeviceIndex_t * punTrackedDevice, <span class="pl-k">struct</span> HmdMatrix34_t * pmatTrackedDeviceToOverlayTransform);</td>
      </tr>
      <tr>
        <td id="L1698" class="blob-num js-line-number" data-line-number="1698"></td>
        <td id="LC1698" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVROverlayError</span> (OPENVR_FNTABLE_CALLTYPE *SetOverlayTransformTrackedDeviceComponent)(VROverlayHandle_t ulOverlayHandle, TrackedDeviceIndex_t unDeviceIndex, <span class="pl-k">char</span> * pchComponentName);</td>
      </tr>
      <tr>
        <td id="L1699" class="blob-num js-line-number" data-line-number="1699"></td>
        <td id="LC1699" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVROverlayError</span> (OPENVR_FNTABLE_CALLTYPE *GetOverlayTransformTrackedDeviceComponent)(VROverlayHandle_t ulOverlayHandle, TrackedDeviceIndex_t * punDeviceIndex, <span class="pl-k">char</span> * pchComponentName, <span class="pl-c1">uint32_t</span> unComponentNameSize);</td>
      </tr>
      <tr>
        <td id="L1700" class="blob-num js-line-number" data-line-number="1700"></td>
        <td id="LC1700" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVROverlayError</span> (OPENVR_FNTABLE_CALLTYPE *ShowOverlay)(VROverlayHandle_t ulOverlayHandle);</td>
      </tr>
      <tr>
        <td id="L1701" class="blob-num js-line-number" data-line-number="1701"></td>
        <td id="LC1701" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVROverlayError</span> (OPENVR_FNTABLE_CALLTYPE *HideOverlay)(VROverlayHandle_t ulOverlayHandle);</td>
      </tr>
      <tr>
        <td id="L1702" class="blob-num js-line-number" data-line-number="1702"></td>
        <td id="LC1702" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">bool</span> (OPENVR_FNTABLE_CALLTYPE *IsOverlayVisible)(VROverlayHandle_t ulOverlayHandle);</td>
      </tr>
      <tr>
        <td id="L1703" class="blob-num js-line-number" data-line-number="1703"></td>
        <td id="LC1703" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVROverlayError</span> (OPENVR_FNTABLE_CALLTYPE *GetTransformForOverlayCoordinates)(VROverlayHandle_t ulOverlayHandle, ETrackingUniverseOrigin eTrackingOrigin, <span class="pl-k">struct</span> HmdVector2_t coordinatesInOverlay, <span class="pl-k">struct</span> HmdMatrix34_t * pmatTransform);</td>
      </tr>
      <tr>
        <td id="L1704" class="blob-num js-line-number" data-line-number="1704"></td>
        <td id="LC1704" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">bool</span> (OPENVR_FNTABLE_CALLTYPE *PollNextOverlayEvent)(VROverlayHandle_t ulOverlayHandle, <span class="pl-k">struct</span> VREvent_t * pEvent, <span class="pl-c1">uint32_t</span> uncbVREvent);</td>
      </tr>
      <tr>
        <td id="L1705" class="blob-num js-line-number" data-line-number="1705"></td>
        <td id="LC1705" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVROverlayError</span> (OPENVR_FNTABLE_CALLTYPE *GetOverlayInputMethod)(VROverlayHandle_t ulOverlayHandle, VROverlayInputMethod * peInputMethod);</td>
      </tr>
      <tr>
        <td id="L1706" class="blob-num js-line-number" data-line-number="1706"></td>
        <td id="LC1706" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVROverlayError</span> (OPENVR_FNTABLE_CALLTYPE *SetOverlayInputMethod)(VROverlayHandle_t ulOverlayHandle, VROverlayInputMethod eInputMethod);</td>
      </tr>
      <tr>
        <td id="L1707" class="blob-num js-line-number" data-line-number="1707"></td>
        <td id="LC1707" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVROverlayError</span> (OPENVR_FNTABLE_CALLTYPE *GetOverlayMouseScale)(VROverlayHandle_t ulOverlayHandle, <span class="pl-k">struct</span> HmdVector2_t * pvecMouseScale);</td>
      </tr>
      <tr>
        <td id="L1708" class="blob-num js-line-number" data-line-number="1708"></td>
        <td id="LC1708" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVROverlayError</span> (OPENVR_FNTABLE_CALLTYPE *SetOverlayMouseScale)(VROverlayHandle_t ulOverlayHandle, <span class="pl-k">struct</span> HmdVector2_t * pvecMouseScale);</td>
      </tr>
      <tr>
        <td id="L1709" class="blob-num js-line-number" data-line-number="1709"></td>
        <td id="LC1709" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">bool</span> (OPENVR_FNTABLE_CALLTYPE *ComputeOverlayIntersection)(VROverlayHandle_t ulOverlayHandle, <span class="pl-k">struct</span> VROverlayIntersectionParams_t * pParams, <span class="pl-k">struct</span> VROverlayIntersectionResults_t * pResults);</td>
      </tr>
      <tr>
        <td id="L1710" class="blob-num js-line-number" data-line-number="1710"></td>
        <td id="LC1710" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">bool</span> (OPENVR_FNTABLE_CALLTYPE *HandleControllerOverlayInteractionAsMouse)(VROverlayHandle_t ulOverlayHandle, TrackedDeviceIndex_t unControllerDeviceIndex);</td>
      </tr>
      <tr>
        <td id="L1711" class="blob-num js-line-number" data-line-number="1711"></td>
        <td id="LC1711" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">bool</span> (OPENVR_FNTABLE_CALLTYPE *IsHoverTargetOverlay)(VROverlayHandle_t ulOverlayHandle);</td>
      </tr>
      <tr>
        <td id="L1712" class="blob-num js-line-number" data-line-number="1712"></td>
        <td id="LC1712" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">VROverlayHandle_t</span> (OPENVR_FNTABLE_CALLTYPE *GetGamepadFocusOverlay)();</td>
      </tr>
      <tr>
        <td id="L1713" class="blob-num js-line-number" data-line-number="1713"></td>
        <td id="LC1713" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVROverlayError</span> (OPENVR_FNTABLE_CALLTYPE *SetGamepadFocusOverlay)(VROverlayHandle_t ulNewFocusOverlay);</td>
      </tr>
      <tr>
        <td id="L1714" class="blob-num js-line-number" data-line-number="1714"></td>
        <td id="LC1714" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVROverlayError</span> (OPENVR_FNTABLE_CALLTYPE *SetOverlayNeighbor)(EOverlayDirection eDirection, VROverlayHandle_t ulFrom, VROverlayHandle_t ulTo);</td>
      </tr>
      <tr>
        <td id="L1715" class="blob-num js-line-number" data-line-number="1715"></td>
        <td id="LC1715" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVROverlayError</span> (OPENVR_FNTABLE_CALLTYPE *MoveGamepadFocusToNeighbor)(EOverlayDirection eDirection, VROverlayHandle_t ulFrom);</td>
      </tr>
      <tr>
        <td id="L1716" class="blob-num js-line-number" data-line-number="1716"></td>
        <td id="LC1716" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVROverlayError</span> (OPENVR_FNTABLE_CALLTYPE *SetOverlayTexture)(VROverlayHandle_t ulOverlayHandle, <span class="pl-k">struct</span> Texture_t * pTexture);</td>
      </tr>
      <tr>
        <td id="L1717" class="blob-num js-line-number" data-line-number="1717"></td>
        <td id="LC1717" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVROverlayError</span> (OPENVR_FNTABLE_CALLTYPE *ClearOverlayTexture)(VROverlayHandle_t ulOverlayHandle);</td>
      </tr>
      <tr>
        <td id="L1718" class="blob-num js-line-number" data-line-number="1718"></td>
        <td id="LC1718" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVROverlayError</span> (OPENVR_FNTABLE_CALLTYPE *SetOverlayRaw)(VROverlayHandle_t ulOverlayHandle, <span class="pl-k">void</span> * pvBuffer, <span class="pl-c1">uint32_t</span> unWidth, <span class="pl-c1">uint32_t</span> unHeight, <span class="pl-c1">uint32_t</span> unDepth);</td>
      </tr>
      <tr>
        <td id="L1719" class="blob-num js-line-number" data-line-number="1719"></td>
        <td id="LC1719" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVROverlayError</span> (OPENVR_FNTABLE_CALLTYPE *SetOverlayFromFile)(VROverlayHandle_t ulOverlayHandle, <span class="pl-k">char</span> * pchFilePath);</td>
      </tr>
      <tr>
        <td id="L1720" class="blob-num js-line-number" data-line-number="1720"></td>
        <td id="LC1720" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVROverlayError</span> (OPENVR_FNTABLE_CALLTYPE *GetOverlayTexture)(VROverlayHandle_t ulOverlayHandle, <span class="pl-k">void</span> ** pNativeTextureHandle, <span class="pl-k">void</span> * pNativeTextureRef, <span class="pl-c1">uint32_t</span> * pWidth, <span class="pl-c1">uint32_t</span> * pHeight, <span class="pl-c1">uint32_t</span> * pNativeFormat, ETextureType * pAPIType, EColorSpace * pColorSpace, <span class="pl-k">struct</span> VRTextureBounds_t * pTextureBounds);</td>
      </tr>
      <tr>
        <td id="L1721" class="blob-num js-line-number" data-line-number="1721"></td>
        <td id="LC1721" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVROverlayError</span> (OPENVR_FNTABLE_CALLTYPE *ReleaseNativeOverlayHandle)(VROverlayHandle_t ulOverlayHandle, <span class="pl-k">void</span> * pNativeTextureHandle);</td>
      </tr>
      <tr>
        <td id="L1722" class="blob-num js-line-number" data-line-number="1722"></td>
        <td id="LC1722" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVROverlayError</span> (OPENVR_FNTABLE_CALLTYPE *GetOverlayTextureSize)(VROverlayHandle_t ulOverlayHandle, <span class="pl-c1">uint32_t</span> * pWidth, <span class="pl-c1">uint32_t</span> * pHeight);</td>
      </tr>
      <tr>
        <td id="L1723" class="blob-num js-line-number" data-line-number="1723"></td>
        <td id="LC1723" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVROverlayError</span> (OPENVR_FNTABLE_CALLTYPE *CreateDashboardOverlay)(<span class="pl-k">char</span> * pchOverlayKey, <span class="pl-k">char</span> * pchOverlayFriendlyName, VROverlayHandle_t * pMainHandle, VROverlayHandle_t * pThumbnailHandle);</td>
      </tr>
      <tr>
        <td id="L1724" class="blob-num js-line-number" data-line-number="1724"></td>
        <td id="LC1724" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">bool</span> (OPENVR_FNTABLE_CALLTYPE *IsDashboardVisible)();</td>
      </tr>
      <tr>
        <td id="L1725" class="blob-num js-line-number" data-line-number="1725"></td>
        <td id="LC1725" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">bool</span> (OPENVR_FNTABLE_CALLTYPE *IsActiveDashboardOverlay)(VROverlayHandle_t ulOverlayHandle);</td>
      </tr>
      <tr>
        <td id="L1726" class="blob-num js-line-number" data-line-number="1726"></td>
        <td id="LC1726" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVROverlayError</span> (OPENVR_FNTABLE_CALLTYPE *SetDashboardOverlaySceneProcess)(VROverlayHandle_t ulOverlayHandle, <span class="pl-c1">uint32_t</span> unProcessId);</td>
      </tr>
      <tr>
        <td id="L1727" class="blob-num js-line-number" data-line-number="1727"></td>
        <td id="LC1727" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVROverlayError</span> (OPENVR_FNTABLE_CALLTYPE *GetDashboardOverlaySceneProcess)(VROverlayHandle_t ulOverlayHandle, <span class="pl-c1">uint32_t</span> * punProcessId);</td>
      </tr>
      <tr>
        <td id="L1728" class="blob-num js-line-number" data-line-number="1728"></td>
        <td id="LC1728" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">void</span> (OPENVR_FNTABLE_CALLTYPE *ShowDashboard)(<span class="pl-k">char</span> * pchOverlayToShow);</td>
      </tr>
      <tr>
        <td id="L1729" class="blob-num js-line-number" data-line-number="1729"></td>
        <td id="LC1729" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">TrackedDeviceIndex_t</span> (OPENVR_FNTABLE_CALLTYPE *GetPrimaryDashboardDevice)();</td>
      </tr>
      <tr>
        <td id="L1730" class="blob-num js-line-number" data-line-number="1730"></td>
        <td id="LC1730" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVROverlayError</span> (OPENVR_FNTABLE_CALLTYPE *ShowKeyboard)(EGamepadTextInputMode eInputMode, EGamepadTextInputLineMode eLineInputMode, <span class="pl-k">char</span> * pchDescription, <span class="pl-c1">uint32_t</span> unCharMax, <span class="pl-k">char</span> * pchExistingText, <span class="pl-k">bool</span> bUseMinimalMode, <span class="pl-c1">uint64_t</span> uUserValue);</td>
      </tr>
      <tr>
        <td id="L1731" class="blob-num js-line-number" data-line-number="1731"></td>
        <td id="LC1731" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVROverlayError</span> (OPENVR_FNTABLE_CALLTYPE *ShowKeyboardForOverlay)(VROverlayHandle_t ulOverlayHandle, EGamepadTextInputMode eInputMode, EGamepadTextInputLineMode eLineInputMode, <span class="pl-k">char</span> * pchDescription, <span class="pl-c1">uint32_t</span> unCharMax, <span class="pl-k">char</span> * pchExistingText, <span class="pl-k">bool</span> bUseMinimalMode, <span class="pl-c1">uint64_t</span> uUserValue);</td>
      </tr>
      <tr>
        <td id="L1732" class="blob-num js-line-number" data-line-number="1732"></td>
        <td id="LC1732" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint32_t</span> (OPENVR_FNTABLE_CALLTYPE *GetKeyboardText)(<span class="pl-k">char</span> * pchText, <span class="pl-c1">uint32_t</span> cchText);</td>
      </tr>
      <tr>
        <td id="L1733" class="blob-num js-line-number" data-line-number="1733"></td>
        <td id="LC1733" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">void</span> (OPENVR_FNTABLE_CALLTYPE *HideKeyboard)();</td>
      </tr>
      <tr>
        <td id="L1734" class="blob-num js-line-number" data-line-number="1734"></td>
        <td id="LC1734" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">void</span> (OPENVR_FNTABLE_CALLTYPE *SetKeyboardTransformAbsolute)(ETrackingUniverseOrigin eTrackingOrigin, <span class="pl-k">struct</span> HmdMatrix34_t * pmatTrackingOriginToKeyboardTransform);</td>
      </tr>
      <tr>
        <td id="L1735" class="blob-num js-line-number" data-line-number="1735"></td>
        <td id="LC1735" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">void</span> (OPENVR_FNTABLE_CALLTYPE *SetKeyboardPositionForOverlay)(VROverlayHandle_t ulOverlayHandle, <span class="pl-k">struct</span> HmdRect2_t avoidRect);</td>
      </tr>
      <tr>
        <td id="L1736" class="blob-num js-line-number" data-line-number="1736"></td>
        <td id="LC1736" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVROverlayError</span> (OPENVR_FNTABLE_CALLTYPE *SetOverlayIntersectionMask)(VROverlayHandle_t ulOverlayHandle, <span class="pl-k">struct</span> VROverlayIntersectionMaskPrimitive_t * pMaskPrimitives, <span class="pl-c1">uint32_t</span> unNumMaskPrimitives, <span class="pl-c1">uint32_t</span> unPrimitiveSize);</td>
      </tr>
      <tr>
        <td id="L1737" class="blob-num js-line-number" data-line-number="1737"></td>
        <td id="LC1737" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVROverlayError</span> (OPENVR_FNTABLE_CALLTYPE *GetOverlayFlags)(VROverlayHandle_t ulOverlayHandle, <span class="pl-c1">uint32_t</span> * pFlags);</td>
      </tr>
      <tr>
        <td id="L1738" class="blob-num js-line-number" data-line-number="1738"></td>
        <td id="LC1738" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">VRMessageOverlayResponse</span> (OPENVR_FNTABLE_CALLTYPE *ShowMessageOverlay)(<span class="pl-k">char</span> * pchText, <span class="pl-k">char</span> * pchCaption, <span class="pl-k">char</span> * pchButton0Text, <span class="pl-k">char</span> * pchButton1Text, <span class="pl-k">char</span> * pchButton2Text, <span class="pl-k">char</span> * pchButton3Text);</td>
      </tr>
      <tr>
        <td id="L1739" class="blob-num js-line-number" data-line-number="1739"></td>
        <td id="LC1739" class="blob-code blob-code-inner js-file-line">};</td>
      </tr>
      <tr>
        <td id="L1740" class="blob-num js-line-number" data-line-number="1740"></td>
        <td id="LC1740" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L1741" class="blob-num js-line-number" data-line-number="1741"></td>
        <td id="LC1741" class="blob-code blob-code-inner js-file-line"><span class="pl-k">struct</span> VR_IVRRenderModels_FnTable</td>
      </tr>
      <tr>
        <td id="L1742" class="blob-num js-line-number" data-line-number="1742"></td>
        <td id="LC1742" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L1743" class="blob-num js-line-number" data-line-number="1743"></td>
        <td id="LC1743" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVRRenderModelError</span> (OPENVR_FNTABLE_CALLTYPE *LoadRenderModel_Async)(<span class="pl-k">char</span> * pchRenderModelName, <span class="pl-k">struct</span> RenderModel_t ** ppRenderModel);</td>
      </tr>
      <tr>
        <td id="L1744" class="blob-num js-line-number" data-line-number="1744"></td>
        <td id="LC1744" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">void</span> (OPENVR_FNTABLE_CALLTYPE *FreeRenderModel)(<span class="pl-k">struct</span> RenderModel_t * pRenderModel);</td>
      </tr>
      <tr>
        <td id="L1745" class="blob-num js-line-number" data-line-number="1745"></td>
        <td id="LC1745" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVRRenderModelError</span> (OPENVR_FNTABLE_CALLTYPE *LoadTexture_Async)(TextureID_t textureId, <span class="pl-k">struct</span> RenderModel_TextureMap_t ** ppTexture);</td>
      </tr>
      <tr>
        <td id="L1746" class="blob-num js-line-number" data-line-number="1746"></td>
        <td id="LC1746" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">void</span> (OPENVR_FNTABLE_CALLTYPE *FreeTexture)(<span class="pl-k">struct</span> RenderModel_TextureMap_t * pTexture);</td>
      </tr>
      <tr>
        <td id="L1747" class="blob-num js-line-number" data-line-number="1747"></td>
        <td id="LC1747" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVRRenderModelError</span> (OPENVR_FNTABLE_CALLTYPE *LoadTextureD3D11_Async)(TextureID_t textureId, <span class="pl-k">void</span> * pD3D11Device, <span class="pl-k">void</span> ** ppD3D11Texture2D);</td>
      </tr>
      <tr>
        <td id="L1748" class="blob-num js-line-number" data-line-number="1748"></td>
        <td id="LC1748" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVRRenderModelError</span> (OPENVR_FNTABLE_CALLTYPE *LoadIntoTextureD3D11_Async)(TextureID_t textureId, <span class="pl-k">void</span> * pDstTexture);</td>
      </tr>
      <tr>
        <td id="L1749" class="blob-num js-line-number" data-line-number="1749"></td>
        <td id="LC1749" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">void</span> (OPENVR_FNTABLE_CALLTYPE *FreeTextureD3D11)(<span class="pl-k">void</span> * pD3D11Texture2D);</td>
      </tr>
      <tr>
        <td id="L1750" class="blob-num js-line-number" data-line-number="1750"></td>
        <td id="LC1750" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint32_t</span> (OPENVR_FNTABLE_CALLTYPE *GetRenderModelName)(<span class="pl-c1">uint32_t</span> unRenderModelIndex, <span class="pl-k">char</span> * pchRenderModelName, <span class="pl-c1">uint32_t</span> unRenderModelNameLen);</td>
      </tr>
      <tr>
        <td id="L1751" class="blob-num js-line-number" data-line-number="1751"></td>
        <td id="LC1751" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint32_t</span> (OPENVR_FNTABLE_CALLTYPE *GetRenderModelCount)();</td>
      </tr>
      <tr>
        <td id="L1752" class="blob-num js-line-number" data-line-number="1752"></td>
        <td id="LC1752" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint32_t</span> (OPENVR_FNTABLE_CALLTYPE *GetComponentCount)(<span class="pl-k">char</span> * pchRenderModelName);</td>
      </tr>
      <tr>
        <td id="L1753" class="blob-num js-line-number" data-line-number="1753"></td>
        <td id="LC1753" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint32_t</span> (OPENVR_FNTABLE_CALLTYPE *GetComponentName)(<span class="pl-k">char</span> * pchRenderModelName, <span class="pl-c1">uint32_t</span> unComponentIndex, <span class="pl-k">char</span> * pchComponentName, <span class="pl-c1">uint32_t</span> unComponentNameLen);</td>
      </tr>
      <tr>
        <td id="L1754" class="blob-num js-line-number" data-line-number="1754"></td>
        <td id="LC1754" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint64_t</span> (OPENVR_FNTABLE_CALLTYPE *GetComponentButtonMask)(<span class="pl-k">char</span> * pchRenderModelName, <span class="pl-k">char</span> * pchComponentName);</td>
      </tr>
      <tr>
        <td id="L1755" class="blob-num js-line-number" data-line-number="1755"></td>
        <td id="LC1755" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint32_t</span> (OPENVR_FNTABLE_CALLTYPE *GetComponentRenderModelName)(<span class="pl-k">char</span> * pchRenderModelName, <span class="pl-k">char</span> * pchComponentName, <span class="pl-k">char</span> * pchComponentRenderModelName, <span class="pl-c1">uint32_t</span> unComponentRenderModelNameLen);</td>
      </tr>
      <tr>
        <td id="L1756" class="blob-num js-line-number" data-line-number="1756"></td>
        <td id="LC1756" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">bool</span> (OPENVR_FNTABLE_CALLTYPE *GetComponentState)(<span class="pl-k">char</span> * pchRenderModelName, <span class="pl-k">char</span> * pchComponentName, VRControllerState_t * pControllerState, <span class="pl-k">struct</span> RenderModel_ControllerMode_State_t * pState, <span class="pl-k">struct</span> RenderModel_ComponentState_t * pComponentState);</td>
      </tr>
      <tr>
        <td id="L1757" class="blob-num js-line-number" data-line-number="1757"></td>
        <td id="LC1757" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">bool</span> (OPENVR_FNTABLE_CALLTYPE *RenderModelHasComponent)(<span class="pl-k">char</span> * pchRenderModelName, <span class="pl-k">char</span> * pchComponentName);</td>
      </tr>
      <tr>
        <td id="L1758" class="blob-num js-line-number" data-line-number="1758"></td>
        <td id="LC1758" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint32_t</span> (OPENVR_FNTABLE_CALLTYPE *GetRenderModelThumbnailURL)(<span class="pl-k">char</span> * pchRenderModelName, <span class="pl-k">char</span> * pchThumbnailURL, <span class="pl-c1">uint32_t</span> unThumbnailURLLen, EVRRenderModelError * peError);</td>
      </tr>
      <tr>
        <td id="L1759" class="blob-num js-line-number" data-line-number="1759"></td>
        <td id="LC1759" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint32_t</span> (OPENVR_FNTABLE_CALLTYPE *GetRenderModelOriginalPath)(<span class="pl-k">char</span> * pchRenderModelName, <span class="pl-k">char</span> * pchOriginalPath, <span class="pl-c1">uint32_t</span> unOriginalPathLen, EVRRenderModelError * peError);</td>
      </tr>
      <tr>
        <td id="L1760" class="blob-num js-line-number" data-line-number="1760"></td>
        <td id="LC1760" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">char</span> * (OPENVR_FNTABLE_CALLTYPE *GetRenderModelErrorNameFromEnum)(EVRRenderModelError error);</td>
      </tr>
      <tr>
        <td id="L1761" class="blob-num js-line-number" data-line-number="1761"></td>
        <td id="LC1761" class="blob-code blob-code-inner js-file-line">};</td>
      </tr>
      <tr>
        <td id="L1762" class="blob-num js-line-number" data-line-number="1762"></td>
        <td id="LC1762" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L1763" class="blob-num js-line-number" data-line-number="1763"></td>
        <td id="LC1763" class="blob-code blob-code-inner js-file-line"><span class="pl-k">struct</span> VR_IVRNotifications_FnTable</td>
      </tr>
      <tr>
        <td id="L1764" class="blob-num js-line-number" data-line-number="1764"></td>
        <td id="LC1764" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L1765" class="blob-num js-line-number" data-line-number="1765"></td>
        <td id="LC1765" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVRNotificationError</span> (OPENVR_FNTABLE_CALLTYPE *CreateNotification)(VROverlayHandle_t ulOverlayHandle, <span class="pl-c1">uint64_t</span> ulUserValue, EVRNotificationType type, <span class="pl-k">char</span> * pchText, EVRNotificationStyle style, <span class="pl-k">struct</span> NotificationBitmap_t * pImage, VRNotificationId * pNotificationId);</td>
      </tr>
      <tr>
        <td id="L1766" class="blob-num js-line-number" data-line-number="1766"></td>
        <td id="LC1766" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVRNotificationError</span> (OPENVR_FNTABLE_CALLTYPE *RemoveNotification)(VRNotificationId notificationId);</td>
      </tr>
      <tr>
        <td id="L1767" class="blob-num js-line-number" data-line-number="1767"></td>
        <td id="LC1767" class="blob-code blob-code-inner js-file-line">};</td>
      </tr>
      <tr>
        <td id="L1768" class="blob-num js-line-number" data-line-number="1768"></td>
        <td id="LC1768" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L1769" class="blob-num js-line-number" data-line-number="1769"></td>
        <td id="LC1769" class="blob-code blob-code-inner js-file-line"><span class="pl-k">struct</span> VR_IVRSettings_FnTable</td>
      </tr>
      <tr>
        <td id="L1770" class="blob-num js-line-number" data-line-number="1770"></td>
        <td id="LC1770" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L1771" class="blob-num js-line-number" data-line-number="1771"></td>
        <td id="LC1771" class="blob-code blob-code-inner js-file-line">	<span class="pl-k">char</span> * (OPENVR_FNTABLE_CALLTYPE *GetSettingsErrorNameFromEnum)(EVRSettingsError eError);</td>
      </tr>
      <tr>
        <td id="L1772" class="blob-num js-line-number" data-line-number="1772"></td>
        <td id="LC1772" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">bool</span> (OPENVR_FNTABLE_CALLTYPE *Sync)(<span class="pl-k">bool</span> bForce, EVRSettingsError * peError);</td>
      </tr>
      <tr>
        <td id="L1773" class="blob-num js-line-number" data-line-number="1773"></td>
        <td id="LC1773" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">void</span> (OPENVR_FNTABLE_CALLTYPE *SetBool)(<span class="pl-k">char</span> * pchSection, <span class="pl-k">char</span> * pchSettingsKey, <span class="pl-k">bool</span> bValue, EVRSettingsError * peError);</td>
      </tr>
      <tr>
        <td id="L1774" class="blob-num js-line-number" data-line-number="1774"></td>
        <td id="LC1774" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">void</span> (OPENVR_FNTABLE_CALLTYPE *SetInt32)(<span class="pl-k">char</span> * pchSection, <span class="pl-k">char</span> * pchSettingsKey, <span class="pl-c1">int32_t</span> nValue, EVRSettingsError * peError);</td>
      </tr>
      <tr>
        <td id="L1775" class="blob-num js-line-number" data-line-number="1775"></td>
        <td id="LC1775" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">void</span> (OPENVR_FNTABLE_CALLTYPE *SetFloat)(<span class="pl-k">char</span> * pchSection, <span class="pl-k">char</span> * pchSettingsKey, <span class="pl-k">float</span> flValue, EVRSettingsError * peError);</td>
      </tr>
      <tr>
        <td id="L1776" class="blob-num js-line-number" data-line-number="1776"></td>
        <td id="LC1776" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">void</span> (OPENVR_FNTABLE_CALLTYPE *SetString)(<span class="pl-k">char</span> * pchSection, <span class="pl-k">char</span> * pchSettingsKey, <span class="pl-k">char</span> * pchValue, EVRSettingsError * peError);</td>
      </tr>
      <tr>
        <td id="L1777" class="blob-num js-line-number" data-line-number="1777"></td>
        <td id="LC1777" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">bool</span> (OPENVR_FNTABLE_CALLTYPE *GetBool)(<span class="pl-k">char</span> * pchSection, <span class="pl-k">char</span> * pchSettingsKey, EVRSettingsError * peError);</td>
      </tr>
      <tr>
        <td id="L1778" class="blob-num js-line-number" data-line-number="1778"></td>
        <td id="LC1778" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">int32_t</span> (OPENVR_FNTABLE_CALLTYPE *GetInt32)(<span class="pl-k">char</span> * pchSection, <span class="pl-k">char</span> * pchSettingsKey, EVRSettingsError * peError);</td>
      </tr>
      <tr>
        <td id="L1779" class="blob-num js-line-number" data-line-number="1779"></td>
        <td id="LC1779" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">float</span> (OPENVR_FNTABLE_CALLTYPE *GetFloat)(<span class="pl-k">char</span> * pchSection, <span class="pl-k">char</span> * pchSettingsKey, EVRSettingsError * peError);</td>
      </tr>
      <tr>
        <td id="L1780" class="blob-num js-line-number" data-line-number="1780"></td>
        <td id="LC1780" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">void</span> (OPENVR_FNTABLE_CALLTYPE *GetString)(<span class="pl-k">char</span> * pchSection, <span class="pl-k">char</span> * pchSettingsKey, <span class="pl-k">char</span> * pchValue, <span class="pl-c1">uint32_t</span> unValueLen, EVRSettingsError * peError);</td>
      </tr>
      <tr>
        <td id="L1781" class="blob-num js-line-number" data-line-number="1781"></td>
        <td id="LC1781" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">void</span> (OPENVR_FNTABLE_CALLTYPE *RemoveSection)(<span class="pl-k">char</span> * pchSection, EVRSettingsError * peError);</td>
      </tr>
      <tr>
        <td id="L1782" class="blob-num js-line-number" data-line-number="1782"></td>
        <td id="LC1782" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">void</span> (OPENVR_FNTABLE_CALLTYPE *RemoveKeyInSection)(<span class="pl-k">char</span> * pchSection, <span class="pl-k">char</span> * pchSettingsKey, EVRSettingsError * peError);</td>
      </tr>
      <tr>
        <td id="L1783" class="blob-num js-line-number" data-line-number="1783"></td>
        <td id="LC1783" class="blob-code blob-code-inner js-file-line">};</td>
      </tr>
      <tr>
        <td id="L1784" class="blob-num js-line-number" data-line-number="1784"></td>
        <td id="LC1784" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L1785" class="blob-num js-line-number" data-line-number="1785"></td>
        <td id="LC1785" class="blob-code blob-code-inner js-file-line"><span class="pl-k">struct</span> VR_IVRScreenshots_FnTable</td>
      </tr>
      <tr>
        <td id="L1786" class="blob-num js-line-number" data-line-number="1786"></td>
        <td id="LC1786" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L1787" class="blob-num js-line-number" data-line-number="1787"></td>
        <td id="LC1787" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVRScreenshotError</span> (OPENVR_FNTABLE_CALLTYPE *RequestScreenshot)(ScreenshotHandle_t * pOutScreenshotHandle, EVRScreenshotType type, <span class="pl-k">char</span> * pchPreviewFilename, <span class="pl-k">char</span> * pchVRFilename);</td>
      </tr>
      <tr>
        <td id="L1788" class="blob-num js-line-number" data-line-number="1788"></td>
        <td id="LC1788" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVRScreenshotError</span> (OPENVR_FNTABLE_CALLTYPE *HookScreenshot)(EVRScreenshotType * pSupportedTypes, <span class="pl-k">int</span> numTypes);</td>
      </tr>
      <tr>
        <td id="L1789" class="blob-num js-line-number" data-line-number="1789"></td>
        <td id="LC1789" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVRScreenshotType</span> (OPENVR_FNTABLE_CALLTYPE *GetScreenshotPropertyType)(ScreenshotHandle_t screenshotHandle, EVRScreenshotError * pError);</td>
      </tr>
      <tr>
        <td id="L1790" class="blob-num js-line-number" data-line-number="1790"></td>
        <td id="LC1790" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint32_t</span> (OPENVR_FNTABLE_CALLTYPE *GetScreenshotPropertyFilename)(ScreenshotHandle_t screenshotHandle, EVRScreenshotPropertyFilenames filenameType, <span class="pl-k">char</span> * pchFilename, <span class="pl-c1">uint32_t</span> cchFilename, EVRScreenshotError * pError);</td>
      </tr>
      <tr>
        <td id="L1791" class="blob-num js-line-number" data-line-number="1791"></td>
        <td id="LC1791" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVRScreenshotError</span> (OPENVR_FNTABLE_CALLTYPE *UpdateScreenshotProgress)(ScreenshotHandle_t screenshotHandle, <span class="pl-k">float</span> flProgress);</td>
      </tr>
      <tr>
        <td id="L1792" class="blob-num js-line-number" data-line-number="1792"></td>
        <td id="LC1792" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVRScreenshotError</span> (OPENVR_FNTABLE_CALLTYPE *TakeStereoScreenshot)(ScreenshotHandle_t * pOutScreenshotHandle, <span class="pl-k">char</span> * pchPreviewFilename, <span class="pl-k">char</span> * pchVRFilename);</td>
      </tr>
      <tr>
        <td id="L1793" class="blob-num js-line-number" data-line-number="1793"></td>
        <td id="LC1793" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">EVRScreenshotError</span> (OPENVR_FNTABLE_CALLTYPE *SubmitScreenshot)(ScreenshotHandle_t screenshotHandle, EVRScreenshotType type, <span class="pl-k">char</span> * pchSourcePreviewFilename, <span class="pl-k">char</span> * pchSourceVRFilename);</td>
      </tr>
      <tr>
        <td id="L1794" class="blob-num js-line-number" data-line-number="1794"></td>
        <td id="LC1794" class="blob-code blob-code-inner js-file-line">};</td>
      </tr>
      <tr>
        <td id="L1795" class="blob-num js-line-number" data-line-number="1795"></td>
        <td id="LC1795" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L1796" class="blob-num js-line-number" data-line-number="1796"></td>
        <td id="LC1796" class="blob-code blob-code-inner js-file-line"><span class="pl-k">struct</span> VR_IVRResources_FnTable</td>
      </tr>
      <tr>
        <td id="L1797" class="blob-num js-line-number" data-line-number="1797"></td>
        <td id="LC1797" class="blob-code blob-code-inner js-file-line">{</td>
      </tr>
      <tr>
        <td id="L1798" class="blob-num js-line-number" data-line-number="1798"></td>
        <td id="LC1798" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint32_t</span> (OPENVR_FNTABLE_CALLTYPE *LoadSharedResource)(<span class="pl-k">char</span> * pchResourceName, <span class="pl-k">char</span> * pchBuffer, <span class="pl-c1">uint32_t</span> unBufferLen);</td>
      </tr>
      <tr>
        <td id="L1799" class="blob-num js-line-number" data-line-number="1799"></td>
        <td id="LC1799" class="blob-code blob-code-inner js-file-line">	<span class="pl-c1">uint32_t</span> (OPENVR_FNTABLE_CALLTYPE *GetResourceFullPath)(<span class="pl-k">char</span> * pchResourceName, <span class="pl-k">char</span> * pchResourceTypeDirectory, <span class="pl-k">char</span> * pchPathBuffer, <span class="pl-c1">uint32_t</span> unBufferLen);</td>
      </tr>
      <tr>
        <td id="L1800" class="blob-num js-line-number" data-line-number="1800"></td>
        <td id="LC1800" class="blob-code blob-code-inner js-file-line">};</td>
      </tr>
      <tr>
        <td id="L1801" class="blob-num js-line-number" data-line-number="1801"></td>
        <td id="LC1801" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L1802" class="blob-num js-line-number" data-line-number="1802"></td>
        <td id="LC1802" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L1803" class="blob-num js-line-number" data-line-number="1803"></td>
        <td id="LC1803" class="blob-code blob-code-inner js-file-line">#<span class="pl-k">if</span> <span class="pl-c1">0</span><span class="pl-c"></span></td>
      </tr>
      <tr>
        <td id="L1804" class="blob-num js-line-number" data-line-number="1804"></td>
        <td id="LC1804" class="blob-code blob-code-inner js-file-line"><span class="pl-c">// Global entry points</span></td>
      </tr>
      <tr>
        <td id="L1805" class="blob-num js-line-number" data-line-number="1805"></td>
        <td id="LC1805" class="blob-code blob-code-inner js-file-line"><span class="pl-c">S_API intptr_t VR_InitInternal( EVRInitError *peError, EVRApplicationType eType );</span></td>
      </tr>
      <tr>
        <td id="L1806" class="blob-num js-line-number" data-line-number="1806"></td>
        <td id="LC1806" class="blob-code blob-code-inner js-file-line"><span class="pl-c">S_API void VR_ShutdownInternal();</span></td>
      </tr>
      <tr>
        <td id="L1807" class="blob-num js-line-number" data-line-number="1807"></td>
        <td id="LC1807" class="blob-code blob-code-inner js-file-line"><span class="pl-c">S_API bool VR_IsHmdPresent();</span></td>
      </tr>
      <tr>
        <td id="L1808" class="blob-num js-line-number" data-line-number="1808"></td>
        <td id="LC1808" class="blob-code blob-code-inner js-file-line"><span class="pl-c">S_API intptr_t VR_GetGenericInterface( const char *pchInterfaceVersion, EVRInitError *peError );</span></td>
      </tr>
      <tr>
        <td id="L1809" class="blob-num js-line-number" data-line-number="1809"></td>
        <td id="LC1809" class="blob-code blob-code-inner js-file-line"><span class="pl-c">S_API bool VR_IsRuntimeInstalled();</span></td>
      </tr>
      <tr>
        <td id="L1810" class="blob-num js-line-number" data-line-number="1810"></td>
        <td id="LC1810" class="blob-code blob-code-inner js-file-line"><span class="pl-c">S_API const char * VR_GetVRInitErrorAsSymbol( EVRInitError error );</span></td>
      </tr>
      <tr>
        <td id="L1811" class="blob-num js-line-number" data-line-number="1811"></td>
        <td id="LC1811" class="blob-code blob-code-inner js-file-line"><span class="pl-c">S_API const char * VR_GetVRInitErrorAsEnglishDescription( EVRInitError error );</span></td>
      </tr>
      <tr>
        <td id="L1812" class="blob-num js-line-number" data-line-number="1812"></td>
        <td id="LC1812" class="blob-code blob-code-inner js-file-line"><span class="pl-c"></span>#<span class="pl-k">endif</span></td>
      </tr>
      <tr>
        <td id="L1813" class="blob-num js-line-number" data-line-number="1813"></td>
        <td id="LC1813" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L1814" class="blob-num js-line-number" data-line-number="1814"></td>
        <td id="LC1814" class="blob-code blob-code-inner js-file-line">#<span class="pl-k">endif</span> <span class="pl-c"><span class="pl-c">//</span> __OPENVR_API_FLAT_H__</span></td>
      </tr>
      <tr>
        <td id="L1815" class="blob-num js-line-number" data-line-number="1815"></td>
        <td id="LC1815" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
      <tr>
        <td id="L1816" class="blob-num js-line-number" data-line-number="1816"></td>
        <td id="LC1816" class="blob-code blob-code-inner js-file-line">
</td>
      </tr>
</table>

  </div>

</div>

<button type="button" data-facebox="#jump-to-line" data-facebox-class="linejump" data-hotkey="l" class="d-none">Jump to Line</button>
<div id="jump-to-line" style="display:none">
  <!-- '"` --><!-- </textarea></xmp> --></option></form><form accept-charset="UTF-8" action="" class="js-jump-to-line-form" method="get"><div style="margin:0;padding:0;display:inline"><input name="utf8" type="hidden" value="&#x2713;" /></div>
    <input class="form-control linejump-input js-jump-to-line-field" type="text" placeholder="Jump to line&hellip;" aria-label="Jump to line" autofocus>
    <button type="submit" class="btn">Go</button>
</form></div>

  </div>
  <div class="modal-backdrop js-touch-events"></div>
</div>


    </div>
  </div>

    </div>

        <div class="container site-footer-container">
  <div class="site-footer" role="contentinfo">
    <ul class="site-footer-links float-right">
        <li><a href="https://github.com/contact" data-ga-click="Footer, go to contact, text:contact">Contact GitHub</a></li>
      <li><a href="https://developer.github.com" data-ga-click="Footer, go to api, text:api">API</a></li>
      <li><a href="https://training.github.com" data-ga-click="Footer, go to training, text:training">Training</a></li>
      <li><a href="https://shop.github.com" data-ga-click="Footer, go to shop, text:shop">Shop</a></li>
        <li><a href="https://github.com/blog" data-ga-click="Footer, go to blog, text:blog">Blog</a></li>
        <li><a href="https://github.com/about" data-ga-click="Footer, go to about, text:about">About</a></li>

    </ul>

    <a href="https://github.com" aria-label="Homepage" class="site-footer-mark" title="GitHub">
      <svg aria-hidden="true" class="octicon octicon-mark-github" height="24" version="1.1" viewBox="0 0 16 16" width="24"><path fill-rule="evenodd" d="M8 0C3.58 0 0 3.58 0 8c0 3.54 2.29 6.53 5.47 7.59.4.07.55-.17.55-.38 0-.19-.01-.82-.01-1.49-2.01.37-2.53-.49-2.69-.94-.09-.23-.48-.94-.82-1.13-.28-.15-.68-.52-.01-.53.63-.01 1.08.58 1.23.82.72 1.21 1.87.87 2.33.66.07-.52.28-.87.51-1.07-1.78-.2-3.64-.89-3.64-3.95 0-.87.31-1.59.82-2.15-.08-.2-.36-1.02.08-2.12 0 0 .67-.21 2.2.82.64-.18 1.32-.27 2-.27.68 0 1.36.09 2 .27 1.53-1.04 2.2-.82 2.2-.82.44 1.1.16 1.92.08 2.12.51.56.82 1.27.82 2.15 0 3.07-1.87 3.75-3.65 3.95.29.25.54.73.54 1.48 0 1.07-.01 1.93-.01 2.2 0 .21.15.46.55.38A8.013 8.013 0 0 0 16 8c0-4.42-3.58-8-8-8z"/></svg>
</a>
    <ul class="site-footer-links">
      <li>&copy; 2017 <span title="0.19031s from github-fe164-cp1-prd.iad.github.net">GitHub</span>, Inc.</li>
        <li><a href="https://github.com/site/terms" data-ga-click="Footer, go to terms, text:terms">Terms</a></li>
        <li><a href="https://github.com/site/privacy" data-ga-click="Footer, go to privacy, text:privacy">Privacy</a></li>
        <li><a href="https://github.com/security" data-ga-click="Footer, go to security, text:security">Security</a></li>
        <li><a href="https://status.github.com/" data-ga-click="Footer, go to status, text:status">Status</a></li>
        <li><a href="https://help.github.com" data-ga-click="Footer, go to help, text:help">Help</a></li>
    </ul>
  </div>
</div>



    

    <div id="ajax-error-message" class="ajax-error-message flash flash-error">
      <svg aria-hidden="true" class="octicon octicon-alert" height="16" version="1.1" viewBox="0 0 16 16" width="16"><path fill-rule="evenodd" d="M8.865 1.52c-.18-.31-.51-.5-.87-.5s-.69.19-.87.5L.275 13.5c-.18.31-.18.69 0 1 .19.31.52.5.87.5h13.7c.36 0 .69-.19.86-.5.17-.31.18-.69.01-1L8.865 1.52zM8.995 13h-2v-2h2v2zm0-3h-2V6h2v4z"/></svg>
      <button type="button" class="flash-close js-flash-close js-ajax-error-dismiss" aria-label="Dismiss error">
        <svg aria-hidden="true" class="octicon octicon-x" height="16" version="1.1" viewBox="0 0 12 16" width="12"><path fill-rule="evenodd" d="M7.48 8l3.75 3.75-1.48 1.48L6 9.48l-3.75 3.75-1.48-1.48L4.52 8 .77 4.25l1.48-1.48L6 6.52l3.75-3.75 1.48 1.48z"/></svg>
      </button>
      You can't perform that action at this time.
    </div>


      
      <script crossorigin="anonymous" integrity="sha256-0j4y5IIRK6Xj6pvY3H5VVoARanTgAnRqyR9BpOWHWps=" src="https://assets-cdn.github.com/assets/frameworks-d23e32e482112ba5e3ea9bd8dc7e555680116a74e002746ac91f41a4e5875a9b.js"></script>
      <script async="async" crossorigin="anonymous" integrity="sha256-h/2E6sW/Ldyuzs25de6nplLfQ24krsp2zrBVuoKrpo8=" src="https://assets-cdn.github.com/assets/github-87fd84eac5bf2ddcaececdb975eea7a652df436e24aeca76ceb055ba82aba68f.js"></script>
      
      
      
      
    <div class="js-stale-session-flash stale-session-flash flash flash-warn flash-banner d-none">
      <svg aria-hidden="true" class="octicon octicon-alert" height="16" version="1.1" viewBox="0 0 16 16" width="16"><path fill-rule="evenodd" d="M8.865 1.52c-.18-.31-.51-.5-.87-.5s-.69.19-.87.5L.275 13.5c-.18.31-.18.69 0 1 .19.31.52.5.87.5h13.7c.36 0 .69-.19.86-.5.17-.31.18-.69.01-1L8.865 1.52zM8.995 13h-2v-2h2v2zm0-3h-2V6h2v4z"/></svg>
      <span class="signed-in-tab-flash">You signed in with another tab or window. <a href="">Reload</a> to refresh your session.</span>
      <span class="signed-out-tab-flash">You signed out in another tab or window. <a href="">Reload</a> to refresh your session.</span>
    </div>
    <div class="facebox" id="facebox" style="display:none;">
  <div class="facebox-popup">
    <div class="facebox-content" role="dialog" aria-labelledby="facebox-header" aria-describedby="facebox-description">
    </div>
    <button type="button" class="facebox-close js-facebox-close" aria-label="Close modal">
      <svg aria-hidden="true" class="octicon octicon-x" height="16" version="1.1" viewBox="0 0 12 16" width="12"><path fill-rule="evenodd" d="M7.48 8l3.75 3.75-1.48 1.48L6 9.48l-3.75 3.75-1.48-1.48L4.52 8 .77 4.25l1.48-1.48L6 6.52l3.75-3.75 1.48 1.48z"/></svg>
    </button>
  </div>
</div>

  </body>
</html>

